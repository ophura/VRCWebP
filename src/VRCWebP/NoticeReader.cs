using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace VRCWebP;

/// <summary>Displays complete license text as non-editable pages without scrollbars or text selection.</summary>
internal sealed class NoticeReader : Grid
{
    private readonly string Text;

    private readonly List<int> PageStarts = [0];

    private int PageIndex;

    private int PageLength;

    private CancellationTokenSource? PageChange;

    private readonly TextBlock Page = new()
    {
        FontSize = 12,

        LineHeight = 18,

        TextWrapping = TextWrapping.Wrap,

        VerticalAlignment = VerticalAlignment.Top
    };

    private readonly TextBlock Measurement = new()
    {
        FontSize = 12,

        LineHeight = 18,

        TextWrapping = TextWrapping.Wrap
    };

    private readonly Border Viewport = new()
    {
        ClipToBounds = true
    };

    private readonly TextBlock Position = new()
    {
        VerticalAlignment = VerticalAlignment.Center,

        Foreground = Brush.Parse("#A8A8A8")
    };

    private readonly Button Previous = Icons.Button("back", "Previous page");

    private readonly Button Next = Icons.Button("arrow", "Next page");

    internal NoticeReader(string Text)
    {
        this.Text = Text;

        DetachedFromVisualTree += (_, _) => PageChange?.Cancel();

        if (Text.Length is 0)
        {
            Previous.IsEnabled = false;

            Next.IsEnabled = false;
        }

        RowDefinitions = new RowDefinitions("*,Auto");

        RowSpacing = 12;

        Viewport.Child = Page;

        Children.Add(Viewport);

        StackPanel Navigation = new()
        {
            Orientation = Orientation.Horizontal,

            HorizontalAlignment = HorizontalAlignment.Right,

            Spacing = 12,

            Children = { Position, Previous, Next }
        };

        Grid.SetRow(Navigation, 1);

        Children.Add(Navigation);

        Viewport.SizeChanged += (_, _) =>
        {
            PageStarts.Clear();

            PageStarts.Add(0);

            PageIndex = 0;

            ShowPage();
        };

        Previous.Click += (_, _) =>
        {
            --PageIndex;

            ShowPage();
        };

        Next.Click += (_, _) =>
        {
            int Start = PageStarts[PageIndex] + PageLength;

            if (PageStarts.Count <= PageIndex + 1)
            {
                PageStarts.Add(Start);
            }

            ++PageIndex;

            ShowPage();
        };
    }

    /// <summary>Measures a page against its actual viewport so every line remains visible.</summary>
    private void ShowPage()
    {
        if (Text.Length is 0)
        {
            return;
        }

        double Width = Viewport.Bounds.Width;

        double Height = Viewport.Bounds.Height;

        if (Width <= 0 || Height < 18)
        {
            return;
        }

        int Start = PageStarts[PageIndex];

        // Measure with exactly the typeface used by the attached page, including
        // inherited font properties. The measuring control is never rendered.
        Measurement.FontFamily = Page.FontFamily;

        Measurement.FontSize = Page.FontSize;

        Measurement.FontStyle = Page.FontStyle;

        Measurement.FontWeight = Page.FontWeight;

        Measurement.FontStretch = Page.FontStretch;

        Measurement.LetterSpacing = Page.LetterSpacing;

        Measurement.LineHeight = Page.LineHeight;

        Measurement.FlowDirection = Page.FlowDirection;

        int Low = 0;

        int High = Math.Min(Text.Length - Start, 8192);

        while (Low < High)
        {
            int Length = (Low + High + 1) / 2;

            Measurement.Text = Text.AsSpan(Start, Length).TrimEnd().ToString();

            Measurement.Measure(new Size(Width, double.PositiveInfinity));

            if (Measurement.DesiredSize.Height <= Height - 0.5)
            {
                Low = Length;
            }
            else
            {
                High = Length - 1;
            }
        }

        PageLength = Math.Max(1, Low);

        if (Start + PageLength < Text.Length)
        {
            int Boundary = ParagraphBoundary(Start, PageLength);

            if (Boundary > Start)
            {
                PageLength = Boundary - Start;
            }
            else
            {
                // A paragraph taller than the viewport must continue on the next page.
                int Break = Text.LastIndexOfAny([' ', '\n'], Start + PageLength - 1, PageLength);

                if (Break >= Start)
                {
                    PageLength = Break - Start + 1;
                }
            }
        }

        string Contents = Text.AsSpan(Start, PageLength).TrimEnd().ToString();

        PageChange?.Cancel();

        if (string.IsNullOrEmpty(Page.Text) || !IsEffectivelyVisible)
        {
            Page.Text = Contents;

            Position.Text = $"{PageIndex + 1}";
        }
        else
        {
            CancellationTokenSource Pending = new();

            PageChange = Pending;

            _ = ChangePageAsync(Contents, $"{PageIndex + 1}", Page.Opacity, Pending);
        }

        Previous.IsEnabled = PageIndex > 0;

        Next.IsEnabled = Start + PageLength < Text.Length;
    }

    /// <summary>Replaces a page while invisible, avoiding overlapping paragraphs or snapshots outside its clip.</summary>
    private async Task ChangePageAsync(string Contents, string Number, double From, CancellationTokenSource Pending)
    {
        TimeSpan Half = UiMotion.Duration / 2;

        try
        {
            await Task.WhenAll(UiMotion.FadeAsync(Page, From, 0, Interval: Half, Token: Pending.Token),
                UiMotion.FadeAsync(Position, Position.Opacity, 0, Interval: Half, Token: Pending.Token));

            Pending.Token.ThrowIfCancellationRequested();

            Page.Text = Contents;

            Position.Text = Number;

            await Task.WhenAll(UiMotion.FadeAsync(Page, 0, 1, Interval: Half, Token: Pending.Token),
                UiMotion.FadeAsync(Position, 0, 1, Interval: Half, Token: Pending.Token));
        }
        catch (OperationCanceledException)
        {
            // Rapid navigation or a resized/closed reader supersedes this page.
        }
        finally
        {
            if (ReferenceEquals(PageChange, Pending))
            {
                PageChange = null;
            }

            Pending.Dispose();
        }
    }

    /// <summary>Finds the last blank-line paragraph separator within the measured page.</summary>
    private int ParagraphBoundary(int Start, int Length)
    {
        int Limit = Start + Length;

        int Boundary = Start;

        for (int LineStart = Start; LineStart < Limit;)
        {
            int End = Text.IndexOf('\n', LineStart, Limit - LineStart);

            if (End < 0)
            {
                break;
            }

            if (Text.AsSpan(LineStart, End - LineStart).IsWhiteSpace())
            {
                Boundary = End + 1;
            }

            LineStart = End + 1;
        }

        return Boundary;
    }
}
