namespace VRCWebP;

/// <summary>Describes a source video stream in display coordinates after rotation.</summary>
internal sealed record MediaInfo(int Width, int Height, double Duration, double FrameRate);

/// <summary>Identifies the sole exported animation container.</summary>
internal static class WebPFormat
{
    internal const string Name = "WebP";

    internal const string Extension = ".webp";

    internal const string MimeType = "image/webp";
}

/// <summary>Declares VRChat's tested image record type while retaining the original animated WebP bytes unchanged.</summary>
internal static class VRChatImageRecord
{
    internal const string Extension = ".png";

    internal const string MimeType = "image/png";
}

/// <summary>Defines the complete, immutable recipe associated with one exported animation.</summary>
internal sealed record ConversionSettings(int Width, int Height, int FrameRate, double Start, double Duration, string Fit, int Compression, int Loops, bool Lossless = true, int Quality = 80, double PanX = 0.5, double PanY = 0.5);

/// <summary>Separates reusable originals from entitlement-controlled website image inventories.</summary>
internal enum UploadDestination
{
    Original,

    Gallery,

    UserIcon,

    AvatarGallery
}

/// <summary>Contains verified output metadata; the file is owned by the workspace until exported.</summary>
internal sealed record ConversionResult(string Path, int Width, int Height, uint Frames, long Bytes, ConversionSettings Settings);

/// <summary>Identifies a reusable server file independently of any publication target.</summary>
internal sealed record UploadResult(string FileId, string Url, int Version, string Status, string MimeType = VRChatImageRecord.MimeType, string? OwnerId = null, string Extension = VRChatImageRecord.Extension, bool ProfileIconEligible = false);

/// <summary>Contains a server-side image summary without loading its bytes.</summary>
internal sealed record RemoteImageFile(string Id, string Name, string Extension, long Bytes, int Version, bool Complete);

/// <summary>Pages an owned-file inventory independently from its displayed image rows.</summary>
internal sealed record RemoteImageBatch(IReadOnlyList<RemoteImageFile> Images, int Offset, bool HasMore);

/// <summary>Conveys a measured progress value and the current operation.</summary>
internal sealed record WorkProgress(double Percent, string Stage);

/// <summary>Identifies the ordered images belonging to an owned avatar without downloading their bodies.</summary>
internal sealed record AvatarGalleryState(string AvatarId, IReadOnlyList<string> FileIds);
