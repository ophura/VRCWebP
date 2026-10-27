using System.Globalization;

namespace VRCWebP;

/// <summary>Applies the measured maximum original length before an upload can create remote state.</summary>
internal static class UploadPolicy
{
    // Generic-file metadata admission is not the supported PNG-compatible image-transfer limit.
    internal const long MaximumBytes = 1288490188;

    internal const long OriginalImageMaximumBytes = 10000000;

    internal const string MaximumDescription = "Maximum original-image upload: 10 MB (10,000,000 bytes), inclusive. Both lossless and lossy WebP are accepted. This upload route cannot complete larger images; reduce the output or save locally.";

    // Live boundary tests: 10,485,760 bytes accepted unchanged; 10,485,762 rejected, 25 October 2026.
    internal const long AvatarGalleryMaximumBytes = 10L * 1024 * 1024;

    internal static long MaximumFor(UploadDestination Destination) => Destination is UploadDestination.Original
        ? OriginalImageMaximumBytes : AvatarGalleryMaximumBytes;

    internal static string DescriptionFor(UploadDestination Destination) => Destination is UploadDestination.AvatarGallery
        ? "Avatar-gallery maximum: 10 MiB (10,485,760 bytes), inclusive. Six images per avatar."
        : Destination is UploadDestination.Original ? MaximumDescription : "Image-inventory maximum: 10 MiB (10,485,760 bytes), inclusive.";

    internal static bool IsWithinLimit(long Bytes, UploadDestination Destination = UploadDestination.Original) => Bytes > 0 && Bytes <= MaximumFor(Destination);

    /// <summary>Rejects empty or oversized streams without hashing, reading their bodies or contacting VRChat.</summary>
    internal static void Validate(long Bytes, UploadDestination Destination = UploadDestination.Original)
    {
        if (!IsWithinLimit(Bytes, Destination))
        {
            throw new InvalidDataException(DescribeRejection(Bytes, Destination));
        }
    }

    internal static string DescribeRejection(long Bytes, UploadDestination Destination = UploadDestination.Original) => Bytes <= 0
        ? "The upload file is empty."
        : string.Create(CultureInfo.InvariantCulture,
            $"Upload blocked: the file is {Bytes:N0} bytes. The maximum for this destination is {MaximumFor(Destination):N0} bytes ({(Destination is UploadDestination.Original ? "10 MB" : "10 MiB")}). Save locally or reduce the resolution, duration or frame rate.");
}
