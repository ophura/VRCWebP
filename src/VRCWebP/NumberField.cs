using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using System.Globalization;

namespace VRCWebP;

/// <summary>Keeps numeric editing quiet, with rounded spinner edges and explicit selection only.</summary>
internal sealed class NumberField : NumericUpDown
{
    private TextBox? Editor;

    private readonly RectangleGeometry ClipGeometry = new()
    {
        RadiusX = 8,

        RadiusY = 8
    };

    private ButtonSpinner? Spinner;

    internal byte Phase { get; private set; }

    /// <summary>Accepts a valid exact value or its rounded display text, while rejecting incomplete or stale edits.</summary>
    internal bool IsInputValid
    {
        get
        {
            if (Value is not decimal Current || Current < Minimum || Current > Maximum)
            {
                return false;
            }

            if (Editor is null)
            {
                return true;
            }

            if (!decimal.TryParse(Editor.Text.AsSpan(), ParsingNumberStyle, CultureInfo.InvariantCulture, out decimal Parsed))
            {
                return false;
            }

            if (Parsed == Current)
            {
                return true;
            }

            Span<char> Formatted = stackalloc char[64];

            return Current.TryFormat(Formatted, out int Written, FormatString, CultureInfo.InvariantCulture)
                && decimal.TryParse(Formatted[..Written], ParsingNumberStyle, CultureInfo.InvariantCulture, out decimal Rounded)
                && Parsed == Rounded;
        }
    }

    protected override Type StyleKeyOverride => typeof(NumericUpDown);

    internal NumberField()
    {
        Focusable = false;

        AddHandler(PointerPressedEvent, OnPointerPressedBeforeFocus, RoutingStrategies.Tunnel, true);

        AddHandler(TextInputEvent, (_, Event) =>
        {
            if (Editor is not null && !AcceptsReplacement(Event.Text ?? ""))
            {
                Event.Handled = true;
            }
        }, RoutingStrategies.Tunnel);

        AddHandler(TextBox.PastingFromClipboardEvent, (_, Event) =>
        {
            Event.Handled = true;

            _ = PasteNumberAsync();
        });
    }

    /// <summary>Accepts unsigned integers or decimal seconds; exponents, signs and grouping are not editing forms.</summary>
    private bool AcceptsReplacement(string Input)
    {
        if (Editor is null || Input.Length > 64)
        {
            return false;
        }

        string Text = Editor.Text ?? "";

        int Start = Math.Min(Editor.SelectionStart, Editor.SelectionEnd);

        int End = Math.Max(Editor.SelectionStart, Editor.SelectionEnd);

        int Length = Text.Length - (End - Start) + Input.Length;

        if (Length > 64)
        {
            return false;
        }

        Span<char> Candidate = stackalloc char[64];

        Text.AsSpan(0, Start).CopyTo(Candidate);

        Input.AsSpan().CopyTo(Candidate[Start..]);

        Text.AsSpan(End).CopyTo(Candidate[(Start + Input.Length)..]);

        bool Point = false;

        for (int Index = 0; Index < Length; ++Index)
        {
            char Character = Candidate[Index];

            if (Character is >= '0' and <= '9')
            {
                continue;
            }

            if (Increment < 1 && Character is '.' && !Point)
            {
                Point = true;

                continue;
            }

            return false;
        }

        return Length is 0 || (Length is 1 && Candidate[0] is '.')
            || decimal.TryParse(Candidate[..Length], ParsingNumberStyle, CultureInfo.InvariantCulture, out _);
    }

    /// <summary>Filters paste before insertion and abandons clipboard reads superseded by focus or selection changes.</summary>
    private async Task PasteNumberAsync()
    {
        TextBox? Target = Editor;

        if (Target?.IsFocused is not true || TopLevel.GetTopLevel(this)?.Clipboard is not { } Clipboard)
        {
            return;
        }

        string? Previous = Target.Text;

        int Start = Target.SelectionStart;

        int End = Target.SelectionEnd;

        try
        {
            string? Input = await Clipboard.TryGetTextAsync();

            if (Input is not null && ReferenceEquals(Editor, Target) && Target.IsFocused && Target.Text == Previous
                && Target.SelectionStart == Start && Target.SelectionEnd == End && AcceptsReplacement(Input))
            {
                Target.SelectedText = Input;
            }
        }
        catch (Exception Error) when (Error is System.Runtime.InteropServices.ExternalException or NotSupportedException)
        {
            // An unavailable clipboard leaves the existing numeric input intact.
        }
    }

    /// <summary>Spinner buttons change values without entering text-editing mode.</summary>
    private void OnPointerPressedBeforeFocus(object? Sender, PointerPressedEventArgs Event)
    {
        for (Visual? Source = Event.Source as Visual; Source is not null && Source != this; Source = Source.GetVisualParent())
        {
            if (Source is RepeatButton)
            {
                _ = (TopLevel.GetTopLevel(this)?.FocusManager?.Focus(null));

                break;
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs Event)
    {
        if (Spinner is not null)
        {
            Spinner.TemplateApplied -= OnSpinnerTemplate;

            Spinner.SizeChanged -= OnSpinnerSizeChanged;
        }

        base.OnApplyTemplate(Event);

        Editor = Event.NameScope.Find<TextBox>("PART_TextBox");

        Editor?.MaxLength = 64;

        Spinner = Event.NameScope.Find<ButtonSpinner>("PART_Spinner");

        if (Spinner is not null)
        {
            Spinner.TemplateApplied += OnSpinnerTemplate;

            Spinner.SizeChanged += OnSpinnerSizeChanged;

            Spinner.Clip = ClipGeometry;
        }
    }

    private void OnSpinnerSizeChanged(object? Sender, SizeChangedEventArgs Event)
    {
        ClipGeometry.Rect = new Rect(Event.NewSize);
    }

    private void OnSpinnerTemplate(object? Sender, TemplateAppliedEventArgs Event)
    {
        string Name = (AutomationProperties.GetName(this) ?? "value").ToLowerInvariant();

        SetSpinTip(Event.NameScope.Find<RepeatButton>("PART_IncreaseButton"), "Increase " + Name);

        SetSpinTip(Event.NameScope.Find<RepeatButton>("PART_DecreaseButton"), "Decrease " + Name);
    }

    private static void SetSpinTip(RepeatButton? Button, string Text)
    {
        if (Button is not null)
        {
            ToolTip.SetTip(Button, Text);

            ToolTip.SetShowOnDisabled(Button, true);
        }
    }

    /// <summary>Removes the framework's focus-time SelectAll while keeping manual selection available.</summary>
    protected override void OnGotFocus(FocusChangedEventArgs Event)
    {
        // NumericUpDown's base redirects descendant focus and calls SelectAll.
        // The editor owns focus directly; leave pointer placement and drag selection intact.
        if (Event.NavigationMethod is not NavigationMethod.Pointer)
        {
            Editor?.ClearSelection();
        }
    }

    protected override void OnSpin(SpinEventArgs Event)
    {
        int Caret = Editor?.CaretIndex ?? 0;

        Phase = unchecked((byte)((Phase << 1) | (Event.Direction is SpinDirection.Increase ? 1 : 0)));

        base.OnSpin(Event);

        if (Editor?.IsFocused is true)
        {
            Editor.CaretIndex = Math.Min(Caret, Editor.Text?.Length ?? 0);

            Editor.ClearSelection();
        }
    }
}
