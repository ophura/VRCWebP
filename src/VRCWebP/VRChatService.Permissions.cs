using System.Text.Json;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    /// <summary>Checks the same prerequisite in the UI and service before any file is read or uploaded.</summary>
    internal bool CanUpload(UploadDestination Destination) => UserId is not null && Destination switch
    {
        UploadDestination.Original or UploadDestination.AvatarGallery => true,

        UploadDestination.Gallery => PermissionsKnown && GalleryUploadAllowed,

        UploadDestination.UserIcon => PermissionsKnown && UserIconUploadAllowed,

        _ => false
    };

    /// <summary>Separates assigned website-image permissions from the reusable original-file workflow.</summary>
    private async Task ReadUploadPermissionsAsync(CancellationToken Token)
    {
        PermissionsKnown = false;

        GalleryUploadAllowed = false;

        UserIconUploadAllowed = false;

        try
        {
            using HttpResponseMessage Response = await Client.GetAsync("https://api.vrchat.cloud/api/1/auth/permissions", HttpCompletionOption.ResponseHeadersRead, Token);

            if (!Response.IsSuccessStatusCode)
            {
                await UploadLog.WriteAsync("Check permissions", Status: (int)Response.StatusCode, Explanation: await ReadServerExplanationAsync(Response, Token));

                return;
            }

            using PooledBuffer Buffer = new(65537);

            await using Stream Source = await Response.Content.ReadAsStreamAsync(Token);

            int Count = await Source.ReadAtLeastAsync(Buffer.Memory, Buffer.Memory.Length, false, Token);

            if (Count > 65536)
            {
                throw new InvalidDataException("The permission response exceeds its supported length.");
            }

            using JsonDocument Document = JsonDocument.Parse(Buffer.Memory[..Count]);

            if (Document.RootElement.ValueKind is not JsonValueKind.Array)
            {
                return;
            }

            foreach (JsonElement Item in Document.RootElement.EnumerateArray())
            {
                if (Item.ValueKind is not JsonValueKind.Object || !Item.TryGetProperty("name", out JsonElement Name) || Name.ValueKind is not JsonValueKind.String)
                {
                    continue;
                }

                GalleryUploadAllowed |= Name.ValueEquals("permission-user-gallery");

                UserIconUploadAllowed |= Name.ValueEquals("permission-user-icons");
            }

            PermissionsKnown = true;
        }
        catch (Exception Error) when (Error is HttpRequestException or IOException or JsonException)
        {
            // A failed optional entitlement lookup must not pretend that access was granted.
            await UploadLog.WriteAsync("Check permissions", Error.GetType().Name);
        }
    }
}
