using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace VRCWebP;

internal sealed partial class MainWindow
{
    private EncodedPreviewSession? EncodedPlayback;

    private CancellationTokenSource? EncodedSeekOperation;

    private Task? EncodedSeekTask;

    private Bitmap? EditingStill;

    /// <summary>Keeps one small source frame for immediate framing edits, instead of retaining the whole editing animation.</summary>
    private void RetainEditingStill()
    {
        if (Frames.Count > 0)
        {
            Bitmap Selected = Frames[Math.Clamp(FrameIndex, 0, Frames.Count - 1)];

            Frames.Remove(Selected);

            ReleaseEditingStill();

            EditingStill = Selected;
        }
    }

    private void ReleaseEditingStill()
    {
        Bitmap? Retired = EditingStill;

        EditingStill = null;

        if (Retired is not null)
        {
            CleanupWork = CleanupWork.ContinueWith(_ => Retired.Dispose(), CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
        }
    }

    /// <summary>Provides an immediate source image while the asynchronous editing animation is refreshed.</summary>
    private void ActivateEditingStill()
    {
        EncodedPlayback?.SetPlaying(false);

        if (PreviewImage.EncodedSource && EditingStill is not null)
        {
            Playback.Stop();

            PreviewImage.EncodedSource = false;

            PreviewImage.Source = EditingStill;

            PreviewImage.InvalidateVisual();

            UiMotion.SetContent(Play, PlayIcon);
        }
    }

    /// <summary>Displays the committed file through a bounded full-resolution decoder, independently of the editing preview length.</summary>
    private async Task LoadEncodedPreviewAsync(CancellationToken Token)
    {
        ConversionResult Created = Result ?? throw new InvalidOperationException("Create a WebP first.");

        if (EncodedPlayback?.Path == Created.Path)
        {
            if (!PreviewImage.EncodedSource && EncodedPlayback.CurrentFrame is WriteableBitmap Existing)
            {
                PreviewImage.EncodedSource = true;

                PreviewImage.Configure(Created.Settings, InputInfo);

                PreviewImage.Source = Existing;

                PreviewImage.InvalidateVisual();

                StartPreviewPlayback();

                UiMotion.SetContent(Play, PauseIcon);
            }

            return;
        }

        await StopEncodedPreviewAsync();

        EncodedPreviewSession? Session = null;

        bool First = true;

        Session = new EncodedPreviewSession(Created, (Frame, Position) =>
        {
            if (!ReferenceEquals(EncodedPlayback, Session) || ClosingRequested)
            {
                return;
            }

            if (First)
            {
                RetainEditingStill();

                ClearFrames(KeepStill: true);

                DecodedSettings = Created.Settings;

                DecodedPath = Created.Path;

                DecodedSeconds = Session!.Duration;

                PreviewPosition.Maximum = DecodedSeconds;

                PreviewImage.Configure(Created.Settings, InputInfo);

                First = false;

                Playback.Interval = TimeSpan.FromSeconds(1d / 30);

                Playback.Start();
            }

            PreviewImage.EncodedSource = true;

            PreviewImage.Source = Frame;

            PreviewImage.InvalidateVisual();

            SetPlaybackPosition(Position);
        }, Error =>
        {
            if (ReferenceEquals(EncodedPlayback, Session) && !ClosingRequested)
            {
                Playback.Stop();

                UiMotion.SetContent(Play, PlayIcon);

                ShowError("Encoded preview failed: " + Error.Message);
            }
        }, () =>
        {
            if (ReferenceEquals(EncodedPlayback, Session) && !ClosingRequested)
            {
                Playback.Stop();

                SetPlaybackPosition(Session!.Duration);

                UiMotion.SetContent(Play, PlayIcon);
            }
        }, Lifetime.Token);

        EncodedPlayback = Session;

        try
        {
            UiMotion.SetText(Status, "Opening encoded WebP");

            Progress.IsIndeterminate = true;

            await Session.StartAsync(Token);

            ToolTip.SetTip(PreviewImage, "The encoded WebP at its original resolution and playback rate.\nDrag to edit framing; create a replacement to apply your changes.");

            UiMotion.SetContent(Play, PauseIcon);
        }
        catch
        {
            await StopEncodedPreviewAsync();

            throw;
        }
        finally
        {
            Progress.IsIndeterminate = false;
        }
    }

    /// <summary>Detaches its image before joining the decoder and disposing the native buffers.</summary>
    private async Task StopEncodedPreviewAsync()
    {
        if (EncodedSeekOperation is not null)
        {
            await EncodedSeekOperation.CancelAsync();
        }

        EncodedPreviewSession? Previous = EncodedPlayback;

        EncodedPlayback = null;

        if (PreviewImage.EncodedSource)
        {
            PreviewImage.Source = null;

            PreviewImage.EncodedSource = false;
        }

        if (Previous is not null)
        {
            await Previous.DisposeAsync();
        }
    }

    /// <summary>Replaces seeks asynchronously; cancellation preserves the last displayed frame and usable file.</summary>
    private async Task SeekEncodedPreviewAsync(EncodedPreviewSession Session, double Position)
    {
        if (EncodedSeekOperation is not null)
        {
            await EncodedSeekOperation.CancelAsync();
        }

        using CancellationTokenSource Pending = CancellationTokenSource.CreateLinkedTokenSource(Lifetime.Token);

        EncodedSeekOperation = Pending;

        Progress.IsIndeterminate = true;

        UiMotion.SetText(Status, "Seeking encoded WebP");

        UpdateButtons();

        try
        {
            await Session.SeekAsync(Position, Pending.Token);

            if (ReferenceEquals(EncodedPlayback, Session))
            {
                SetPlaybackPosition(Position);

                UiMotion.SetText(Status, "WebP ready");
            }
        }
        catch (OperationCanceledException)
        {
            if (ReferenceEquals(EncodedSeekOperation, Pending) && ReferenceEquals(EncodedPlayback, Session))
            {
                Session.CancelSeek();
            }
        }
        catch (Exception Error)
        {
            Session.CancelSeek();

            if (!ClosingRequested)
            {
                ShowError(Error.Message);
            }
        }
        finally
        {
            if (ReferenceEquals(EncodedSeekOperation, Pending))
            {
                EncodedSeekOperation = null;

                Progress.IsIndeterminate = false;

                UpdateButtons();
            }
        }
    }
}
