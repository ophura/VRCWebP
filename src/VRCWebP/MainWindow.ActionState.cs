using System.Globalization;

namespace VRCWebP;

internal sealed partial class MainWindow
{
    /// <summary>Rejects incomplete numeric edits before an action can consume the previous parsed value.</summary>
    private string? RecipeInputReason()
    {
        if (!((NumberField)CanvasWidth).IsInputValid || !((NumberField)CanvasHeight).IsInputValid)
        {
            return "Enter valid canvas dimensions in pixels.";
        }

        if (!((NumberField)Start).IsInputValid || !((NumberField)Duration).IsInputValid || !((NumberField)Loops).IsInputValid)
        {
            return "Enter valid Start, Length and Loops values.";
        }

        if (InputInfo?.Duration > 0 && (double)((Start.Value ?? 0) + (Duration.Value ?? 6)) > InputInfo.Duration + 0.05)
        {
            return "The clip extends past the source.\nReduce its start time or length.";
        }

        return null;
    }

    /// <summary>Compares only export parameters; preview duration never changes a committed file.</summary>
    private bool HasCurrentOutput() => Result is not null && RecipeInputReason() is null && Result.Settings == ReadSettings();

    /// <summary>Disables duplicate creation without blocking access to the last successful output.</summary>
    private string? CreateUnavailableReason() => Busy ? "Wait for the current task to finish."
        : InputPath is null ? "Open a media file first."
        : RecipeInputReason() is string Reason ? Reason
        : HasCurrentOutput() ? "This WebP has already been created.\nChange an output parameter to create another."
        : null;

    /// <summary>Requires the selected destination's prerequisites and a canonical target before enabling Apply.</summary>
    private string? ImageApplyUnavailableReason()
    {
        if (Busy)
        {
            return "Wait for the current task to finish.";
        }

        if (Api.UserId is null)
        {
            return "Connect your browser first.";
        }

        int Index = Purpose.SelectedIndex;

        if (Index is 5 or 6)
        {
            return null;
        }

        if (Uploaded is null)
        {
            return "Select a complete uploaded image first.";
        }

        if (Index < 0 || !ImagePurposes[Index].IsEnabled)
        {
            return ImagePurposeDescription(Index);
        }

        if (Index is >= 3)
        {
            return null;
        }

        string Prefix = Index is 2 ? "avtr_" : "grp_";

        return VRChatService.IsTargetId(Target.Text?.Trim() ?? "", Prefix) ? null
            : Index is 2 ? "Enter a complete avtr_ ID for an avatar you own." : "Enter a complete grp_ ID for the group.";
    }

    /// <summary>Formats the exact committed byte count into bounded stack storage.</summary>
    private static string FormatCreatedFileBytes(long Bytes)
    {
        Span<char> Text = stackalloc char[64];

        return Text.TryWrite(CultureInfo.InvariantCulture, $"Encoded WebP size: {Bytes:N0} bytes", out int Written)
            ? new string(Text[..Written]) : throw new InvalidOperationException("The file-size label exceeded its bound.");
    }

    /// <summary>Displays the destination-specific ceiling once, beside its selector's label.</summary>
    private static string FormatUploadCaption(long Bytes)
    {
        Span<char> Text = stackalloc char[80];

        return Text.TryWrite(CultureInfo.InvariantCulture, $"Upload to ({Bytes:N0} maximum bytes)", out int Written)
            ? new string(Text[..Written]) : throw new InvalidOperationException("The upload-limit label exceeded its bound.");
    }
}
