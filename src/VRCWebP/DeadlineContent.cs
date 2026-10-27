using System.Net;

namespace VRCWebP;

/// <summary>Transfers an admitted request deadline to its response body; disposing the response releases the deadline.</summary>
internal sealed class DeadlineContent : HttpContent
{
    private readonly HttpContent Inner;

    private readonly CancellationTokenSource Deadline;

    internal DeadlineContent(HttpContent Inner, CancellationTokenSource Deadline)
    {
        this.Inner = Inner;
        this.Deadline = Deadline;

        foreach (KeyValuePair<string, IEnumerable<string>> Header in Inner.Headers)
        {
            _ = Headers.TryAddWithoutValidation(Header.Key, Header.Value);
        }
    }

    protected override bool TryComputeLength(out long Length)
    {
        Length = Inner.Headers.ContentLength ?? 0;

        return Inner.Headers.ContentLength.HasValue;
    }

    protected override Task SerializeToStreamAsync(Stream Target, TransportContext? Context) =>
        SerializeToStreamAsync(Target, Context, CancellationToken.None);

    protected override async Task SerializeToStreamAsync(Stream Target, TransportContext? Context, CancellationToken Token)
    {
        using CancellationTokenSource Read = CancellationTokenSource.CreateLinkedTokenSource(Deadline.Token, Token);

        await Inner.CopyToAsync(Target, Read.Token).ConfigureAwait(false);

        Read.Token.ThrowIfCancellationRequested();
    }

    protected override Task<Stream> CreateContentReadStreamAsync() => CreateContentReadStreamAsync(CancellationToken.None);

    protected override async Task<Stream> CreateContentReadStreamAsync(CancellationToken Token)
    {
        using CancellationTokenSource Open = CancellationTokenSource.CreateLinkedTokenSource(Deadline.Token, Token);

        Stream Source = await Inner.ReadAsStreamAsync(Open.Token).ConfigureAwait(false);

        return new DeadlineStream(Source, Deadline.Token);
    }

    protected override void Dispose(bool Disposing)
    {
        if (Disposing)
        {
            Inner.Dispose();

            Deadline.Dispose();
        }

        base.Dispose(Disposing);
    }

    /// <summary>Links each pending read to caller cancellation and the original absolute deadline.</summary>
    private sealed class DeadlineStream(Stream Source, CancellationToken Deadline) : Stream
    {
        public override async ValueTask<int> ReadAsync(Memory<byte> Buffer, CancellationToken Token = default)
        {
            using CancellationTokenSource Read = CancellationTokenSource.CreateLinkedTokenSource(Deadline, Token);

            return await Source.ReadAsync(Buffer, Read.Token).ConfigureAwait(false);
        }

        public override Task<int> ReadAsync(byte[] Buffer, int Offset, int Count, CancellationToken Token) =>
            ReadAsync(Buffer.AsMemory(Offset, Count), Token).AsTask();

        public override int Read(byte[] Buffer, int Offset, int Count) =>
            ReadAsync(Buffer.AsMemory(Offset, Count)).AsTask().GetAwaiter().GetResult();

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                Source.Dispose();
            }

            base.Dispose(Disposing);
        }

        public override bool CanRead => Source.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();

            set => throw new NotSupportedException();
        }

        public override void Flush() { }

        public override long Seek(long Offset, SeekOrigin Origin) => throw new NotSupportedException();

        public override void SetLength(long Length) => throw new NotSupportedException();

        public override void Write(byte[] Buffer, int Offset, int Count) => throw new NotSupportedException();
    }
}
