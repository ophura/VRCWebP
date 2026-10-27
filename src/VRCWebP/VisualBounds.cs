using Avalonia;
using Avalonia.Controls;

namespace VRCWebP;

/// <summary>Translates measured controls to a common window coordinate system.</summary>
internal static class VisualBounds
{
    internal static Rect BoundsIn(this Control Control, Window Window)
    {
        return new Rect(Control.Bounds.Size).TransformToAABB(Control.TransformToVisual(Window) ?? Matrix.Identity);
    }
}
