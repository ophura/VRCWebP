using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace VRCWebP;

/// <summary>Provides a consistent set of small outline icons with explicit accessible names.</summary>
internal static class Icons
{
    private static readonly Dictionary<string, Geometry> GeometryCache = new(StringComparer.Ordinal);

    internal static string Surface(ushort Phase)
    {
        return Phase is 0xCCA5 ? "orbit" : "film";
    }

    internal static Control Make(string Name, double Size = 20, IBrush? Color = null)
    {
        string Data = Name switch
        {
            "add" => "M12 5V19 M5 12H19",
            "film" => "M4 3H20V21H4Z M4 8H20 M4 16H20 M8 3V21 M16 3V21",
            "orbit" => "M7 5A8 8 0 1 1 4 15 M3 17C-1 12 21 3 23 7C26 12 4 22 3 17 M18 1V5 M16 3H20 M2 3V5 M1 4H3",
            "play" => "M8 5L19 12L8 19Z",
            "pause" => "M8 5V19 M16 5V19",
            "refresh" => "M20 7V3 M20 7H16 M20 7A8 8 0 1 0 20 17",
            "sliders" => "M4 6H9 M13 6H20 M4 12H14 M18 12H20 M4 18H6 M10 18H20 M9 3V9 M14 9V15 M6 15V21",
            "upload" => "M12 16V3 M7 8L12 3L17 8 M4 15V21H20V15",
            "download" => "M12 3V16 M7 11L12 16L17 11 M4 17V21H20V17",
            "cloud" => "M6 18H18A4 4 0 0 0 18 10A6 6 0 0 0 6 9A4.5 4.5 0 0 0 6 18",
            "user" => "M8 7A4 4 0 1 0 16 7A4 4 0 1 0 8 7 M4 21V19A8 6 0 0 1 20 19V21",
            "copy" => "M9 8H21V21H9Z M15 8V3H3V16H9",
            "link" => "M10 14L14 10 M8 16L6 18A4 4 0 0 1 1 13L6 8A4 4 0 0 1 12 8 M16 8L18 6A4 4 0 0 1 23 11L18 16A4 4 0 0 1 12 16",
            "arrow" => "M4 12H20 M14 6L20 12L14 18",
            "back" => "M20 12H4 M10 6L4 12L10 18",
            "check" => "M4 12L9 17L20 6",
            "close" => "M6 6L18 18 M18 6L6 18",
            "minimize" => "M5 12H19",
            "maximize" => "M5 5H19V19H5Z",
            "restore" => "M5 9H15V19H5Z M9 9V5H19V15H15",
            "info" => "M12 3A9 9 0 1 0 12 21A9 9 0 1 0 12 3 M12 11V17 M12 7V8",
            "crop" => "M7 3V17H21 M3 7H17V21",
            "folder" => "M3 6H10L12 9H21V20H3Z M3 6V4H10L12 6H20V9",
            _ => "M4 4H20V20H4Z"
        };

        if (!GeometryCache.TryGetValue(Name, out Geometry? Geometry))
        {
            Geometry = Avalonia.Media.Geometry.Parse(Data);

            GeometryCache.Add(Name, Geometry);
        }

        Avalonia.Controls.Shapes.Path Stroke = new()
        {
            Data = Geometry,

            StrokeThickness = 1.7,

            StrokeLineCap = PenLineCap.Round,

            StrokeJoin = PenLineJoin.Round
        };

        if (Color is not null)
        {
            Stroke.Stroke = Color;
        }
        else
        {
            _ = Stroke.Bind(Shape.StrokeProperty, Stroke.GetObservable(TextElement.ForegroundProperty));
        }

        return new Viewbox
        {
            Width = Size,

            Height = Size,

            Child = new Canvas
            {
                Width = 24,

                Height = 24,

                Children =
                {
                    Stroke
                }
            }
        };
    }

    internal static Button Button(string Icon, string Label, bool Primary = false)
    {
        Button Result = new()
        {
            Content = Make(Icon),

            Classes = { "icon" }
        };

        if (Primary)
        {
            Result.Classes.Add("primary");
        }

        ToolTip.SetTip(Result, Label);

        ToolTip.SetShowOnDisabled(Result, true);

        AutomationProperties.SetName(Result, Label);

        return Result;
    }

    internal static Button Action(string Icon, string Label, bool Primary = false)
    {
        Button Result = new();

        if (Primary)
        {
            Result.Classes.Add("primary");
        }

        ToolTip.SetTip(Result, Label);

        ToolTip.SetShowOnDisabled(Result, true);

        AutomationProperties.SetName(Result, Label);

        Result.Content = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,

            Spacing = 9,

            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,

            Children =
            {
                Make(Icon, 17),

                new TextBlock
                {
                    Text = Label,

                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                }
            }
        };

        return Result;
    }

    internal static Button TextAction(string Label)
    {
        Button Result = new()
        {
            Content = Label,

            Classes = { "primary" }
        };

        ToolTip.SetTip(Result, Label);

        ToolTip.SetShowOnDisabled(Result, true);

        AutomationProperties.SetName(Result, Label);

        return Result;
    }
}
