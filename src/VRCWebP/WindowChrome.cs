using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using System.Runtime.InteropServices;

namespace VRCWebP;

/// <summary>Applies Mica, dark DWM chrome and custom caption buttons across the whole window.</summary>
internal static partial class WindowChrome
{

    internal static void Configure(Window Window, double Width, double Height)
    {
        Window.Deactivated += (_, _) =>
        {
            {
                TooltipFlow.MoveTo(null);

                Window.FocusManager?.Focus(null);
            }
        };

        Window.Closed += (_, _) => TooltipFlow.MoveTo(null);

        Window.Width = Width;

        Window.Height = Height;

        Window.WindowDecorations = WindowDecorations.Full;

        Window.ExtendClientAreaToDecorationsHint = true;

        Window.ExtendClientAreaTitleBarHeightHint = 0;

        Window.TransparencyLevelHint = [WindowTransparencyLevel.Mica, WindowTransparencyLevel.AcrylicBlur];

        Window.Background = Brush.Parse("#E0151515");

        Window.TransparencyBackgroundFallback = Brush.Parse("#181818");

        Window.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;

        Window.UseLayoutRounding = true;

        TextOptions.SetTextRenderingMode(Window, TextRenderingMode.Antialias);

        using (Stream Icon = typeof(WindowChrome).Assembly.GetManifestResourceStream("Payload.application.ico") ?? throw new FileNotFoundException("Missing application icon."))
        {
            Window.Icon = new WindowIcon(Icon);
        }

        Window.AddHandler(InputElement.PointerPressedEvent, (_, Event) =>
        {
            if (Window.FocusManager?.GetFocusedElement() is TextBox Editor
                && (Event.Source is not Visual Source || !Source.GetSelfAndVisualAncestors().Contains(Editor)))
            {
                Window.FocusManager.Focus(null);
            }

            if (!Window.CanResize || Window.WindowState is not WindowState.Normal || !Event.GetCurrentPoint(Window).Properties.IsLeftButtonPressed)
            {
                return;
            }

            if (ResizeEdge(Window, Event.GetPosition(Window)) is WindowEdge Edge)
            {
                Window.BeginResizeDrag(Edge, Event);

                Event.Handled = true;
            }
        }, Avalonia.Interactivity.RoutingStrategies.Tunnel);

        Window.PropertyChanged += (_, Event) =>
        {
            if (Event.Property == Window.WindowStateProperty)
            {
                Window.Cursor = null;
            }
        };

        Window.AddHandler(InputElement.GotFocusEvent, (_, Event) =>
        {
            if (Event.Source is TextBox Editor && Event.NavigationMethod is NavigationMethod.Tab)
            {
                Editor.ClearSelection();
            }
        }, Avalonia.Interactivity.RoutingStrategies.Bubble, true);

        Window.Opened += (_, _) =>
        {
            nint HitTest(nint Handle, uint Message, nint WParam, nint LParam, ref bool Handled)
            {

                if (Message is 0x0084)
                {
                    PixelPoint Screen = new(unchecked((short)((long)LParam & 0xFFFF)), unchecked((short)(((long)LParam >> 16) & 0xFFFF)));

                    Point Position = Window.PointToClient(Screen);

                    if (ResizeEdge(Window, Position) is WindowEdge Edge)
                    {
                        Handled = true;

                        return NativeResizeHit(Edge);
                    }

                    if (new Rect(Window.Bounds.Size).Contains(Position))
                    {
                        Handled = true;


                        return 1;
                    }
                }

                return 0;
            }

            Win32Properties.AddWndProcHookCallback(Window, HitTest);

            Window.Closed += (_, _) => Win32Properties.RemoveWndProcHookCallback(Window, HitTest);

            _ = new MonitorLayout(Window);

            nint Handle = Window.TryGetPlatformHandle()?.Handle ?? 0;

            int Enabled = 1;

            _ = DwmSetWindowAttribute(Handle, 20, ref Enabled, sizeof(int));

            int Backdrop = 2;

            _ = DwmSetWindowAttribute(Handle, 38, ref Backdrop, sizeof(int));

            int BorderColor = 0x003B3B3B;

            _ = DwmSetWindowAttribute(Handle, 34, ref BorderColor, sizeof(int));

            int Round = 2;

            _ = DwmSetWindowAttribute(Handle, 33, ref Round, sizeof(int));
        };
    }

    /// <summary>Creates an unbranded drag region with caption controls and optional contextual actions.</summary>
    internal static Control Header(Window Window, Control? Extra = null, Control? Actions = null)
    {
        Grid Header = new()
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto,Auto"),

            Height = 64,

            Background = Brushes.Transparent,

            Margin = new Thickness(20, 0, 8, 0)
        };

        Border DragSurface = new()
        {
            Background = Brushes.Transparent
        };

        WindowDecorationProperties.SetElementRole(DragSurface, WindowDecorationsElementRole.User);

        Header.Children.Add(DragSurface);

        Grid.SetColumn(DragSurface, 1);

        if (Actions is not null)
        {
            Actions.VerticalAlignment = VerticalAlignment.Center;

            Header.Children.Add(Actions);
        }

        if (Extra is not null)
        {
            Grid.SetColumn(Extra, 2);

            Extra.VerticalAlignment = VerticalAlignment.Center;

            Extra.Margin = new Thickness(0, 0, 18, 0);

            Header.Children.Add(Extra);
        }

        StackPanel Caption = new()
        {
            Orientation = Orientation.Horizontal,

            VerticalAlignment = VerticalAlignment.Center,

            Spacing = 2
        };

        if (Window.CanResize)
        {
            Button Minimize = Icons.Button("minimize", "Minimize");

            Minimize.Classes.Add("chrome");

            WindowDecorationProperties.SetElementRole(Minimize, WindowDecorationsElementRole.User);

            Minimize.Click += (_, _) => Window.WindowState = WindowState.Minimized;

            Caption.Children.Add(Minimize);

            Button Maximize = Icons.Button("maximize", "Maximize / restore");

            Control MaximizeIcon = (Control)Maximize.Content!;

            Control RestoreIcon = Icons.Make("restore");

            Maximize.Classes.Add("chrome");

            WindowDecorationProperties.SetElementRole(Maximize, WindowDecorationsElementRole.User);

            Maximize.Click += (_, _) => Window.WindowState = Window.WindowState is WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

            Window.PropertyChanged += (_, Event) =>
            {
                if (Event.Property == Window.WindowStateProperty)
                {
                    bool Restoring = Window.WindowState is WindowState.Maximized;

                    UiMotion.SetContent(Maximize, Restoring ? RestoreIcon : MaximizeIcon);

                    ToolTip.SetTip(Maximize, Restoring ? "Restore" : "Maximize");

                    AutomationProperties.SetName(Maximize, Restoring ? "Restore" : "Maximize");
                }
            };

            Caption.Children.Add(Maximize);
        }

        Button Close = Icons.Button("close", "Close");

        Close.Classes.Add("chrome");

        Close.Classes.Add("close");

        WindowDecorationProperties.SetElementRole(Close, WindowDecorationsElementRole.User);

        Close.Click += (_, _) => Window.Close();

        Caption.Children.Add(Close);

        Grid.SetColumn(Caption, 3);

        Header.Children.Add(Caption);

        Window.Resources[typeof(WindowChrome)] = new HeaderLayout(Header, [.. Header.GetVisualDescendants().OfType<Button>()]);

        Header.PointerPressed += (_, Event) =>
        {

            Point Position = Event.GetPosition(Window);

            if (!Event.GetCurrentPoint(Window).Properties.IsLeftButtonPressed || ResizeEdge(Window, Position) is not null || !IsHeaderDragPoint(Window, Position))
            {
                return;
            }

            if (Event.ClickCount is 2 && Window.CanResize)
            {
                Window.WindowState = Window.WindowState is WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
            else
            {
                Window.BeginMoveDrag(Event);
            }

            Event.Handled = true;
        };

        return Header;
    }

    /// <summary>Uses a shared hit area for resize dragging and its directional cursor.</summary>
    internal static WindowEdge? ResizeEdge(Window Window, Point Position)
    {
        if (!Window.CanResize || Window.WindowState is not WindowState.Normal || !new Rect(Window.Bounds.Size).Contains(Position))
        {
            return null;
        }

        bool Left = Position.X < 6;

        bool Right = Position.X >= Window.Bounds.Width - 6;

        bool Top = Position.Y < 10;

        bool Bottom = Position.Y >= Window.Bounds.Height - 6;

        if (Top || Bottom)
        {
            return Top
                ? Left ? WindowEdge.NorthWest : Right ? WindowEdge.NorthEast : WindowEdge.North
                : Left ? WindowEdge.SouthWest : Right ? WindowEdge.SouthEast : WindowEdge.South;
        }

        return Left ? WindowEdge.West : Right ? WindowEdge.East : null;
    }

    private static int NativeResizeHit(WindowEdge Edge)
    {
        return Edge switch
        {
            WindowEdge.West => 10,
            WindowEdge.East => 11,
            WindowEdge.North => 12,
            WindowEdge.NorthWest => 13,
            WindowEdge.NorthEast => 14,
            WindowEdge.South => 15,
            WindowEdge.SouthWest => 16,
            WindowEdge.SouthEast => 17,
            _ => 1
        };
    }


    /// <summary>Identifies drag space without assigning nonclient mouse input to interactive header controls.</summary>
    internal static bool IsHeaderDragPoint(Window Window, Point Position)
    {
        if (!Window.Resources.TryGetValue(typeof(WindowChrome), out object? Value) || Value is not HeaderLayout Layout
            || !Layout.Header.BoundsIn(Window).Contains(Position))
        {
            return false;
        }

        foreach (Button Button in Layout.Buttons)
        {
            if (Button.IsEffectivelyVisible && Button.BoundsIn(Window).Contains(Position))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>Caches interactive controls once while their measured bounds continue to follow DPI and layout changes.</summary>
    private sealed record HeaderLayout(Grid Header, Button[] Buttons);


    [LibraryImport("dwmapi.dll")]
    private static partial int DwmSetWindowAttribute(nint Window, int Attribute, ref int Value, int Size);
}
