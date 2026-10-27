using System.Net;
using System.Text.Json;

namespace VRCWebP;

/// <summary>Retains public server reasons without persisting arbitrary response text or echoed user data.</summary>
internal static class ServerExplanation
{
    private static readonly string[] PublicReasons =
    [
        "Application error",
        "You cannot respond to your own invites",
        "Please identify yourself with a properly formatted user agent",
        "That file is much too big",
        "File too large",
        "Image too large",
        "Image sizes must be under 10 MB",
        "Payload Too Large",
        "Request Entity Too Large",
        "Missing Credentials",
        "Authentication required",
        "Not authenticated",
        "Unauthorized",
        "Forbidden",
        "Missing Authentication Token",
        "Not logged in",
        "You don't have permission",
        "You do not have permission",
        "Permission denied",
        "Access denied",
        "File not found",
        "No image given in request",
        "You must upload a valid image",
        "Invalid file ID",
        "Invalid file extension",
        "Invalid file API URL",
        "Cannot be used as icon",
        "Cannot use that banner unless you have VRC+",
        "Cannot use that icon unless you have VRC+",
        "Custom profile banners require VRC+",
        "Profile icons require an image from the User icon inventory",
        "No frames were found at the selected start time",
        "The server image exceeds the bounded preview download limit (10 MiB)",
        "The server image exceeded the bounded preview download limit (10 MiB)",
        "The preview is temporarily unavailable",
        "you are not the owner of this file",
        "Too many requests",
        "Rate limit exceeded",
        "AccessDenied",
        "SignatureDoesNotMatch",
        "AuthorizationHeaderMalformed",
        "AuthorizationQueryParametersError",
        "InvalidAccessKeyId",
        "InvalidDigest",
        "InvalidRequest",
        "InvalidToken",
        "InvalidArgument",
        "MissingContentLength",
        "MissingSecurityHeader",
        "RequestTimeTooSkewed",
        "RequestExpired",
        "XAmzContentSHA256Mismatch",
        "NoSuchUpload",
        "IncompleteBody",
        "BadDigest",
        "EntityTooLarge",
        "EntityTooSmall",
        "InvalidPartOrder",
        "InvalidPart",
        "RequestTimeout",
        "ExpiredToken",
        "SlowDown"
    ];

    private static readonly string[] PublicFields =
    [
        "fileMd5", "fileSizeInBytes", "signatureMd5", "signatureSizeInBytes", "mimeType", "extension", "etags", "partNumber",
        "userIcon", "bannerCustomUrl", "bannerType"
    ];

    /// <summary>Accepts common API error envelopes without interpreting non-string fields as messages.</summary>
    internal static string? GetMessage(JsonElement Root)
    {
        if (Root.ValueKind is not JsonValueKind.Object)
        {
            return null;
        }

        if (Root.TryGetProperty("error", out JsonElement Error))
        {
            if (Error.ValueKind is JsonValueKind.String)
            {
                return Error.GetString();
            }

            if (Error.ValueKind is JsonValueKind.Object && Error.TryGetProperty("message", out JsonElement Detail) && Detail.ValueKind is JsonValueKind.String)
            {
                return Detail.GetString();
            }
        }

        return Root.TryGetProperty("message", out JsonElement Message) && Message.ValueKind is JsonValueKind.String ? Message.GetString() : null;
    }

    /// <summary>Copies only recognized public clauses, never unknown names, values, paths, URLs or credential-shaped text.</summary>
    internal static string Sanitize(string? Message)
    {
        if (string.IsNullOrWhiteSpace(Message))
        {
            return "No server explanation supplied";
        }

        string Normalized = Message[..Math.Min(Message.Length, 4096)].Replace('＋', '+');

        ReadOnlySpan<char> Bounded = Normalized.AsSpan();

        if (Bounded.SequenceEqual("No server explanation supplied")
            || Bounded.SequenceEqual("Unrecognized server explanation omitted for privacy"))
        {
            return Bounded.ToString();
        }

        foreach (string Reason in PublicReasons)
        {
            if (Bounded.Contains(Reason, StringComparison.OrdinalIgnoreCase))
            {
                return Reason;
            }
        }

        foreach (string Field in PublicFields)
        {
            if (Bounded.Contains(Field, StringComparison.OrdinalIgnoreCase))
            {
                if (Bounded.Contains("required", StringComparison.OrdinalIgnoreCase))
                {
                    return "Required field: " + Field;
                }

                if (Bounded.Contains("invalid", StringComparison.OrdinalIgnoreCase)
                    || Bounded.Contains("must be", StringComparison.OrdinalIgnoreCase))
                {
                    return "Invalid field: " + Field;
                }
            }
        }

        return "Unrecognized server explanation omitted for privacy";
    }
}

/// <summary>Distinguishes a server explanation from local exceptions that may contain private file paths.</summary>
internal sealed class ServerRequestException(string Message, HttpStatusCode Status, string? Explanation = null)
    : HttpRequestException(Message, null, Status)
{
    internal string SafeExplanation { get; } = ServerExplanation.Sanitize(Explanation ?? Message);
}
