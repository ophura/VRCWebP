using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;

namespace VRCWebP;

/// <summary>Fits content to client bounds while leaving shell snapping and dragging under Windows' control.</summary>
internal sealed class MonitorLayout : IDisposable
{
    private readonly Window Window;

    private readonly Size Minimum;

    private readonly LayoutTransformControl Host;

    private readonly ScaleTransform Transform = new(1, 1);

    private readonly Action ApplyPending;


    private bool Queued;

    private bool Disposed;


    internal MonitorLayout(Window Window)
    {
        this.Window = Window;

        Minimum = Window.CanResize ? new Size(Window.MinWidth, Window.MinHeight) : new Size(Window.Width, Window.Height);


        ApplyPending = Apply;

        Control? Body = Window.Content as Control;

        Window.Content = null;

        Host = new LayoutTransformControl
        {
            Child = Body,

            LayoutTransform = Transform
        };

        Grid Frame = new()
        {
            ClipToBounds = true,

            Children = { Host }
        };

        if (Window is MainWindow Main)
        {
            Main.AttachLayoutTransition(Frame);
        }

        Window.Content = Frame;

        Window.Resources[typeof(MonitorLayout)] = this;

        UpdateTrackBounds();

        Window.SizeChanged += OnLayoutChanged;

        Window.ScalingChanged += OnScalingChanged;

        Window.Screens.Changed += OnDisplaysChanged;

        Window.Closed += OnClosed;

        FitInitialBounds();

        Schedule();
    }

    /// <summary>Caps only the initial size; subsequent move/resize/snap bounds belong to the shell.</summary>
    private void FitInitialBounds()
    {
        Screen? Screen = Window.Screens.ScreenFromWindow(Window) ?? Window.Screens.Primary;

        if (Screen is null)
        {
            return;
        }

        Size Area = new(Screen.WorkingArea.Width / Screen.Scaling, Screen.WorkingArea.Height / Screen.Scaling);


        Size Available = AvailableSize(Area);

        if (Window.Width > Available.Width)
        {
            Window.Width = Available.Width;
        }

        if (Window.Height > Available.Height)
        {
            Window.Height = Available.Height;
        }
    }

    private void OnLayoutChanged(object? Sender, EventArgs Event)
    {
        Schedule();
    }

    private void OnScalingChanged(object? Sender, EventArgs Event)
    {
        UpdateTrackBounds();

        Schedule();
    }

    /// <summary>Keeps manual resizing usable while allowing the current monitor's quarter-screen snap zones.</summary>
    private void UpdateTrackBounds()
    {
        Screen? Screen = Window.Screens.ScreenFromWindow(Window) ?? Window.Screens.Primary;

        if (!Window.CanResize || Screen is null)
        {
            Window.MinWidth = 0;

            Window.MinHeight = 0;

            return;
        }

        Size Area = new(Screen.WorkingArea.Width / Screen.Scaling, Screen.WorkingArea.Height / Screen.Scaling);


        Window.MinWidth = Math.Min(900, Area.Width / 2);

        Window.MinHeight = Math.Min(580, Area.Height / 2);
    }

    private void OnDisplaysChanged(object? Sender, EventArgs Event)
    {
        UpdateTrackBounds();

        Schedule();

        if (Window.WindowState is WindowState.Normal && !Window.Screens.All.Any(Screen => Screen.WorkingArea.Contains(Window.Position)))
        {
            if (Window.Screens.Primary is Screen Primary)
            {
                Window.Position = new PixelPoint(Primary.WorkingArea.X + 16, Primary.WorkingArea.Y + 16);
            }
        }
    }

    private void OnClosed(object? Sender, EventArgs Event)
    {
        Dispose();
    }

    private void Schedule()
    {
        if (Queued || Disposed)
        {
            return;
        }

        Queued = true;

        Dispatcher.UIThread.Post(ApplyPending, DispatcherPriority.Render);
    }

    /// <summary>Updates a reusable transform only when usable client dimensions require a different scale.</summary>
    private void Apply()
    {
        Queued = false;

        if (Disposed || Window.WindowState is WindowState.Minimized || Window.Bounds.Width <= 0 || Window.Bounds.Height <= 0)
        {
            return;
        }

        Size Required = Host.Child is Control Body
            ? new Size(Body.MinWidth > 0 ? Body.MinWidth : Minimum.Width, Body.MinHeight > 0 ? Body.MinHeight : Minimum.Height)
            : Minimum;

        double Scale = FitScale(Required, Window.ClientSize);

        if (Math.Abs(Transform.ScaleX - Scale) <= 0.0001)
        {
            return;
        }


        Transform.ScaleX = Scale;

        Transform.ScaleY = Scale;
    }

    internal static double FitScale(Size Minimum, Size Available)
    {
        return Math.Min(1, Math.Min(Available.Width / Minimum.Width, Available.Height / Minimum.Height));
    }

    internal static Size AvailableSize(Size LogicalWorkArea)
    {
        return new Size(Math.Max(1, LogicalWorkArea.Width - 64), Math.Max(1, LogicalWorkArea.Height - 64));
    }


    public void Dispose()
    {
        if (Disposed)
        {
            return;
        }

        Disposed = true;

        Window.SizeChanged -= OnLayoutChanged;

        Window.ScalingChanged -= OnScalingChanged;

        Window.Screens.Changed -= OnDisplaysChanged;

        Window.Closed -= OnClosed;
    }
}
