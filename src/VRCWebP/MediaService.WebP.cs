using FFMpegCore;
using System.Buffers.Binary;
using System.Globalization;

namespace VRCWebP;

internal sealed partial class MediaService
{
    /// <summary>Streams bounded lossy or lossless frame ranges, preserving alpha; lossless guarantees decoded output pixels.</summary>
    private static async Task EncodeWebPAsync(string Binary, string Input, string Output, string Filter, ConversionSettings Settings,
        IProgress<WorkProgress>? Progress, MediaWorkerLimit Limit, int First, int Count, CancellationToken Token)
    {
        bool StreamImage = IsWebPInput(Input);

        string Trim = StreamImage ? "trim=start=" + Settings.Start.ToString("R", CultureInfo.InvariantCulture)
            + ":duration=" + Settings.Duration.ToString("R", CultureInfo.InvariantCulture) + ",setpts=PTS-STARTPTS," : "";

        // Nearest timestamps preserve cadence when a container quantizes frame times.
        // Passing EOF retains a partial final interval; the explicit range still bounds frame count.
        string Sampling = $"{Trim}fps={Settings.FrameRate}:round=near:eof_action=pass,trim=start_frame={First}:end_frame={First + Count},setpts=PTS-STARTPTS,{Filter}";

        FFMpegArgumentProcessor Command = FFMpegArguments.FromFileInput(Input, true, Options =>
            {
                if (!StreamImage && Settings.Start > 0)
                {
                    _ = Options.Seek(TimeSpan.FromSeconds(Settings.Start));
                }
            })
            .OutputToFile(Output, true, Options => Options
                .WithVideoCodec("libwebp")
                .WithDuration(TimeSpan.FromSeconds(Settings.Duration))
                .ForcePixelFormat("bgra")
                .WithCustomArgument($"-map 0:v:0 -an -sn -dn -map_metadata -1 -map_chapters -1 -vf \"{Sampling}\" -frames:v {Count} -lossless {(Settings.Lossless ? 1 : 0)} -quality {(Settings.Lossless ? (Settings.Compression is 0 ? 0 : 75) : Settings.Quality)} -compression_level {(Settings.Lossless ? Settings.Compression : 0)} -loop {Settings.Loops} -f webp"));

        _ = await RunAsync(Binary, "-hide_banner -nostdin -v error -threads 2 -filter_threads 2 -stats_period 0.1 -progress pipe:1 -nostats " + Command.Arguments, Line =>
        {
            if (Line.StartsWith("out_time_us=", StringComparison.Ordinal) && long.TryParse(Line.AsSpan(12), CultureInfo.InvariantCulture, out long Microseconds))
            {
                Progress?.Report(new WorkProgress(Math.Clamp(Microseconds / (Settings.Duration * 10000d), 0, 99.9), "Encoding WebP"));

                if (new FileInfo(Output).Length > uint.MaxValue)
                {
                    throw new InvalidDataException("The animation exceeds WebP's 4 GiB container capacity. Reduce its dimensions, duration or frame rate.");
                }
            }
        }, Token, Limit);

        Token.ThrowIfCancellationRequested();

        Progress?.Report(new WorkProgress(99.9, "Checking animation"));

        FinalizeWebP(Output, Settings.FrameRate, First, Count, Token);
    }

    /// <summary>Marks full-canvas frames as replacements so changing transparent pixels never retain the previous image.</summary>
    private static void FinalizeWebP(string Path, int Rate, int First, int Count, CancellationToken Token)
    {
        using FileStream Output = new(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

        if (Output.Length > uint.MaxValue)
        {
            throw new InvalidDataException("The animation exceeds WebP's 4 GiB container capacity.");
        }

        Output.Position = 12;

        Span<byte> Header = stackalloc byte[8];

        int Frames = 0;

        while (Output.Position < Output.Length)
        {
            Token.ThrowIfCancellationRequested();

            Output.ReadExactly(Header);

            uint Length = BinaryPrimitives.ReadUInt32LittleEndian(Header[4..]);

            long Next = Output.Position + Length + (Length & 1);

            if (Next > Output.Length)
            {
                throw new InvalidDataException("The WebP is truncated.");
            }

            if (Header[..4].SequenceEqual("ANMF"u8) && Length >= 16)
            {
                Output.Position += 12;

                int Duration = (int)Math.Round((First + Frames + 1) * 1000d / Rate) - (int)Math.Round((First + Frames) * 1000d / Rate);

                WriteUInt24(Header, Duration);
                Header[3] = 2;
                Output.Write(Header[..4]);
                ++Frames;
            }

            Output.Position = Next;
        }

        if (Frames != Count)
        {
            throw new InvalidDataException("The source ended before all selected frames could be encoded.");
        }
    }

    /// <summary>Checks bounded RIFF chunks, canvas, animation timing, loop count and frame rectangles.</summary>
    private static ConversionResult InspectWebP(string Path, ConversionSettings Settings, CancellationToken Token)
    {
        using FileStream Input = File.OpenRead(Path);

        Span<byte> Header = stackalloc byte[16];

        Input.ReadExactly(Header[..12]);

        if (!Header[..4].SequenceEqual("RIFF"u8) || !Header[8..12].SequenceEqual("WEBP"u8)
            || BinaryPrimitives.ReadUInt32LittleEndian(Header[4..]) + 8L != Input.Length)
        {
            throw new InvalidDataException("The encoder did not produce a complete WebP container.");
        }

        int Width = 0;

        int Height = 0;

        uint Frames = 0;

        bool Animation = false;

        bool LoopChecked = false;

        long Milliseconds = 0;

        while (Input.Position < Input.Length)
        {
            Token.ThrowIfCancellationRequested();

            Input.ReadExactly(Header[..8]);

            uint Type = BinaryPrimitives.ReadUInt32LittleEndian(Header);

            uint Length = BinaryPrimitives.ReadUInt32LittleEndian(Header[4..]);

            long Next = Input.Position + Length + (Length & 1);

            if (Next > Input.Length)
            {
                throw new InvalidDataException("The WebP is truncated.");
            }

            if (Type is 0x58385056 && Length is 10)
            {
                Input.ReadExactly(Header[..10]);

                Width = ReadUInt24(Header[4..]) + 1;

                Height = ReadUInt24(Header[7..]) + 1;

                Animation = (Header[0] & 2) is not 0;
            }
            else if (Type is 0x4D494E41 && Length is 6)
            {
                Input.ReadExactly(Header[..6]);

                LoopChecked = BinaryPrimitives.ReadUInt16LittleEndian(Header[4..]) == Settings.Loops;
            }
            else if (Type is 0x464D4E41 && Length >= 16)
            {
                Input.ReadExactly(Header);

                int X = ReadUInt24(Header) * 2;

                int Y = ReadUInt24(Header[3..]) * 2;

                int FrameWidth = ReadUInt24(Header[6..]) + 1;

                int FrameHeight = ReadUInt24(Header[9..]) + 1;

                if (X + FrameWidth > Width || Y + FrameHeight > Height || ReadUInt24(Header[12..]) is 0 || (Header[15] & 0xFC) is not 0)
                {
                    throw new InvalidDataException("The WebP has invalid animation frame metadata.");
                }

                Milliseconds += ReadUInt24(Header[12..]);

                ++Frames;
            }

            Input.Position = Next;
        }

        if (!Animation || !LoopChecked || Frames < 2 || Width != Settings.Width || Height != Settings.Height)
        {
            throw new InvalidDataException("At least two animation frames are required, with the selected dimensions and loop count.");
        }

        int Expected = (int)Math.Ceiling((Settings.Duration * Settings.FrameRate) - 0.000001);

        if (Frames != Expected || Math.Abs(Milliseconds - Math.Round(Expected * 1000d / Settings.FrameRate)) > 1)
        {
            throw new InvalidDataException("The WebP frame count or timing does not match the selected clip.");
        }

        return new ConversionResult(Path, Width, Height, Frames, Input.Length, Settings);
    }

    private static int ReadUInt24(ReadOnlySpan<byte> Bytes) => Bytes[0] | (Bytes[1] << 8) | (Bytes[2] << 16);
}
