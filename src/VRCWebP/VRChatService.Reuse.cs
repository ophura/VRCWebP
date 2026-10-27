using System.Globalization;
using System.Text.Json;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    internal const string WebPAvatarExplanation = "VRChat rejected .webp-declared records for avatar thumbnails. New uploads use the verified PNG declaration; this existing file cannot be relabelled here.";

    internal const string ForeignAvatarThumbnailExplanation = "VRChat requires the avatar thumbnail file to belong to the connected account. Reuse one of your own uploads.";

    /// <summary>Keeps the observed record-extension restriction separate from image bytes and file ownership.</summary>
    internal string? AvatarThumbnailUnavailableReason(UploadResult Image) => Image.OwnerId is string Owner && Owner != UserId
        ? ForeignAvatarThumbnailExplanation : Image.Extension is ".webp" ? WebPAvatarExplanation : null;

    /// <summary>Resolves a complete existing image without creating records, transferring image bytes or changing its tags.</summary>
    internal async Task<UploadResult> ReuseImageAsync(string Reference, CancellationToken Token)
    {
        RequireLogin();

        (string FileId, int? RequestedVersion) = ParseImageReference(Reference);

        try
        {
            using JsonDocument Document = Read(await Files.GetFileWithHttpInfoAsync(FileId, Token));

            JsonElement Root = Document.RootElement;

            if (Text(Root, "id") != FileId || (Root.TryGetProperty("deleted", out JsonElement Removed) && Removed.ValueKind is JsonValueKind.True) || !Root.TryGetProperty("versions", out JsonElement Versions) || Versions.ValueKind is not JsonValueKind.Array)
            {
                throw new InvalidDataException("VRChat returned no matching image record.");
            }

            string Mime = Text(Root, "mimeType");

            string Extension = Text(Root, "extension").ToLowerInvariant();

            if (Mime is not ("image/webp" or "image/png" or "image/jpeg") || Extension is not (".webp" or ".png" or ".jpg" or ".jpeg"))
            {
                throw new InvalidDataException("This file record is not a supported VRChat image.");
            }

            JsonElement Selected = default;

            int Version = 0;

            foreach (JsonElement Entry in Versions.EnumerateArray())
            {
                if (!Entry.TryGetProperty("version", out JsonElement Number) || !Number.TryGetInt32(out int Candidate) || Candidate < 1 || (Entry.TryGetProperty("deleted", out JsonElement Deleted) && Deleted.ValueKind is JsonValueKind.True))
                {
                    continue;
                }

                if (RequestedVersion is int Wanted ? Candidate == Wanted : Candidate > Version)
                {
                    Version = Candidate;

                    Selected = Entry;
                }
            }

            if (Version is 0 || Text(Selected, "status") is not "complete"
                || !Selected.TryGetProperty("file", out JsonElement Image) || Text(Image, "status") is not "complete")
            {
                throw new InvalidDataException("This image version is missing, deleted or still processing. Only complete images can be reused.");
            }

            await UploadLog.WriteAsync("Reuse image", Status: 200);

            string OwnerId = Text(Root, "ownerId");

            return new UploadResult(FileId, $"https://api.vrchat.cloud/api/1/file/{FileId}/{Version}/file", Version, "Existing image ready", Mime, OwnerId.Length is 0 ? null : OwnerId, Extension, HasImageTag(Root, "icon"));
        }
        catch (Exception Error) when (Error is not OperationCanceledException)
        {
            await LogImageFailureAsync("Reuse image", Error);

            throw;
        }
    }

    /// <summary>Throws on invalid references before issuing any network request.</summary>
    internal static (string FileId, int? Version) ParseImageReference(string Reference)
    {
        return TryParseImageReference(Reference, out (string FileId, int? Version) Parsed, out string? Reason, Canonicalize: true)
            ? Parsed : throw new ArgumentException(Reason, nameof(Reference));
    }

    /// <summary>Shares non-throwing input validation between the UI and service; arbitrary URLs are never fetched.</summary>
    internal static bool TryParseImageReference(string Reference, out (string FileId, int? Version) Parsed, out string? Reason, bool Canonicalize = false)
    {
        Parsed = default;

        Reason = "Enter a complete file_ ID or an HTTPS VRChat original-file URL.";

        string Value = Reference.Trim();

        string Id = Value;

        int? Version = null;

        if (Uri.TryCreate(Value, UriKind.Absolute, out Uri? Link))
        {
            if (Link.Scheme is not "https" || !Link.IsDefaultPort || Link.UserInfo.Length is not 0 || Link.Query.Length is not 0
                || Link.Fragment.Length is not 0 || Link.Host is not ("api.vrchat.cloud" or "api.vrchat.com"))
            {
                return false;
            }

            ReadOnlySpan<char> Path = Link.AbsolutePath.AsSpan();

            Span<Range> Parts = stackalloc Range[8];

            int Count = Path.Split(Parts, '/', StringSplitOptions.RemoveEmptyEntries);

            if (Count is not (4 or 6) || !Path[Parts[0]].SequenceEqual("api") || !Path[Parts[1]].SequenceEqual("1")
                || !Path[Parts[2]].SequenceEqual("file"))
            {
                Reason = "Use an original-file URL.\nThumbnail links and website pages cannot be reused.";

                return false;
            }

            Id = Path[Parts[3]].ToString();

            if (Count is 6)
            {
                if (!Path[Parts[5]].SequenceEqual("file") || !int.TryParse(Path[Parts[4]], NumberStyles.None, CultureInfo.InvariantCulture, out int Number) || Number < 1)
                {
                    Reason = "The file URL must identify a valid image version.";

                    return false;
                }

                Version = Number;
            }
        }

        if (!IsTargetId(Id, "file_") || !Guid.TryParseExact(Id.AsSpan(5), "D", out Guid Identifier))
        {
            return false;
        }

        Parsed = (Canonicalize ? "file_" + Identifier.ToString("D") : Id, Version);

        Reason = null;

        return true;
    }

    /// <summary>Accepts only canonical UUID identifiers for the requested resource type.</summary>
    internal static bool IsTargetId(string Id, string Prefix) => Id.Length == Prefix.Length + 36 && Id.StartsWith(Prefix, StringComparison.Ordinal) && Guid.TryParseExact(Id.AsSpan(Prefix.Length), "D", out _);

    /// <summary>Persists only known public reasons and normalized operation names for image lookup and assignment failures.</summary>
    private static Task LogImageFailureAsync(string Stage, Exception Error)
    {
        int? Status = Error switch
        {
            HttpRequestException Http => (int?)Http.StatusCode,

            VRChat.API.Client.ApiException Api => Api.ErrorCode,

            _ => null
        };

        return UploadLog.WriteAsync(Stage, Error.GetType().Name, Status, Error is ServerRequestException Server ? Server.SafeExplanation : ServerExplanation.Sanitize(Error.Message));
    }
}
