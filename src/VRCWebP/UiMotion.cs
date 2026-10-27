using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System.Runtime.CompilerServices;

namespace VRCWebP;

/// <summary>Shares one motion duration and replaces pending visibility animations without retaining closed controls.</summary>
internal static class UiMotion
{
    internal static readonly TimeSpan Duration = TimeSpan.FromMilliseconds(200);

    /// <summary>Applies the shared timing to transitions declared locally inside framework templates as well.</summary>
    internal static void Initialize()
    {
        _ = Control.LoadedEvent.AddClassHandler<Control>((Control, _) =>
        {
            TextOptions.SetTextRenderingMode(Control, TextRenderingMode.Antialias);

            if (Control.Transitions is { } Transitions)
            {
                foreach (TransitionBase Transition in Transitions.OfType<TransitionBase>())
                {
                    Transition.Duration = Duration;
                }
            }
        });
    }

    // All state belongs to the UI dispatcher. Weak keys do not root closed controls.
    // A content state owns at most one ghost/snapshot; replacement composites the previous
    // surface, cancels its fade and disposes it only after the new surface has captured it.
    private static readonly ConditionalWeakTable<Control, VisibilityState> Visibility = [];

    private static readonly ConditionalWeakTable<Control, ContentState> Content = [];

    /// <summary>Crossfades a textual announcement without changing its logical value or accessibility timing.</summary>
    internal static void SetText(TextBlock Control, string? Text)
    {
        if (Control.Text != Text)
        {
            Change(Control, () => Control.Text = Text);
        }
    }

    /// <summary>Crossfades an icon or caption change, preserving a single bounded previous surface.</summary>
    internal static void SetContent(ContentControl Control, object Content)
    {
        if (!ReferenceEquals(Control.Content, Content))
        {
            Change(Control, () => Control.Content = Content);
        }
    }

    /// <summary>Composites an interrupted transition before replacing its content, keeping rapid updates continuous.</summary>
    internal static void Change(Control Control, Action Mutation)
    {
        OverlayLayer? Layer = OverlayLayer.GetOverlayLayer(Control);

        if (!Control.IsEffectivelyVisible || Layer is null || Control.Bounds.Width <= 0 || Control.Bounds.Height <= 0
            || Control.TranslatePoint(default, Layer) is not Point Origin)
        {
            Mutation();

            return;
        }

        ContentState State = Content.GetValue(Control, static Control => new ContentState(Control));

        TextOptions.SetTextRenderingMode(Control, TextRenderingMode.Antialias);

        double Scale = Math.Min(TopLevel.GetTopLevel(Control)?.RenderScaling ?? 1, 1024d / Math.Max(Control.Bounds.Width, Control.Bounds.Height));

        PixelSize Size = new(Math.Max(1, (int)Math.Ceiling(Control.Bounds.Width * Scale)), Math.Max(1, (int)Math.Ceiling(Control.Bounds.Height * Scale)));

        RenderTargetBitmap Snapshot = new(Size, new Vector(96 * Scale, 96 * Scale));

        try
        {
            Snapshot.Render(Control);

            if (State.Ghost?.Source is RenderTargetBitmap Previous)
            {
                using RenderTargetBitmap Current = Snapshot;

                Snapshot = new RenderTargetBitmap(Size, new Vector(96 * Scale, 96 * Scale));

                using DrawingContext Drawing = Snapshot.CreateDrawingContext();

                Rect Area = new(Control.Bounds.Size);

                Drawing.DrawImage(Current, Area);

                using (Drawing.PushOpacity(State.Ghost.Opacity))
                {
                    Drawing.DrawImage(Previous, Area);
                }
            }

            State.Pending?.Cancel();

            Mutation();

            Image Ghost = new()
            {
                Source = Snapshot,

                Width = Control.Bounds.Width,

                Height = Control.Bounds.Height,

                Margin = new Thickness(Origin.X, Origin.Y, 0, 0),

                HorizontalAlignment = HorizontalAlignment.Left,

                VerticalAlignment = VerticalAlignment.Top,

                IsHitTestVisible = false,

                Stretch = Stretch.Fill
            };

            Layer.Children.Add(Ghost);

            CancellationTokenSource Pending = new();

            State.Ghost = Ghost;

            State.Pending = Pending;

            _ = CompleteContentAsync(Control, Layer, State, Ghost, Snapshot, Pending);
        }
        catch
        {
            Snapshot.Dispose();

            throw;
        }
    }

    /// <summary>Releases the previous content surface after completion, replacement or detachment.</summary>
    private static async Task CompleteContentAsync(Control Control, OverlayLayer Layer, ContentState State, Image Ghost, RenderTargetBitmap Snapshot, CancellationTokenSource Pending)
    {
        try
        {
            await Task.WhenAll(FadeAsync(Control, 0, 1, Token: Pending.Token), FadeAsync(Ghost, 1, 0, Token: Pending.Token));
        }
        catch (OperationCanceledException)
        {
            // A later change already captured the displayed composite before replacing it.
        }
        finally
        {
            _ = Layer.Children.Remove(Ghost);

            Ghost.Source = null;

            Snapshot.Dispose();

            if (ReferenceEquals(State.Pending, Pending))
            {
                State.Pending = null;

                State.Ghost = null;
            }

            Pending.Dispose();
        }
    }

    /// <summary>Fades a control before collapsing it, while disabling hidden actions immediately.</summary>
    internal static void SetVisible(Control Control, bool Visible)
    {
        VisibilityState State = Visibility.GetValue(Control, static Control => new VisibilityState(Control));

        if (State.Target == Visible)
        {
            return;
        }

        State.Target = Visible;

        double From = Control.IsVisible ? Control.Opacity : 0;

        State.Pending?.Cancel();

        if (TopLevel.GetTopLevel(Control) is null)
        {
            Control.IsVisible = Visible;

            return;
        }

        Control.IsHitTestVisible = Visible && State.HitTesting;

        if (Visible)
        {
            Control.Opacity = From;

            Control.IsVisible = true;
        }

        CancellationTokenSource Pending = new();

        State.Pending = Pending;

        _ = CompleteVisibilityAsync(Control, State, From, Visible, Pending);
    }

    /// <summary>Animates the currently rendered opacity without stacking transitions on inherited brushes.</summary>
    internal static Task FadeAsync(Control Control, double From, double To, TimeSpan? Interval = null, CancellationToken Token = default)
    {
        // Explicit keyframes own opacity. A styled transition on the same property
        // otherwise starts another animation when the final base value changes.
        if (Control.Transitions is Transitions Existing && Existing.OfType<TransitionBase>().Any(Transition => Transition.Property == Visual.OpacityProperty))
        {
            Transitions Remaining = [];

            foreach (ITransition Transition in Existing)
            {
                if (Transition is not TransitionBase Base || Base.Property != Visual.OpacityProperty)
                {
                    Remaining.Add(Transition);
                }
            }

            Control.Transitions = Remaining;
        }

        Control.Opacity = To;

        Animation Fade = new()
        {
            Duration = Interval ?? Duration,

            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),

                    Setters = { new Setter(Visual.OpacityProperty, From) }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),

                    Setters = { new Setter(Visual.OpacityProperty, To) }
                }
            }
        };

        return Fade.RunAsync(Control, Token);
    }

    /// <summary>Collapses only the latest requested state after its exit has rendered.</summary>
    private static async Task CompleteVisibilityAsync(Control Control, VisibilityState State, double From, bool Visible, CancellationTokenSource Pending)
    {
        try
        {
            await FadeAsync(Control, From, Visible ? 1 : 0, Token: Pending.Token);

            if (ReferenceEquals(State.Pending, Pending))
            {
                Control.IsVisible = Visible;
            }
        }
        catch (OperationCanceledException)
        {
            // A later state or detachment supersedes this visual transition.
        }
        finally
        {
            if (ReferenceEquals(State.Pending, Pending))
            {
                State.Pending = null;
            }

            Pending.Dispose();
        }
    }

    /// <summary>Keeps cancellation and original hit testing with the weakly owned control.</summary>
    private sealed class VisibilityState
    {
        internal bool? Target;

        internal CancellationTokenSource? Pending;

        internal readonly bool HitTesting;

        internal VisibilityState(Control Control)
        {
            HitTesting = Control.IsHitTestVisible;

            Control.DetachedFromVisualTree += (_, _) =>
            {
                Pending?.Cancel();

                Target = null;
            };
        }
    }

    /// <summary>Owns at most one active content ghost for a live control.</summary>
    private sealed class ContentState
    {
        internal Image? Ghost;

        internal CancellationTokenSource? Pending;

        internal ContentState(Control Control)
        {
            Control.DetachedFromVisualTree += (_, _) => Pending?.Cancel();
        }
    }
}
