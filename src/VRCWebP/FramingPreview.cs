using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;

namespace VRCWebP;

/// <summary>Shows the exported canvas over the uncropped source and lets direct manipulation set the export offset.</summary>
internal sealed class FramingPreview : Control
{
    internal static readonly StyledProperty<IImage?> SourceProperty = AvaloniaProperty.Register<FramingPreview, IImage?>(nameof(Source));

    public IImage? Source
    {
        get => GetValue(SourceProperty);

        set => SetValue(SourceProperty, value);
    }

    static FramingPreview()
    {
        AffectsRender<FramingPreview>(SourceProperty);
    }

    private static readonly IBrush CanvasBrush = SolidColorBrush.Parse("#303030");

    private static readonly IBrush CheckerBrush = SolidColorBrush.Parse("#393939");

    private static readonly IBrush ExcludedBrush = SolidColorBrush.Parse("#BD101010");

    private static readonly Pen FrameShadow = new(Brushes.Black, 3);

    private static readonly Pen FrameEdge = new(SolidColorBrush.Parse("#DDDDDD"), 1);

    private static readonly Cursor DragCursor = new(StandardCursorType.SizeAll);

    private FormattedText? FrameCaption;

    private Point? PreviousPoint;

    private IPointer? CapturedPointer;

    private readonly DispatcherTimer Motion = new()
    {
        Interval = TimeSpan.FromMilliseconds(16)
    };

    private long MotionStarted;

    private Rect PreviousFrame;

    private Rect PreviousPicture;

    private ConversionSettings? Recipe;

    private Size SourceSize;

    /// <summary>Encoded pixels already include framing; draw them once inside the output canvas.</summary>
    internal bool EncodedSource { get; set; }

    internal double PanX { get; private set; } = 0.5;

    internal double PanY { get; private set; } = 0.5;

    internal event Action? FramingChanged;

    internal FramingPreview()
    {
        Motion.Tick += (_, _) =>
        {
            if (System.Diagnostics.Stopwatch.GetElapsedTime(MotionStarted) >= UiMotion.Duration)
            {
                Motion.Stop();
            }

            InvalidateVisual();
        };

        SizeChanged += (_, _) => Motion.Stop();
    }

    /// <summary>Changes the canvas without decoding or encoding another animation.</summary>
    internal void Configure(ConversionSettings Settings, MediaInfo? Info)
    {
        bool Animate = Source is not null && Recipe is not null && SourceSize.Width > 0 && SourceSize.Height > 0
            && Bounds.Width > 64 && Bounds.Height > 64
            && (Recipe.Width != Settings.Width || Recipe.Height != Settings.Height || Recipe.Fit != Settings.Fit);

        if (Animate)
        {
            (PreviousFrame, PreviousPicture) = VisualGeometry();
        }

        Recipe = Settings;

        SourceSize = Info is null ? Source?.Size ?? default : new Size(Info.Width, Info.Height);

        if (Animate)
        {
            MotionStarted = System.Diagnostics.Stopwatch.GetTimestamp();

            Motion.Start();
        }

        InvalidateVisual();
    }

    /// <summary>Resets the offset on import or an explicit double click.</summary>
    internal void ResetFraming(bool Animate = false)
    {
        bool HasFrame = Animate && Source is not null && Recipe is not null && SourceSize.Width > 0 && SourceSize.Height > 0;

        if (HasFrame)
        {
            (PreviousFrame, PreviousPicture) = VisualGeometry();
        }

        PanX = 0.5;

        PanY = 0.5;

        if (HasFrame)
        {
            MotionStarted = System.Diagnostics.Stopwatch.GetTimestamp();

            Motion.Start();
        }

        InvalidateVisual();
    }

    /// <summary>Draws included pixels, transparent margins and dimmed excluded pixels in one coordinate system.</summary>
    public override void Render(DrawingContext Context)
    {
        if (Source is null || Recipe is null || SourceSize.Width <= 0 || SourceSize.Height <= 0)
        {
            return;
        }

        (Rect Frame, Rect Picture) = VisualGeometry();

        if (Frame.Width <= 0 || Frame.Height <= 0)
        {
            return;
        }

        using (Context.PushClip(new Rect(Bounds.Size)))
        {
            Context.FillRectangle(CanvasBrush, Frame);

            using (Context.PushClip(Frame))
            {
                for (double Y = Frame.Y; Y < Frame.Bottom; Y += 32)
                {
                    for (double X = Frame.X; X < Frame.Right; X += 32)
                    {
                        if (((int)((X - Frame.X) / 32) + (int)((Y - Frame.Y) / 32)) % 2 is 0)
                        {
                            Context.FillRectangle(CheckerBrush, new Rect(X, Y, 32, 32));
                        }
                    }
                }
            }

            Context.DrawImage(Source, new Rect(Source.Size), Picture);

            Context.FillRectangle(ExcludedBrush, new Rect(0, 0, Bounds.Width, Frame.Top));

            Context.FillRectangle(ExcludedBrush, new Rect(0, Frame.Bottom, Bounds.Width, Math.Max(0, Bounds.Height - Frame.Bottom)));

            Context.FillRectangle(ExcludedBrush, new Rect(0, Frame.Top, Frame.Left, Frame.Height));

            Context.FillRectangle(ExcludedBrush, new Rect(Frame.Right, Frame.Top, Math.Max(0, Bounds.Width - Frame.Right), Frame.Height));

            Context.DrawRectangle(null, FrameShadow, Frame);

            Context.DrawRectangle(null, FrameEdge, Frame);

            FrameCaption ??= new FormattedText("Output frame", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                new Typeface(new FontFamily("avares://VRCWebP/Assets/Fonts#Cascadia Code")), 11, Brushes.LightGray);

            Context.DrawText(FrameCaption, new Point(Frame.Left, Frame.Top - 22));
        }
    }

    /// <summary>Interpolates only recipe changes; direct dragging and resizing continue to follow the pointer immediately.</summary>
    private (Rect Frame, Rect Picture) VisualGeometry()
    {
        if (Recipe is null)
        {
            return default;
        }

        (Rect Frame, Rect Picture) = Geometry(Bounds.Size, SourceSize, Recipe.Width, Recipe.Height, Recipe.Fit, PanX, PanY);

        if (EncodedSource)
        {
            return (Frame, Frame);
        }

        if (!Motion.IsEnabled)
        {
            return (Frame, Picture);
        }

        double Progress = Math.Clamp(System.Diagnostics.Stopwatch.GetElapsedTime(MotionStarted).TotalSeconds / UiMotion.Duration.TotalSeconds, 0, 1);

        double Eased = 1 - Math.Pow(1 - Progress, 3);

        return (Interpolate(PreviousFrame, Frame, Eased), Interpolate(PreviousPicture, Picture, Eased));
    }

    private static Rect Interpolate(Rect From, Rect To, double Progress) => new(
        From.X + ((To.X - From.X) * Progress),
        From.Y + ((To.Y - From.Y) * Progress),
        From.Width + ((To.Width - From.Width) * Progress),
        From.Height + ((To.Height - From.Height) * Progress));

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs Event)
    {
        Motion.Stop();

        ReleaseDrag();

        base.OnDetachedFromVisualTree(Event);
    }

    /// <summary>Computes the same normalized crop or transparent padding used by the encoder.</summary>
    internal static (Rect Frame, Rect Picture) Geometry(Size Viewport, Size Source, int Width, int Height, string Fit, double X, double Y)
    {
        double AvailableWidth = Math.Max(0, Viewport.Width - 64);

        double AvailableHeight = Math.Max(0, Viewport.Height - 64);

        double CanvasScale = Math.Min(AvailableWidth / Width, AvailableHeight / Height);

        Size Canvas = new(Width * CanvasScale, Height * CanvasScale);

        Rect Frame = new((Viewport.Width - Canvas.Width) / 2, (Viewport.Height - Canvas.Height) / 2, Canvas.Width, Canvas.Height);

        double Scale = Fit is "Fill" ? Math.Max(Canvas.Width / Source.Width, Canvas.Height / Source.Height)
            : Math.Min(Canvas.Width / Source.Width, Canvas.Height / Source.Height);

        Size Picture = new(Source.Width * Scale, Source.Height * Scale);

        return (Frame, new Rect(Frame.X + ((Canvas.Width - Picture.Width) * X), Frame.Y + ((Canvas.Height - Picture.Height) * Y),
            Picture.Width, Picture.Height));
    }

    protected override void OnPointerPressed(PointerPressedEventArgs Event)
    {
        base.OnPointerPressed(Event);

        if (Recipe is null || Source is null || !Event.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            return;
        }

        Motion.Stop();

        if (Event.ClickCount is 2)
        {
            ResetFraming(Animate: true);

            FramingChanged?.Invoke();

            Event.Handled = true;

            return;
        }

        TooltipFlow.MoveTo(null);

        PreviousPoint = Event.GetPosition(this);

        CapturedPointer = Event.Pointer;

        Event.Pointer.Capture(this);

        Cursor = DragCursor;

        Event.Handled = true;
    }

    protected override void OnPointerMoved(PointerEventArgs Event)
    {
        base.OnPointerMoved(Event);

        if (PreviousPoint is not Point Previous || Recipe is null)
        {
            return;
        }

        TooltipFlow.MoveTo(null);

        Point Current = Event.GetPosition(this);

        (Rect Frame, Rect Picture) = Geometry(Bounds.Size, SourceSize, Recipe.Width, Recipe.Height, Recipe.Fit, PanX, PanY);

        double Horizontal = Frame.Width - Picture.Width;

        double Vertical = Frame.Height - Picture.Height;

        double X = Math.Abs(Horizontal) < 0.01 ? PanX : Math.Clamp(PanX + ((Current.X - Previous.X) / Horizontal), 0, 1);

        double Y = Math.Abs(Vertical) < 0.01 ? PanY : Math.Clamp(PanY + ((Current.Y - Previous.Y) / Vertical), 0, 1);

        PreviousPoint = Current;

        if (X != PanX || Y != PanY)
        {
            PanX = X;

            PanY = Y;

            InvalidateVisual();

            FramingChanged?.Invoke();
        }

        Event.Handled = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs Event)
    {
        base.OnPointerReleased(Event);

        ReleaseDrag();
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs Event)
    {
        base.OnPointerCaptureLost(Event);

        PreviousPoint = null;

        CapturedPointer = null;

        Cursor = null;
    }

    /// <summary>Ends capture before work disables the preview or the window closes.</summary>
    internal void ReleaseDrag()
    {
        PreviousPoint = null;

        IPointer? Pointer = CapturedPointer;

        CapturedPointer = null;

        Pointer?.Capture(null);

        Cursor = null;
    }
}
