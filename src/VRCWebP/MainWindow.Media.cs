using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Buffers;
using System.Globalization;

namespace VRCWebP;

internal sealed partial class MainWindow
{
    // Dispatcher-owned counters reject stale results after an await; background workers
    // receive immutable settings and own decoded bitmaps until the dispatcher accepts them.
    // Operation/PreviewOperation cancel producers first; CleanupWork joins deferred file cleanup.
    // DecodedSeconds describes sampled source time, independent from export duration/quality.
    private long RecipeVersion;

    private long ProgressRevision;

    private Task CleanupWork = Task.CompletedTask;

    private bool UpdatingPosition;

    private bool UserCancelledPreview;

    private double DecodedSeconds;

    private long PlaybackStarted;

    private double PlaybackOffset;

    private ConversionSettings? DecodedSettings;

    private string? DecodedPath;

    private readonly Slider PreviewPosition = new()
    {
        Minimum = 0,

        Maximum = 4,

        Height = 40,

        MinHeight = 40
    };

    /// <summary>Updates the visible frame without validating a partially edited clip or preparing a file.</summary>
    private void ConfigureFraming()
    {
        ConversionSettings Canvas = new((int)(CanvasWidth.Value ?? 512), (int)(CanvasHeight.Value ?? 512), 12, 0, 1,
            Fit.SelectedItem as string ?? "Contain", 0, 0, PanX: PreviewImage.PanX, PanY: PreviewImage.PanY);

        PreviewImage.Configure(Canvas, InputInfo);
    }

    /// <summary>Copies a stable source off the dispatcher, reporting bytes actually read.</summary>
    private Task ImportAsync(string Path)
    {
        return RunWorkAsync(async Token =>
        {
            ++RecipeVersion;

            IProgress<WorkProgress> Loading = Reporter(Token: Token);

            string Owned = await Task.Run(() => CopySourceAsync(Path, Loading, Token), Token);

            try
            {
                ++ProgressRevision;

                UiMotion.SetText(Status, "Reading metadata");

                Progress.IsIndeterminate = true;

                MediaInfo Info = await Task.Run(() => MediaService.ProbeAsync(Owned, Token), Token);

                Token.ThrowIfCancellationRequested();

                if (InputPath is not null)
                {
                    QueueDelete(InputPath);
                }

                InputPath = Owned;

                InputInfo = Info;

                Owned = "";

                Updating = true;

                try
                {
                    (int Width, int Height) Size = CanvasPreset.SourceSize(Info.Width, Info.Height);

                    CanvasWidth.Value = Size.Width;

                    CanvasHeight.Value = Size.Height;

                    Start.Value = 0;

                    Start.Maximum = Info.Duration > 0 ? (decimal)Math.Max(0, Info.Duration - 0.1) : 86400;

                    Duration.Maximum = Info.Duration > 0 ? (decimal)Math.Max(0.1, Math.Min(60, Info.Duration)) : 60;

                    Duration.Value = Duration.Maximum;

                    PreviewLength.Value = Math.Min(4, Duration.Maximum);

                    FrameRate.Value = Info.FrameRate > 0 ? Math.Clamp(Math.Round(Info.FrameRate), 1, 60) : 24;

                    CompressionMode.SelectedIndex = 0;

                    Compression.Value = 0;

                    Loops.Value = 0;

                    PreviewImage.ResetFraming();

                    Fit.SelectedIndex = 0;

                    Preset.SelectedIndex = 0;
                }
                finally
                {
                    Updating = false;
                }

                await StopEncodedPreviewAsync();

                ResetOutput();

                UiMotion.SetText(Filename, System.IO.Path.GetFileName(Path));

                UiMotion.SetVisible(SourceHeader, true);

                UiMotion.SetText(SourceDetails, $"{Info.Width} × {Info.Height} · {Info.Duration:0.00} s · {Info.FrameRate:0.##} fps");

                UiMotion.SetVisible(Empty, false);

                ClearFrames();

                Progress.IsIndeterminate = false;

                await LoadPreviewAsync(Token);

                UiMotion.SetText(Status, "Ready");

                ShowOutputStatistics();

            }
            finally
            {
                Progress.IsIndeterminate = false;

                if (Owned.Length > 0)
                {
                    QueueDelete(Owned);
                }
            }
        });
    }

    private async Task<string> CopySourceAsync(string Path, IProgress<WorkProgress> Reporter, CancellationToken Token)
    {
        string Folder = System.IO.Path.Combine(Media.Workspace, Guid.NewGuid().ToString("N"));

        _ = Directory.CreateDirectory(Folder);

        string Destination = System.IO.Path.Combine(Folder, System.IO.Path.GetFileName(Path));

        try
        {
            await CopyFileAsync(Path, Destination, Reporter, Token);

            return Destination;
        }
        catch
        {
            File.Delete(Destination);

            Directory.Delete(Folder);

            throw;
        }
    }

    /// <summary>Uses overlapped I/O and one pooled buffer; progress counts completed writes.</summary>
    private static async Task CopyFileAsync(string Path, string Destination, IProgress<WorkProgress> Reporter, CancellationToken Token)
    {
        byte[] Buffer = ArrayPool<byte>.Shared.Rent(1048576);

        try
        {
            await using FileStream Source = new(Path, FileMode.Open, FileAccess.Read, FileShare.Read, 65536, FileOptions.Asynchronous | FileOptions.SequentialScan);

            await using FileStream Output = new(Destination, FileMode.CreateNew, FileAccess.Write, FileShare.None, 65536, FileOptions.Asynchronous | FileOptions.SequentialScan);

            long Length = Source.Length;

            long Copied = 0;

            int Previous = -1;

            int Count;

            while ((Count = await Source.ReadAsync(Buffer.AsMemory(0, 1048576), Token)) > 0)
            {
                await Output.WriteAsync(Buffer.AsMemory(0, Count), Token);

                Copied += Count;

                int Percent = Length > 0 ? (int)Math.Min(99, Copied * 100d / Length) : 99;

                if (Percent != Previous)
                {
                    Reporter.Report(new WorkProgress(Percent, "Loading media"));

                    Previous = Percent;
                }
            }

            await Output.FlushAsync(Token);

            Token.ThrowIfCancellationRequested();

            Reporter.Report(new WorkProgress(100, "Loading media"));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(Buffer, clearArray: true);
        }
    }

    /// <summary>Decodes on a worker and transfers ownership only for the current recipe.</summary>
    private async Task LoadPreviewAsync(CancellationToken Token)
    {
        if (InputPath is null)
        {
            return;
        }

        if (HasCurrentOutput())
        {
            await LoadEncodedPreviewAsync(Token);

            UpdateButtons();

            return;
        }

        EncodedPlayback?.SetPlaying(false);

        Playback.Stop();

        PreviewDetails.Text = "Preview: 0%";

        UiMotion.SetText(Status, "Preview: 0%");

        Progress.IsIndeterminate = false;

        ConversionSettings Settings = ReadSettings();

        long Version = RecipeVersion;

        double Seconds = Math.Min((double)(PreviewLength.Value ?? 4), Settings.Duration);

        IProgress<WorkProgress> Updates = Reporter("Preview", 0, 0.8, Token);

        string Input = InputPath;

        List<Bitmap> Decoded = await Task.Run(async () =>
        {
            string[] Paths = await Media.PreviewFramesAsync(Input, Settings, Token, Seconds, Updates, RawFrame: true);

            List<Bitmap> Images = new(Paths.Length);

            try
            {
                for (int Index = 0; Index < Paths.Length; ++Index)
                {
                    Token.ThrowIfCancellationRequested();

                    Images.Add(new Bitmap(Paths[Index]));

                    Updates.Report(new WorkProgress(100 + ((Index + 1) * 25d / Paths.Length), "Preview"));
                }

                Token.ThrowIfCancellationRequested();

                return Images;
            }
            catch
            {
                foreach (Bitmap Image in Images)
                {
                    Image.Dispose();
                }

                throw;
            }
            finally
            {
                Directory.Delete(System.IO.Path.GetDirectoryName(Paths[0])!, true);
            }
        }, Token);

        if (Token.IsCancellationRequested || Version != RecipeVersion)
        {
            await Task.Run(() => Decoded.ForEach(Image => Image.Dispose()), CancellationToken.None);

            Token.ThrowIfCancellationRequested();

            return;
        }

        PreviewImage.EncodedSource = false;

        PreviewImage.Source = Decoded[0];

        await StopEncodedPreviewAsync();

        ClearFrames();

        Frames.AddRange(Decoded);

        DecodedSeconds = Seconds;

        DecodedSettings = Settings;

        DecodedPath = Input;

        ToolTip.SetTip(PreviewImage, "Inside the light frame is the exported picture; dimmed pixels are excluded.\nDrag to position the image. Double-click to center it.\nCheckerboard areas remain transparent.");

        PreviewPosition.Maximum = Seconds;

        PreviewImage.Configure(Settings, InputInfo);

        PreviewImage.Source = Frames[0];

        SetPlaybackPosition(0);

        StartPreviewPlayback();

        UiMotion.SetContent(Play, PauseIcon);

        SetPlaybackPosition();

        UpdateButtons();
    }

    private Task RefreshPreviewAsync()
    {
        PreviewTask = RefreshPreviewCoreAsync();

        return PreviewTask;
    }

    /// <summary>Debounces only preview work; file preparation belongs to the explicit Create action.</summary>
    private async Task RefreshPreviewCoreAsync()
    {
        if (InputPath is null || Busy || ClosingRequested)
        {
            return;
        }

        CancellationTokenSource? Previous = PreviewOperation;

        using CancellationTokenSource Pending = CancellationTokenSource.CreateLinkedTokenSource(Lifetime.Token);

        PreviewOperation = Pending;

        if (Previous is not null)
        {
            await Previous.CancelAsync();
        }

        UpdateButtons();

        try
        {
            await WorkGate.WaitAsync(Pending.Token);

            try
            {
                ConversionSettings Settings = ReadSettings();

                double Seconds = Math.Min((double)(PreviewLength.Value ?? 4), Settings.Duration);

                string PreviewPath = HasCurrentOutput() ? Result!.Path : InputPath;

                if (DecodedSettings is not ConversionSettings PreviousSettings || Frames.Count is 0 || DecodedPath != PreviewPath
                    || PreviousSettings.Start != Settings.Start || DecodedSeconds != Seconds)
                {
                    await LoadPreviewAsync(Pending.Token);
                }

                PreviewImage.Configure(Settings, InputInfo);

                ShowOutputStatistics();

                UiMotion.SetText(Status, HasCurrentOutput() ? "WebP ready" : "Preview ready");
            }
            finally
            {
                _ = WorkGate.Release();
            }
        }
        catch (OperationCanceledException)
        {
            if (ReferenceEquals(PreviewOperation, Pending) && !Busy)
            {
                if (UserCancelledPreview)
                {
                    UiMotion.SetText(Status, "Cancelled");
                }
                else
                {
                    UiMotion.SetText(Status, "Updating preview");
                }
            }
        }
        catch (Exception Error)
        {
            if (ReferenceEquals(PreviewOperation, Pending))
            {
                UiMotion.SetText(OutputDetails, "Output unavailable");

                ShowError(Error.Message);
            }
        }
        finally
        {
            await Pending.CancelAsync();

            if (ReferenceEquals(PreviewOperation, Pending))
            {
                PreviewOperation = null;

                UpdateButtons();
            }
        }
    }

    /// <summary>Encodes only on explicit request, then opens the upload destination.</summary>
    private async Task ConvertAsync(CancellationToken Token)
    {
        if (InputPath is null)
        {
            return;
        }

        ConversionSettings Settings = ReadSettings();

        UiMotion.SetText(Status, "Encoding WebP: 0%");

        Progress.IsIndeterminate = false;

        Progress.Value = 0;

        if (Result?.Settings != Settings)
        {
            string Input = InputPath;

            IProgress<WorkProgress> Encoding = Reporter(Token: Token);

            ConversionResult Created = await Task.Run(() => Media.ConvertAsync(Input, Settings, Encoding, Token), Token);

            ConversionResult? Previous = Result;

            Result = Created;

            if (Previous is not null)
            {
                await StopEncodedPreviewAsync();

                QueueDelete(Previous.Path);
            }
        }

        await LoadPreviewAsync(Token);

        ShowOutputStatistics();

        InspectorTabs.SelectedIndex = 1;

        UiMotion.SetText(Status, WebPFormat.Name + " ready");
    }

    /// <summary>Distinguishes decoded preview statistics from a file committed with Create WebP.</summary>
    private void ShowOutputStatistics()
    {
        if (Result is not null && HasCurrentOutput())
        {
            UiMotion.SetText(OutputDetails, string.Create(CultureInfo.InvariantCulture, $"WebP · {Result.Bytes / 1048576d:0.##} MiB · {Result.Settings.Duration:0.00} s\n{Result.Width} × {Result.Height} · {Result.Frames} frames"));

            ToolTip.SetTip(OutputDetails, "Verified " + WebPFormat.Name + " dimensions, frame count and exact file size");
        }
        else if (InputPath is not null && Frames.Count > 0)
        {
            UiMotion.SetText(OutputDetails, string.Create(CultureInfo.InvariantCulture, $"Preview · {DecodedSeconds:0.00} s · {Frames.Count / DecodedSeconds:0.##} fps\n{CanvasWidth.Value:0} × {CanvasHeight.Value:0} · {Frames.Count} frames"));

            ToolTip.SetTip(OutputDetails, "Decoded preview only. Create prepares the selected output format and displays its actual size; compression applies to that file.");
        }

        UpdateUploadAccess();
    }

    /// <summary>Ignores queued reports once an operation is completed, cancelled or superseded.</summary>
    private Progress<WorkProgress> Reporter(string? Stage = null, double Offset = 0, double Scale = 1, CancellationToken Token = default)
    {
        long Version = RecipeVersion;

        long Revision = ++ProgressRevision;

        return new Progress<WorkProgress>(Value =>
        {
            if (ClosingRequested || Token.IsCancellationRequested || Version != RecipeVersion || Revision != ProgressRevision)
            {
                return;
            }

            double Percent = Math.Clamp(Offset + (Value.Percent * Scale), 0, 100);

            string Name = Stage ?? Value.Stage;

            Progress.IsIndeterminate = false;

            Progress.Value = Percent;

            ToolTip.SetTip(Progress, $"{Name}: {Percent:0}%");

            UiMotion.SetText(Status, $"{Name}: {Percent:0}%");

            if (Name is "Preview")
            {
                PreviewDetails.Text = $"Preview: {Percent:0}%";
            }
        });
    }

    /// <summary>Uses elapsed time rather than accumulating timer delays, so busy frames do not slow the animation.</summary>
    private void StartPreviewPlayback()
    {
        if (EncodedPlayback is not null)
        {
            EncodedPlayback.SetPlaying(true);

            Playback.Start();

            return;
        }

        if (Frames.Count is 0 || DecodedSeconds <= 0)
        {
            return;
        }

        PlaybackOffset = PreviewPosition.Value % DecodedSeconds;

        PlaybackStarted = System.Diagnostics.Stopwatch.GetTimestamp();

        Playback.Interval = TimeSpan.FromSeconds(1d / 30);

        Playback.Start();
    }

    private void SetPlaybackPosition(double? Requested = null)
    {
        UpdatingPosition = true;

        PreviewPosition.Value = Requested ?? (Frames.Count > 1 ? FrameIndex * DecodedSeconds / Frames.Count : 0);

        Span<char> Text = stackalloc char[64];

        if (Text.TryWrite(CultureInfo.InvariantCulture, $"{PreviewPosition.Value:F2} / {DecodedSeconds:F2} s", out int Written)
            && !Text[..Written].SequenceEqual(PreviewDetails.Text.AsSpan()))
        {
            PreviewDetails.Text = new string(Text[..Written]);
        }

        UpdatingPosition = false;
    }

    private void SeekPreview()
    {
        if (!UpdatingPosition && EncodedPlayback is not null)
        {
            EncodedPlayback.SetPlaying(false);

            Playback.Stop();

            UiMotion.SetContent(Play, PlayIcon);

            double Requested = Math.Clamp(PreviewPosition.Value, 0, DecodedSeconds);

            EncodedSeekTask = SeekEncodedPreviewAsync(EncodedPlayback, Requested);

            return;
        }

        if (!UpdatingPosition && Frames.Count > 0 && DecodedSeconds > 0)
        {
            Playback.Stop();

            UiMotion.SetContent(Play, PlayIcon);

            double Requested = Math.Clamp(PreviewPosition.Value, 0, DecodedSeconds);

            FrameIndex = Math.Clamp((int)(Requested / DecodedSeconds * Frames.Count), 0, Frames.Count - 1);

            PreviewImage.Source = Frames[FrameIndex];

            SetPlaybackPosition(Requested);
        }
    }

    /// <summary>Serializes workspace cleanup on workers without stalling pointer handling.</summary>
    private void QueueDelete(string Path)
    {
        CleanupWork = CleanupWork.ContinueWith(_ =>
        {
            try
            {
                File.Delete(Path);
            }
            catch (IOException)
            {
                // Workspace disposal retries files temporarily retained by a scanner.
            }
        }, TaskScheduler.Default);
    }
}
