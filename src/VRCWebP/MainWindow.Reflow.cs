using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.Styling;

namespace VRCWebP;

internal sealed partial class MainWindow
{
    private readonly Image ReflowImage = new()
    {
        IsHitTestVisible = false,

        HorizontalAlignment = HorizontalAlignment.Left,

        VerticalAlignment = VerticalAlignment.Top,

        Stretch = Stretch.Fill
    };

    private readonly Animation ReflowFadeIn = CreateLayoutFade(0, 1);

    private readonly Animation ReflowFadeOut = CreateLayoutFade(1, 0);

    private CancellationTokenSource? ReflowCancellation;

    private static Animation CreateLayoutFade(double Start, double End)
    {
        return new Animation
        {
            Duration = UiMotion.Duration,

            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),

                    Setters = { new Setter(Visual.OpacityProperty, Start) }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),

                    Setters = { new Setter(Visual.OpacityProperty, End) }
                }
            }
        };
    }

    internal void AttachLayoutTransition(Grid Frame)
    {
        Frame.Children.Add(ReflowImage);
    }

    /// <summary>Crossfades one disposable snapshot across a structural layout change.</summary>
    private void BeginLayoutTransition()
    {
        ReflowCancellation?.Cancel();

        ReflowImage.Source = null;

        BodyLayout.Opacity = 1;

        if (!IsVisible || Lifetime.IsCancellationRequested || ReflowImage.Parent is null || BodyLayout.Bounds.Width <= 0 || BodyLayout.Bounds.Height <= 0)
        {
            return;
        }

        double CaptureScale = Math.Min(RenderScaling, 2048d / Math.Max(BodyLayout.Bounds.Width, BodyLayout.Bounds.Height));

        RenderTargetBitmap Snapshot = new(new PixelSize(Math.Max(1, (int)Math.Ceiling(BodyLayout.Bounds.Width * CaptureScale)),
            Math.Max(1, (int)Math.Ceiling(BodyLayout.Bounds.Height * CaptureScale))), new Vector(96 * CaptureScale, 96 * CaptureScale));

        try
        {
            Snapshot.Render(BodyLayout);
        }
        catch
        {
            Snapshot.Dispose();

            throw;
        }

        Rect PreviousBounds = BodyLayout.BoundsIn(this);

        ReflowImage.Width = PreviousBounds.Width;

        ReflowImage.Height = PreviousBounds.Height;

        ReflowImage.Margin = new Thickness(PreviousBounds.X, PreviousBounds.Y, 0, 0);

        ReflowImage.Source = Snapshot;

        ReflowImage.Opacity = 1;

        BodyLayout.Opacity = 0;

        CancellationTokenSource Pending = CancellationTokenSource.CreateLinkedTokenSource(Lifetime.Token);

        ReflowCancellation = Pending;

        _ = CompleteLayoutTransitionAsync(Snapshot, Pending);
    }

    /// <summary>Releases the snapshot on completion, cancellation or shutdown.</summary>
    private async Task CompleteLayoutTransitionAsync(RenderTargetBitmap Snapshot, CancellationTokenSource Pending)
    {
        try
        {
            await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Render);

            Pending.Token.ThrowIfCancellationRequested();

            BodyLayout.Opacity = 1;

            ReflowImage.Opacity = 0;

            await Task.WhenAll(ReflowFadeIn.RunAsync(BodyLayout, Pending.Token),
                ReflowFadeOut.RunAsync(ReflowImage, Pending.Token));
        }
        catch (OperationCanceledException)
        {
            // A newer layout or window shutdown supersedes this snapshot.
        }
        finally
        {
            if (ReferenceEquals(ReflowCancellation, Pending))
            {
                ReflowImage.Source = null;

                BodyLayout.Opacity = 1;

                ReflowCancellation = null;
            }

            Snapshot.Dispose();

            Pending.Dispose();
        }
    }
}
