using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VRChat.API.Client;

/// <summary>A write-only HTTP sink that rejects bytes before they can exceed its owned pooled lease.</summary>
internal sealed class ResponseBuffer(int Limit) : Stream
{
    private static readonly UTF8Encoding Utf8 = new(false, true);

    private byte[] Buffer = ArrayPool<byte>.Shared.Rent(Limit);
    private int Count;

    internal string ReadUtf8() => Utf8.GetString(Buffer, 0, Count);

    public override void Write(ReadOnlySpan<byte> Bytes)
    {
        ObjectDisposedException.ThrowIf(Buffer is null, this);

        if (Bytes.Length > Limit - Count)
        {
            throw new InvalidDataException("The API response exceeds its supported byte limit.");
        }

        Bytes.CopyTo(Buffer.AsSpan(Count));
        Count += Bytes.Length;
    }

    public override void Write(byte[] Bytes, int Offset, int Length) => Write(Bytes.AsSpan(Offset, Length));

    public override ValueTask WriteAsync(ReadOnlyMemory<byte> Bytes, CancellationToken Token = default)
    {
        Token.ThrowIfCancellationRequested();
        Write(Bytes.Span);
        return ValueTask.CompletedTask;
    }

    public override Task WriteAsync(byte[] Bytes, int Offset, int Length, CancellationToken Token) =>
        WriteAsync(Bytes.AsMemory(Offset, Length), Token).AsTask();

    protected override void Dispose(bool Disposing)
    {
        byte[] Returned = Interlocked.Exchange(ref Buffer, null);

        if (Returned is not null)
        {
            ArrayPool<byte>.Shared.Return(Returned, clearArray: true);
        }

        base.Dispose(Disposing);
    }

    public override bool CanRead => false;
    public override bool CanSeek => false;
    public override bool CanWrite => Buffer is not null;
    public override long Length => Count;
    public override long Position { get => Count; set => throw new NotSupportedException(); }
    public override void Flush() { }
    public override int Read(byte[] Bytes, int Offset, int Length) => throw new NotSupportedException();
    public override long Seek(long Offset, SeekOrigin Origin) => throw new NotSupportedException();
    public override void SetLength(long Length) => throw new NotSupportedException();
}
