using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace VRCWebP;

/// <summary>Streams an exact part length and reports actual bytes written to the HTTP transport.</summary>
internal sealed class UploadContent : HttpContent
{
    private readonly Stream Source;

    private readonly long Length;

    private readonly Action<long> Report;

    internal UploadContent(Stream Source, long Length, string Mime, Action<long> Report)
    {
        this.Source = Source;

        this.Length = Length;

        this.Report = Report;

        Headers.ContentType = new MediaTypeHeaderValue(Mime);
    }

    protected override bool TryComputeLength(out long Result)
    {
        Result = Length;

        return true;
    }

    protected override Task SerializeToStreamAsync(Stream Stream, TransportContext? Context)
    {
        return SerializeToStreamAsync(Stream, Context, CancellationToken.None);
    }

    protected override async Task SerializeToStreamAsync(Stream Stream, TransportContext? Context, CancellationToken CancellationToken)
    {
        using PooledBuffer Lease = new(65536);

        Memory<byte> Buffer = Lease.Memory;

        long Written = 0;

        long LastReport = Stopwatch.GetTimestamp();

        while (Written < Length)
        {
            int Count = await Source.ReadAsync(Buffer[..(int)Math.Min(Buffer.Length, Length - Written)], CancellationToken);

            if (Count is 0)
            {
                throw new EndOfStreamException("The upload source changed while being read.");
            }

            await Stream.WriteAsync(Buffer[..Count], CancellationToken);

            Written += Count;

            long Now = Stopwatch.GetTimestamp();

            if (Written == Length || Now - LastReport >= Stopwatch.Frequency / 10)
            {
                Report(Written);

                LastReport = Now;
            }
        }
    }
}
