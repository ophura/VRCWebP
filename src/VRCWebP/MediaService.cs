using FFMpegCore;
using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace VRCWebP;

/// <summary>Uses FFMpegCore's typed argument builder with embedded FFmpeg and cancellable child processes.</summary>
internal sealed partial class MediaService : IDisposable
{
    internal const int MaximumPreviewFrames = 120;

    internal string Workspace { get; } = Path.Combine(Path.GetTempPath(), "VRCWebP", Guid.NewGuid().ToString("N"));

    /// <summary>Decodes a bounded, adjustable preview to owned PNG frames for smooth UI playback.</summary>
    internal async Task<string[]> PreviewFramesAsync(string Input, ConversionSettings Settings, CancellationToken CancellationToken, double PreviewSeconds = 4,
        IProgress<WorkProgress>? Progress = null, MediaWorkerLimit? WorkerLimit = null, bool RawFrame = false)
    {
        double Seconds = Math.Min(PreviewSeconds, Settings.Duration);

        if (!double.IsFinite(Seconds) || Seconds is < 0.1 or > 60)
        {
            throw new ArgumentOutOfRangeException(nameof(PreviewSeconds), "Preview length must be between 0.1 seconds and the selected clip length, up to 60 seconds.");
        }

        int FrameBudget = Math.Clamp((int)Math.Floor((Seconds * 12) + 0.000001), 1, MaximumPreviewFrames);

        string Rate = (FrameBudget / Seconds).ToString("R", CultureInfo.InvariantCulture);

        string DirectoryPath = Path.Combine(Workspace, Guid.NewGuid().ToString("N"));

        _ = Directory.CreateDirectory(DirectoryPath);

        double Scale = Math.Min(1, 480d / Math.Max(Settings.Width, Settings.Height));

        int Width = Math.Max(16, (int)(Settings.Width * Scale));

        int Height = Math.Max(16, (int)(Settings.Height * Scale));

        string Filter = FramingFilter(Settings, Width, Height);

        if (RawFrame)
        {
            Filter = "format=rgba,scale=480:480:force_original_aspect_ratio=decrease";
        }

        bool StreamImage = IsWebPInput(Input);

        string OutputSeek = StreamImage && Settings.Start > 0 ? "-ss " + Settings.Start.ToString("R", CultureInfo.InvariantCulture) + " " : "";

        FFMpegArgumentProcessor Command = FFMpegArguments.FromFileInput(Input, true, Options =>
            {
                if (Settings.Start > 0 && !StreamImage)
                {
                    _ = Options.Seek(TimeSpan.FromSeconds(Settings.Start));
                }
            })
            .OutputToFile(Path.Combine(DirectoryPath, "frame-%03d.png"), true, Options => Options
                .WithDuration(TimeSpan.FromSeconds(Seconds))
                .ForcePixelFormat("rgba")
                .WithCustomArgument($"{OutputSeek}-an -sn -dn -map_metadata -1 -map_chapters -1 -vf \"fps={Rate}:round=near:eof_action=pass,{Filter}\" -frames:v {FrameBudget} -f image2"));

        try
        {
            string Binary = await Task.Run(() => Payload.Get("ffmpeg.exe"), CancellationToken);

            _ = await RunAsync(Binary, "-hide_banner -nostdin -v error -filter_threads 2 -progress pipe:1 -nostats " + Command.Arguments, Line =>
            {
                if (Line.StartsWith("out_time_us=", StringComparison.Ordinal) && long.TryParse(Line.AsSpan(12), out long Microseconds))
                {
                    Progress?.Report(new WorkProgress(Math.Clamp(Microseconds / (Seconds * 10000d), 0, 100), "Preview"));
                }
            }, CancellationToken, WorkerLimit);

            string[] Frames = [.. Directory.GetFiles(DirectoryPath, "frame-*.png").Order(StringComparer.Ordinal)];

            if (Frames.Length is 0)
            {
                throw new InvalidDataException("No frames were found at the selected start time.");
            }

            return Frames;
        }
        catch
        {
            Directory.Delete(DirectoryPath, true);

            throw;
        }
    }

    /// <summary>Reads the first video stream without reflection-based JSON deserialization.</summary>
    internal static async Task<MediaInfo> ProbeAsync(string Path, CancellationToken CancellationToken)
    {
        string Binary = await Task.Run(() => Payload.Get("ffprobe.exe"), CancellationToken);

        string Text = await RunAsync(Binary, "-v error -select_streams v:0 -show_entries stream=width,height,duration,avg_frame_rate:stream_side_data=rotation:format=duration -of json " + Quote(Path), null, CancellationToken);

        using JsonDocument Document = JsonDocument.Parse(Text);

        JsonElement Streams = Document.RootElement.GetProperty("streams");

        if (Streams.GetArrayLength() is 0)
        {
            throw new InvalidDataException("No video or animated image stream was found.");
        }

        JsonElement Stream = Streams[0];

        int Width = Stream.GetProperty("width").GetInt32();

        int Height = Stream.GetProperty("height").GetInt32();

        double Duration = Number(Stream, "duration");

        if (Duration <= 0 && Document.RootElement.TryGetProperty("format", out JsonElement Format))
        {
            Duration = Number(Format, "duration");
        }

        string Rate = Stream.TryGetProperty("avg_frame_rate", out JsonElement FrameRate) ? FrameRate.GetString() ?? "0/1" : "0/1";

        int Separator = Rate.AsSpan().IndexOf('/');

        double Fps = Separator > 0 && double.TryParse(Rate.AsSpan(0, Separator), CultureInfo.InvariantCulture, out double Numerator)
            && double.TryParse(Rate.AsSpan(Separator + 1), CultureInfo.InvariantCulture, out double Denominator) && Denominator > 0 ? Numerator / Denominator : 0;

        if (Stream.TryGetProperty("side_data_list", out JsonElement SideData))
        {
            foreach (JsonElement Item in SideData.EnumerateArray())
            {
                if (Item.TryGetProperty("rotation", out JsonElement Rotation) && Math.Abs(Rotation.GetInt32()) % 180 is 90)
                {
                    (Width, Height) = (Height, Width);
                }
            }
        }

        if (Width <= 0 || Height <= 0)
        {
            throw new InvalidDataException("This media has no usable dimensions.");
        }

        if (IsWebPInput(Path))
        {
            MediaInfo? Animation = await Task.Run(() => ReadWebPTiming(Path, CancellationToken), CancellationToken);

            if (Animation is not null)
            {
                return Animation;
            }
        }

        return new MediaInfo(Width, Height, Duration, Fps);
    }

    /// <summary>Encodes to a new owned file and verifies the selected animation container before exposing it.</summary>
    internal async Task<ConversionResult> ConvertAsync(string Input, ConversionSettings Settings, IProgress<WorkProgress> Progress, CancellationToken CancellationToken)
    {
        _ = Directory.CreateDirectory(Workspace);

        string Output = Path.Combine(Workspace, Guid.NewGuid().ToString("N") + WebPFormat.Extension);

        try
        {
            Progress.Report(new WorkProgress(0, "Encoding WebP"));

            await EncodeAsync(Input, Output, Settings, Progress, CancellationToken);

            ConversionResult Result = Inspect(Output, Settings, CancellationToken);

            Progress.Report(new WorkProgress(100, "Encoded"));

            return Result;
        }
        catch
        {
            File.Delete(Output);

            throw;
        }
    }

    /// <summary>Builds the WebP encoder command; fitting preserves transparency and source aspect ratio.</summary>
    private static async Task EncodeAsync(string Input, string Output, ConversionSettings Settings, IProgress<WorkProgress>? Progress, CancellationToken CancellationToken)
    {
        if (Settings.Width is < 16 or > 4096 || Settings.Height is < 16 or > 4096 || Settings.FrameRate is < 1 or > 60
            || Settings.Duration is < 0.1 or > 60 || Settings.Start < 0 || !double.IsFinite(Settings.Start)
            || !double.IsFinite(Settings.Duration) || Settings.Compression is < 0 or > 6 || Settings.Loops is < 0 or > ushort.MaxValue
            || Settings.Quality is < 0 or > 100 || !double.IsFinite(Settings.PanX) || !double.IsFinite(Settings.PanY)
            || Settings.PanX is < 0 or > 1 || Settings.PanY is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Settings), "Use 16–4096 px, 1–60 fps, a 0.1–60 second clip, compression 0–6 and loops 0–65,535.");
        }

        string Binary = await Task.Run(() => Payload.Get("ffmpeg.exe"), CancellationToken);

        string Filter = FramingFilter(Settings, Settings.Width, Settings.Height);

        await EncodeParallelWebPAsync(Binary, Input, Output, Filter, Settings, Progress, CancellationToken);
    }

    /// <summary>Shares exact normalized crop/padding expressions between fitted previews and both encoder paths.</summary>
    private static string FramingFilter(ConversionSettings Settings, int Width, int Height)
    {
        string Size = $"{Width}:{Height}";

        string X = Settings.PanX.ToString("R", CultureInfo.InvariantCulture);

        string Y = Settings.PanY.ToString("R", CultureInfo.InvariantCulture);

        return Settings.Fit is "Fill"
            ? $"format=rgba,scale={Size}:force_original_aspect_ratio=increase:flags=lanczos,crop={Size}:(iw-ow)*{X}:(ih-oh)*{Y}"
            : $"format=rgba,scale={Size}:force_original_aspect_ratio=decrease:flags=lanczos,pad={Size}:(ow-iw)*{X}:(oh-ih)*{Y}:color=0x00000000";
    }

    /// <summary>Checks the complete animation container before exposing an exported file.</summary>
    internal static ConversionResult Inspect(string Path, ConversionSettings Settings, CancellationToken CancellationToken = default)
    {
        return InspectWebP(Path, Settings, CancellationToken);
    }

    /// <summary>Drains both pipes concurrently and kills the process tree when cancelled.</summary>
    internal static async Task<string> RunAsync(string Binary, string Arguments, Action<string>? OnLine, CancellationToken CancellationToken,
        MediaWorkerLimit? SharedLimit = null)
    {
        using Process Process = new()
        {
            StartInfo = new ProcessStartInfo(Binary, Arguments)
            {
                UseShellExecute = false,

                CreateNoWindow = true,

                RedirectStandardOutput = true,

                RedirectStandardError = true
            }
        };

        _ = Process.Start();

        using MediaWorkerLimit? OwnedLimit = SharedLimit is null ? new() : null;

        MediaWorkerLimit Limit = SharedLimit ?? OwnedLimit!;

        try
        {
            Limit.Attach(Process);
        }
        catch
        {
            StopProcess(Process);

            await Process.WaitForExitAsync(CancellationToken.None);

            throw;
        }

        try
        {
            Process.PriorityClass = ProcessPriorityClass.BelowNormal;
        }
        catch (InvalidOperationException)
        {
            // Very short probes may exit before their priority can be changed.
        }

        using CancellationTokenRegistration Registration = CancellationToken.Register(() =>
        {
            StopProcess(Process);
        });

        Task<string> Errors = ReadBoundedAsync(Process.StandardError, 8192);

        System.Text.StringBuilder Output = new();

        try
        {
            await ReadOutputAsync(Process.StandardOutput, Output, OnLine, CancellationToken);

            await Process.WaitForExitAsync(CancellationToken);

            string Error = await Errors;

            CancellationToken.ThrowIfCancellationRequested();

            if (Process.ExitCode is not 0)
            {
                if (Limit.PeakBytes >= MediaWorkerLimit.MaximumBytes * 9 / 10)
                {
                    throw new InvalidDataException("The media worker reached its memory safety limit. Reduce the output dimensions or compression and try again.");
                }

                throw new InvalidDataException(string.IsNullOrWhiteSpace(Error) ? "The media process failed." : Error.Trim()[..Math.Min(Error.Trim().Length, 1000)]);
            }

            return Output.ToString();
        }
        finally
        {
            StopProcess(Process);

            await Process.WaitForExitAsync(CancellationToken.None);

            _ = await Errors;
        }
    }

    /// <summary>Enforces line and retained-total ceilings before materializing worker text; stderr drains independently.</summary>
    private static async Task ReadOutputAsync(StreamReader Reader, System.Text.StringBuilder Output, Action<string>? OnLine, CancellationToken Token)
    {
        char[] Buffer = ArrayPool<char>.Shared.Rent(1024);
        System.Text.StringBuilder Line = new();

        try
        {
            int Count;

            while ((Count = await Reader.ReadAsync(Buffer.AsMemory(0, 1024), Token)) > 0)
            {
                for (int Index = 0; Index < Count; ++Index)
                {
                    char Value = Buffer[Index];

                    if (Value is '\n')
                    {
                        Emit();
                    }
                    else if (Value is not '\r')
                    {
                        if (Line.Length >= 16384 || (OnLine is null && Output.Length + Line.Length >= 1048576))
                        {
                            throw new InvalidDataException("The media process returned too much metadata.");
                        }

                        _ = Line.Append(Value);
                    }
                }
            }

            if (Line.Length > 0)
            {
                Emit();
            }

            void Emit()
            {
                if (OnLine is null)
                {
                    if (Output.Length + Line.Length + 1 > 1048576)
                    {
                        throw new InvalidDataException("The media process returned too much metadata.");
                    }

                    _ = Output.Append(Line).Append('\n');
                }
                else
                {
                    OnLine(Line.ToString());
                }

                _ = Line.Clear();
            }
        }
        finally
        {
            ArrayPool<char>.Shared.Return(Buffer, clearArray: true);
        }
    }

    /// <summary>Terminates owned workers without turning a normal exit race into an encoding failure.</summary>
    internal static void StopProcess(Process Process)
    {
        try
        {
            if (!Process.HasExited)
            {
                Process.Kill(true);
            }
        }
        catch (InvalidOperationException)
        {
            // Completion may race either cancellation or final pipe cleanup.
        }
    }

    /// <summary>Drains diagnostics while retaining only a bounded message and returning the read buffer.</summary>
    internal static async Task<string> ReadBoundedAsync(StreamReader Reader, int Limit)
    {
        char[] Buffer = ArrayPool<char>.Shared.Rent(1024);

        System.Text.StringBuilder Text = new(Limit);

        try
        {
            int Count;

            while ((Count = await Reader.ReadAsync(Buffer.AsMemory(0, 1024))) > 0)
            {
                _ = Text.Append(Buffer, 0, Math.Min(Count, Limit - Text.Length));
            }

            return Text.ToString();
        }
        finally
        {
            ArrayPool<char>.Shared.Return(Buffer, clearArray: true);
        }
    }

    private static double Number(JsonElement Element, string Name)
    {
        return Element.TryGetProperty(Name, out JsonElement Value) && double.TryParse(Value.ToString(), CultureInfo.InvariantCulture, out double Result) && double.IsFinite(Result) ? Result : 0;
    }

    internal static string Quote(string Value) => "\"" + Value + "\"";

    public void Dispose()
    {
        try
        {
            Directory.Delete(Workspace, true);
        }
        catch (IOException)
        {
            // A terminated decoder may briefly retain its file handle.
        }
        catch (UnauthorizedAccessException)
        {
            // A system scanner may retain a handle; the directory is under the OS temp root.
        }
    }
}
