namespace VRCWebP;

/// <summary>Separates generic canvas choices from presets backed by VRChat's own published material.</summary>
internal sealed record CanvasPreset(string Label, int Width, int Height, string Description)
{
    internal static readonly CanvasPreset[] All =
    [
        new("Source", 0, 0, "Keep the source dimensions, within the supported 16–4096 pixel range."),
        new("1:1", 512, 512, "Square canvas: 512 × 512 pixels. Generic ratio."),
        new("4:3", 800, 600, "Canvas: 800 × 600 pixels. Generic ratio."),
        new("Group banner: 16:9", 2000, 1125, "2000 × 1125: VRChat's official group-banner template. Nameplates crop this canvas. Source: ask.vrchat.com/t/48931."),
        new("Avatar gallery: 16:9", 1920, 1080, "1920 × 1080: VRChat recommends this canvas for avatar-gallery images. Source: creators.vrchat.com/economy/products/avatar/. The tested upload ceiling is 10 MiB."),
        new("Custom", -1, -1, "Edit width and height manually. Selecting Custom keeps the current dimensions.")
    ];

    /// <summary>Fits large sources within the encoder limit while retaining their aspect ratio.</summary>
    internal static (int Width, int Height) SourceSize(int Width, int Height)
    {
        double Scale = Math.Min(1, 4096d / Math.Max(Width, Height));

        return (Math.Clamp((int)(Width * Scale), 16, 4096), Math.Clamp((int)(Height * Scale), 16, 4096));
    }
}
