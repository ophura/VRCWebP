using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace VRCWebP;

/// <summary>Animates the optional destination controls and their occupied height with the shared motion policy.</summary>
internal sealed class MotionExpander : Expander
{
    private Border? Body;

    protected override Type StyleKeyOverride => typeof(Expander);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs Event)
    {
        base.OnApplyTemplate(Event);

        Body = Event.NameScope.Find<Border>("ExpanderContent");

        if (Body is not null)
        {
            Body.IsVisible = IsExpanded;

            Body.MaxHeight = IsExpanded ? double.PositiveInfinity : 0;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs Change)
    {
        base.OnPropertyChanged(Change);

        if (Change.Property != IsExpandedProperty || Body is null)
        {
            return;
        }

        if (IsExpanded)
        {
            Body.Child?.Measure(new Size(Math.Max(1, Bounds.Width - 24), double.PositiveInfinity));

            double Height = Body.Child?.DesiredSize.Height ?? 0;

            UiMotion.SetVisible(Body, true);

            Body.MaxHeight = Height + Body.Padding.Top + Body.Padding.Bottom;
        }
        else
        {
            if (double.IsPositiveInfinity(Body.MaxHeight))
            {
                Body.MaxHeight = Body.Bounds.Height;
            }

            Body.MaxHeight = 0;

            UiMotion.SetVisible(Body, false);
        }
    }
}
