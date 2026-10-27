using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace VRCWebP;

/// <summary>Reuses one tooltip surface and serializes complete exit and entry transitions.</summary>
internal static class TooltipFlow
{
    private const int DwellMilliseconds = 1000;

    private static readonly TextBlock Text = new()
    {
        FontFamily = new FontFamily("avares://VRCWebP/Assets/Fonts#Cascadia Code"),

        FontSize = 12,

        LineHeight = 18,

        Foreground = Brush.Parse("#F2F2F2"),

        TextWrapping = TextWrapping.Wrap,

        MaxLines = 0,

        TextTrimming = TextTrimming.None,

        MaxWidth = 420
    };

    private static readonly Border Surface = new()
    {
        Background = Brush.Parse("#2D2D2D"),

        BorderBrush = Brush.Parse("#484848"),

        BorderThickness = new Thickness(1),

        CornerRadius = new CornerRadius(8),

        Padding = new Thickness(16, 13),

        IsHitTestVisible = false,

        Child = Text,

        Opacity = 0,

        Transitions =
        [
            new DoubleTransition
            {
                Property = Visual.OpacityProperty,

                Duration = UiMotion.Duration
            }
        ]
    };

    private static readonly Popup Popup = new()
    {
        Child = Surface,

        IsHitTestVisible = false,

        ShouldUseOverlayLayer = true,

        TakesFocusFromNativeControl = false,

        WindowManagerAddShadowHint = false,

        Placement = PlacementMode.Bottom,

        PlacementConstraintAdjustment = PopupPositionerConstraintAdjustment.SlideX | PopupPositionerConstraintAdjustment.SlideY
    };

    private static Control? Desired;

    private static Control? Shown;

    private static long Deadline;

    private static long Revision;

    private static long ShownRevision;

    private static Task? Pump;


    /// <summary>Registers class handlers once, including popups and controls created after startup.</summary>
    internal static void Initialize()
    {
        ToolTip.TipProperty.Changed.AddClassHandler<Control>((Owner, _) =>
        {
            if (ReferenceEquals(Owner, Shown))
            {
                if (ToolTip.GetTip(Owner) is string Hint && Owner.IsEffectivelyVisible && TopLevel.GetTopLevel(Owner) is not null)
                {
                    UiMotion.SetText(Text, Hint);

                    Position(Owner);
                }
                else
                {
                    MoveTo(null);
                }
            }
        });

        InputElement.PointerMovedEvent.AddClassHandler<TopLevel>((Root, Event) =>
        {

            Control? Owner = null;

            // Routed input omits disabled controls. Rendered hit testing still finds their explanatory hints.
            Visual? Hit = Root.GetVisualAt(Event.GetPosition(Root), static Visual => Visual.IsVisible
                && (Visual is not InputElement Input || Input.IsHitTestVisible));

            for (Visual? Source = Hit ?? Event.Source as Visual; Source is not null; Source = Source.GetVisualParent())
            {
                if (Source is Control Candidate && ToolTip.GetTip(Candidate) is string
                    && (Candidate.IsEffectivelyEnabled || ToolTip.GetShowOnDisabled(Candidate)))
                {
                    Owner = Candidate;

                    break;
                }
            }

            MoveTo(Owner);
        }, RoutingStrategies.Tunnel, true);

        InputElement.PointerExitedEvent.AddClassHandler<Control>((Sender, Event) =>
        {

            if ((ReferenceEquals(Sender, Desired) && !Sender.IsPointerOver)
                || (Sender is TopLevel && ReferenceEquals(Event.Source, Sender)))
            {
                MoveTo(null);
            }
        }, RoutingStrategies.Direct, true);

        InputElement.PointerPressedEvent.AddClassHandler<TopLevel>((_, _) => MoveTo(null), RoutingStrategies.Tunnel, true);

        InputElement.KeyDownEvent.AddClassHandler<TopLevel>((_, _) => MoveTo(null), RoutingStrategies.Tunnel, true);
    }

    /// <summary>Retains only the most recent destination while a previous animation completes.</summary>
    internal static void MoveTo(Control? Owner)
    {
        if (ReferenceEquals(Desired, Owner))
        {
            return;
        }

        Desired = Owner;

        ++Revision;

        Deadline = Environment.TickCount64 + DwellMilliseconds;

        if (Pump is null || Pump.IsCompleted)
        {
            Pump = UpdateAsync();
        }
    }

    /// <summary>Completes one exit before retargeting and fading in the latest requested hint.</summary>
    private static async Task UpdateAsync()
    {
        try
        {
            while (!ReferenceEquals(Shown, Desired) || (Shown is not null && ShownRevision != Revision))
            {
                if (Shown is not null)
                {
                    Surface.Opacity = 0;

                    await Task.Delay(UiMotion.Duration);

                    Popup.IsOpen = false;

                    Popup.PlacementTarget = null;

                    ((ISetLogicalParent)Popup).SetParent(null);

                    Shown = null;
                }

                Control? Candidate = Desired;

                if (Candidate is null)
                {
                    continue;
                }

                long Visit = Revision;

                await Task.Delay((int)Math.Clamp(Deadline - Environment.TickCount64, 0, DwellMilliseconds));

                if (Visit != Revision || !ReferenceEquals(Candidate, Desired))
                {
                    continue;
                }

                if (!Candidate.IsEffectivelyVisible || TopLevel.GetTopLevel(Candidate) is null)
                {
                    Desired = null;

                    continue;
                }

                Text.Text = ToolTip.GetTip(Candidate) as string;

                ((ISetLogicalParent)Popup).SetParent(Candidate);

                Position(Candidate);

                Popup.IsOpen = true;

                Shown = Candidate;

                ShownRevision = Visit;

                await Task.Delay(20);

                if (Visit != Revision)
                {
                    continue;
                }

                Surface.Opacity = 1;
            }
        }
        catch (InvalidOperationException)
        {
            // A window or popup can detach while a tooltip is waiting to appear.
            Popup.IsOpen = false;

            Popup.PlacementTarget = null;

            ((ISetLogicalParent)Popup).SetParent(null);

            Shown = null;

            Desired = null;
        }
    }

    /// <summary>Wraps the complete hint independently of its owner's line limit and places it inside the client area.</summary>
    private static void Position(Control Owner)
    {
        TopLevel Host = TopLevel.GetTopLevel(Owner)!;

        Point Origin = Owner.TranslatePoint(default, Host) ?? default;

        double AvailableWidth = Math.Max(40, Host.Bounds.Width - 32);

        Text.MaxWidth = Math.Min(Owner is TextBlock { MaxLines: 1 } ? 720 : 420, AvailableWidth - 34);

        Surface.Measure(new Size(AvailableWidth, double.PositiveInfinity));

        Size Size = Surface.DesiredSize;

        bool Above = Origin.Y + Owner.Bounds.Height + Size.Height + 16 > Host.Bounds.Height;

        Popup.PlacementTarget = Owner;

        Popup.Placement = Above ? PlacementMode.Top : PlacementMode.Bottom;

        Popup.VerticalOffset = Above ? -8 : 8;
    }
}
