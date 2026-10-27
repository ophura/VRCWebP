using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace VRCWebP;

/// <summary>Fades dropdown surfaces in place and retains their last pixels during the exit transition.</summary>
internal sealed class MotionChoice : ComboBox
{
    private Popup? Popup;

    private OverlayLayer? Layer;

    private Point Origin;

    private CancellationTokenSource? Entry;

    private CancellationTokenSource? Exit;

    protected override Type StyleKeyOverride => typeof(ComboBox);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs Event)
    {
        if (Popup is not null)
        {
            Popup.Opened -= OnOpened;

            Popup.Closed -= OnClosed;

            if (Popup.Child is Control Previous)
            {
                Previous.LayoutUpdated -= OnPopupLayout;
            }
        }

        Entry?.Cancel();

        Exit?.Cancel();

        base.OnApplyTemplate(Event);

        Popup = Event.NameScope.Find<Popup>("PART_Popup");

        if (Popup is not null)
        {
            Popup.ShouldUseOverlayLayer = true;

            Popup.Opened += OnOpened;

            Popup.Closed += OnClosed;
        }
    }

    private void OnOpened(object? Sender, EventArgs Event)
    {
        if (Popup?.Child is not Control Surface)
        {
            return;
        }

        Entry?.Cancel();

        Layer = OverlayLayer.GetOverlayLayer(this);

        Surface.LayoutUpdated += OnPopupLayout;

        CancellationTokenSource Pending = new();

        Entry = Pending;

        _ = EnterAsync(Surface, Pending);
    }

    private async Task EnterAsync(Control Surface, CancellationTokenSource Pending)
    {
        try
        {
            Surface.Opacity = 0;

            await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Render);

            Pending.Token.ThrowIfCancellationRequested();

            await UiMotion.FadeAsync(Surface, 0, 1, Token: Pending.Token);
        }
        catch (OperationCanceledException)
        {
            // Closing or retemplating supersedes the entry animation.
        }
        finally
        {
            if (ReferenceEquals(Entry, Pending))
            {
                Entry = null;
            }

            Pending.Dispose();
        }
    }

    private void OnPopupLayout(object? Sender, EventArgs Event)
    {
        if (Layer is not null && Popup?.Child is Control Surface && Surface.TranslatePoint(default, Layer) is Point Position)
        {
            Origin = Position;
        }
    }

    private void OnClosed(object? Sender, EventArgs Event)
    {
        if (Popup?.Child is not Control Surface)
        {
            return;
        }

        Surface.LayoutUpdated -= OnPopupLayout;

        double Opacity = Surface.Opacity;

        Entry?.Cancel();

        Exit?.Cancel();

        if (Layer is not { } Owner || !IsEffectivelyVisible || Surface.Bounds.Width <= 0 || Surface.Bounds.Height <= 0)
        {
            return;
        }

        double Scale = Math.Min(TopLevel.GetTopLevel(this)?.RenderScaling ?? 1, 1024d / Math.Max(Surface.Bounds.Width, Surface.Bounds.Height));

        RenderTargetBitmap Snapshot = new(new PixelSize(Math.Max(1, (int)Math.Ceiling(Surface.Bounds.Width * Scale)),
            Math.Max(1, (int)Math.Ceiling(Surface.Bounds.Height * Scale))), new Vector(96 * Scale, 96 * Scale));

        try
        {
            Snapshot.Render(Surface);
        }
        catch
        {
            Snapshot.Dispose();

            throw;
        }

        Image Ghost = new()
        {
            Source = Snapshot,

            Width = Surface.Bounds.Width,

            Height = Surface.Bounds.Height,

            HorizontalAlignment = HorizontalAlignment.Left,

            VerticalAlignment = VerticalAlignment.Top,

            Margin = new Thickness(Origin.X, Origin.Y, 0, 0),

            IsHitTestVisible = false,

            Stretch = Stretch.Fill
        };

        Owner.Children.Add(Ghost);

        CancellationTokenSource Pending = new();

        Exit = Pending;

        _ = LeaveAsync(Owner, Ghost, Snapshot, Opacity, Pending);
    }

    private async Task LeaveAsync(OverlayLayer Owner, Image Ghost, RenderTargetBitmap Snapshot, double Opacity, CancellationTokenSource Pending)
    {
        try
        {
            await UiMotion.FadeAsync(Ghost, Opacity, 0, Token: Pending.Token);
        }
        catch (OperationCanceledException)
        {
            // A later dismissal supersedes the previous bounded snapshot.
        }
        finally
        {
            _ = Owner.Children.Remove(Ghost);

            Ghost.Source = null;

            Snapshot.Dispose();

            if (ReferenceEquals(Exit, Pending))
            {
                Exit = null;
            }

            Pending.Dispose();
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs Event)
    {
        Entry?.Cancel();

        Exit?.Cancel();

        base.OnDetachedFromVisualTree(Event);
    }
}
