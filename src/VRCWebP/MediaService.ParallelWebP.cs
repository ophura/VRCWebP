using System.Buffers;
using System.Buffers.Binary;

namespace VRCWebP;

internal sealed partial class MediaService
{

    /// <summary>Encodes bounded contiguous frame ranges concurrently under one shared memory ceiling, then streams their frames in order.</summary>
    private static async Task EncodeParallelWebPAsync(string Binary, string Input, string Output, string Filter, ConversionSettings Settings,
        IProgress<WorkProgress>? Progress, CancellationToken Token)
    {
        int Frames = (int)Math.Ceiling((Settings.Duration * Settings.FrameRate) - 0.000001);

        long WorkerBytes = 67108864 + ((long)Settings.Width * Settings.Height * 48);

        int Workers = Math.Min(Math.Clamp(Environment.ProcessorCount / 2, 1, 4),
            Math.Min(Math.Max(1, Frames / 8), (int)Math.Max(1, 805306368 / WorkerBytes)));

        try
        {
            await EncodeRangesAsync(Binary, Input, Output, Filter, Settings, Progress, Frames, Workers, Token);
        }
        catch (InvalidDataException) when (Workers > 1 && !Token.IsCancellationRequested)
        {
            File.Delete(Output);

            Progress?.Report(new WorkProgress(0, "Retrying with one worker"));

            await EncodeRangesAsync(Binary, Input, Output, Filter, Settings, Progress, Frames, 1, Token);
        }
    }

    /// <summary>Joins every worker before cleanup or fallback; progress callbacks are serialized across all ranges.</summary>
    private static async Task EncodeRangesAsync(string Binary, string Input, string Output, string Filter, ConversionSettings Settings,
        IProgress<WorkProgress>? Progress, int Frames, int Workers, CancellationToken Token)
    {

        using MediaWorkerLimit Limit = new();

        if (Workers is 1)
        {
            await EncodeWebPAsync(Binary, Input, Output, Filter, Settings, Progress, Limit, 0, Frames, Token);

            return;
        }

        string Folder = Output + ".frames";

        Directory.CreateDirectory(Folder);

        using CancellationTokenSource Pending = CancellationTokenSource.CreateLinkedTokenSource(Token);

        double[] Completed = new double[Workers];

        string[] Paths = new string[Workers];

        object Gate = new();

        Task[] Tasks = new Task[Workers];

        try
        {
            for (int Index = 0; Index < Workers; ++Index)
            {
                int Worker = Index;

                int First = Frames * Worker / Workers;

                int Count = (Frames * (Worker + 1) / Workers) - First;

                Paths[Worker] = Path.Combine(Folder, Worker + ".webp");

                Tasks[Worker] = EncodeRangeAsync();

                async Task EncodeRangeAsync()
                {
                    try
                    {
                        RangeProgress Updates = new(Value =>
                        {
                            lock (Gate)
                            {
                                Completed[Worker] = Math.Clamp(Value.Percent * Settings.Duration * Settings.FrameRate / 100, 0, Count);

                                Progress?.Report(new WorkProgress(Math.Min(98, Completed.Sum() * 98 / Frames), "Encoding WebP"));
                            }
                        });

                        await EncodeWebPAsync(Binary, Input, Paths[Worker], Filter, Settings, Updates, Limit, First, Count, Pending.Token);

                        lock (Gate)
                        {
                            Completed[Worker] = Count;
                        }
                    }
                    catch
                    {
                        await Pending.CancelAsync();

                        throw;
                    }
                }
            }

            try
            {
                await Task.WhenAll(Tasks);
            }
            catch
            {
                Token.ThrowIfCancellationRequested();

                Exception? Failure = Tasks.FirstOrDefault(Task => Task.IsFaulted)?.Exception?.InnerException;

                throw new InvalidDataException("Parallel encoding could not complete.", Failure);
            }

            Progress?.Report(new WorkProgress(98, "Joining animation"));

            await JoinWebPAsync(Paths, Output, Settings, Frames, Token);
        }
        finally
        {
            Directory.Delete(Folder, true);
        }
    }

    /// <summary>Copies frame chunks with one pooled buffer and reconstructs exact global frame timing without decoding or recompression.</summary>
    private static async Task JoinWebPAsync(string[] Paths, string Destination, ConversionSettings Settings, int ExpectedFrames, CancellationToken Token)
    {
        await using FileStream Output = new(Destination, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 65536, FileOptions.Asynchronous);

        byte[] Buffer = ArrayPool<byte>.Shared.Rent(1048576);

        byte[] Header = new byte[44];

        "RIFF"u8.CopyTo(Header);

        "WEBPVP8X"u8.CopyTo(Header.AsSpan(8));

        BinaryPrimitives.WriteUInt32LittleEndian(Header.AsSpan(16), 10);

        Header[20] = 0x12;

        WriteUInt24(Header.AsSpan(24), Settings.Width - 1);

        WriteUInt24(Header.AsSpan(27), Settings.Height - 1);

        "ANIM"u8.CopyTo(Header.AsSpan(30));

        BinaryPrimitives.WriteUInt32LittleEndian(Header.AsSpan(34), 6);

        BinaryPrimitives.WriteUInt16LittleEndian(Header.AsSpan(42), (ushort)Settings.Loops);

        try
        {
            await Output.WriteAsync(Header, Token);

            int Frames = 0;

            foreach (string Path in Paths)
            {
                await using FileStream Input = new(Path, FileMode.Open, FileAccess.Read, FileShare.Read, 65536, FileOptions.Asynchronous);

                Input.Position = 12;

                int RangeFrames = 0;

                while (Input.Position < Input.Length)
                {
                    await Input.ReadExactlyAsync(Header.AsMemory(0, 8), Token);

                    uint Length = BinaryPrimitives.ReadUInt32LittleEndian(Header.AsSpan(4));

                    long Next = Input.Position + Length + (Length & 1);

                    if (Next > Input.Length)
                    {
                        throw new InvalidDataException("An encoded frame range is truncated.");
                    }

                    if (Header.AsSpan(0, 4).SequenceEqual("ANMF"u8) && Length >= 16)
                    {
                        if (Output.Position + 8L + Length + (Length & 1) > uint.MaxValue)
                        {
                            throw new InvalidDataException("The animation exceeds WebP's 4 GiB container capacity.");
                        }

                        await Input.ReadExactlyAsync(Header.AsMemory(8, 16), Token);

                        int Start = (int)Math.Round(Frames * 1000d / Settings.FrameRate);

                        int End = (int)Math.Round((Frames + 1) * 1000d / Settings.FrameRate);

                        WriteUInt24(Header.AsSpan(20), End - Start);

                        Header[23] = 2;

                        await Output.WriteAsync(Header.AsMemory(0, 24), Token);

                        while (Input.Position < Next)
                        {
                            int Count = await Input.ReadAsync(Buffer.AsMemory(0, (int)Math.Min(1048576, Next - Input.Position)), Token);

                            if (Count is 0)
                            {
                                throw new EndOfStreamException();
                            }

                            await Output.WriteAsync(Buffer.AsMemory(0, Count), Token);
                        }

                        ++Frames;

                        ++RangeFrames;
                    }

                    Input.Position = Next;
                }

                if (RangeFrames < 2)
                {
                    throw new InvalidDataException("An encoded frame range has too few animation frames.");
                }
            }

            if (Frames != ExpectedFrames)
            {
                throw new InvalidDataException("The source ended before all selected frames could be encoded.");
            }

            Output.Position = 4;

            BinaryPrimitives.WriteUInt32LittleEndian(Header, checked((uint)(Output.Length - 8)));

            await Output.WriteAsync(Header.AsMemory(0, 4), Token);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(Buffer, clearArray: true);
        }
    }

    private static void WriteUInt24(Span<byte> Bytes, int Value)
    {
        Bytes[0] = (byte)Value;

        Bytes[1] = (byte)(Value >> 8);

        Bytes[2] = (byte)(Value >> 16);
    }

    /// <summary>Reports worker progress synchronously so the aggregate callback remains serialized.</summary>
    private sealed class RangeProgress(Action<WorkProgress> Callback) : IProgress<WorkProgress>
    {
        public void Report(WorkProgress Value) => Callback(Value);
    }
}
