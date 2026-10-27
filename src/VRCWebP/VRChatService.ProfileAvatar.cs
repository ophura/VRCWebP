using System.Text.Json;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    /// <summary>Refreshes cached image pointers after checking that the requested avatar remains selected.</summary>
    private async Task RefreshAvatarProfileReferenceAsync(string ExpectedAvatarId, CancellationToken Token)
    {
        using CancellationTokenSource Deadline = CancellationTokenSource.CreateLinkedTokenSource(Token);

        Deadline.CancelAfter(TimeSpan.FromSeconds(30));

        Token = Deadline.Token;

        using JsonDocument User = Read(await Authentication.GetCurrentUserWithHttpInfoAsync(Token));

        if (!IsAvatarId(ExpectedAvatarId) || Text(User.RootElement, "id") != UserId || Text(User.RootElement, "currentAvatar") != ExpectedAvatarId)
        {
            return;
        }

        using JsonDocument Avatar = Read(await Avatars.GetAvatarWithHttpInfoAsync(ExpectedAvatarId, Token));

        if (Text(Avatar.RootElement, "id") != ExpectedAvatarId)
        {
            throw new InvalidDataException("VRChat returned a different avatar record during profile refresh.");
        }

        string Image = Text(Avatar.RootElement, "imageUrl");

        string Thumbnail = Text(Avatar.RootElement, "thumbnailImageUrl");

        if (SameProfileImageReference(Text(User.RootElement, "currentAvatarImageUrl"), Image) && SameProfileImageReference(Text(User.RootElement, "currentAvatarThumbnailImageUrl"), Thumbnail))
        {
            return;
        }

        if (!SameProfileImageReference(Image, Image) || !SameProfileImageReference(Thumbnail, Thumbnail))
        {
            throw new InvalidDataException("The current avatar has no verifiable image reference.");
        }

        using JsonDocument Latest = Read(await Authentication.GetCurrentUserWithHttpInfoAsync(Token));

        if (Text(Latest.RootElement, "id") != UserId || Text(Latest.RootElement, "currentAvatar") != ExpectedAvatarId)
        {
            throw new InvalidOperationException("Your current avatar changed during the image update. Its selection was left unchanged.");
        }

        using JsonDocument Selected = Read(await Avatars.SelectAvatarWithHttpInfoAsync(ExpectedAvatarId, Token));

        if (Text(Selected.RootElement, "currentAvatar") != ExpectedAvatarId
            || !SameProfileImageReference(Text(Selected.RootElement, "currentAvatarImageUrl"), Image)
            || !SameProfileImageReference(Text(Selected.RootElement, "currentAvatarThumbnailImageUrl"), Thumbnail))
        {
            throw new InvalidOperationException("VRChat did not confirm the refreshed current-avatar thumbnail reference.");
        }

        await UploadLog.WriteAsync("Refresh current avatar thumbnail", Status: 200);
    }

    /// <summary>Compares file identity and version across original image and derived thumbnail URLs.</summary>
    private static bool SameProfileImageReference(string Actual, string Expected)
    {
        if (!Uri.TryCreate(Expected, UriKind.Absolute, out Uri? Address) || Address.Scheme is not "https"
            || Address.Host is not ("api.vrchat.cloud" or "api.vrchat.com") || Address.UserInfo.Length is not 0 || !Address.IsDefaultPort
            || Address.Query.Length is not 0 || Address.Fragment.Length is not 0)
        {
            return false;
        }

        ReadOnlySpan<char> Path = Address.AbsolutePath.AsSpan();

        Span<Range> Parts = stackalloc Range[7];

        int Count = Path.Split(Parts, '/', StringSplitOptions.RemoveEmptyEntries);

        if (Count is not (5 or 6) || !Path[Parts[0]].SequenceEqual("api") || !Path[Parts[1]].SequenceEqual("1")
            || !(Path[Parts[2]].SequenceEqual("file") || (Count is 6 && Path[Parts[2]].SequenceEqual("image")))
            || !int.TryParse(Path[Parts[4]], System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out int Version)
            || Version < 1)
        {
            return false;
        }

        string Id = Path[Parts[3]].ToString();

        return Id.Length is 41 && Id.StartsWith("file_", StringComparison.Ordinal) && Guid.TryParseExact(Id.AsSpan(5), "D", out _)
            && ProfileImageMatches(Actual, Id, Version);
    }

    /// <summary>Checks the resolved URL and a small image signature without buffering or decoding the full animation.</summary>
    private async Task VerifyProfileImageAvailableAsync(string Url, CancellationToken Token)
    {
        if (!SameProfileImageReference(Url, Url))
        {
            throw new InvalidDataException("Profile settings saved, but VRChat returned an invalid image URL.");
        }

        using CancellationTokenSource Deadline = CancellationTokenSource.CreateLinkedTokenSource(Token);

        Deadline.CancelAfter(TimeSpan.FromSeconds(15));

        using HttpRequestMessage Request = new(HttpMethod.Get, Url);

        Request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(0, 31);

        using HttpResponseMessage Response = await Client.SendAsync(Request, HttpCompletionOption.ResponseHeadersRead, Deadline.Token);

        if (!Response.IsSuccessStatusCode)
        {
            string Explanation = await ReadServerExplanationAsync(Response, Deadline.Token);

            await UploadLog.WriteAsync("Verify displayed profile image", Status: (int)Response.StatusCode, Explanation: Explanation);

            throw new ServerRequestException($"Profile settings saved, but its image cannot be loaded (HTTP {(int)Response.StatusCode}): {Explanation}.", Response.StatusCode, Explanation);
        }

        using PooledBuffer Buffer = new(32);

        await using Stream Body = await Response.Content.ReadAsStreamAsync(Deadline.Token);

        int Count = await Body.ReadAtLeastAsync(Buffer.Memory[..32], 32, false, Deadline.Token);

        ReadOnlySpan<byte> Header = Buffer.Memory.Span[..Count];

        bool Image = (Header.Length >= 12 && Header.StartsWith("RIFF"u8) && Header[8..].StartsWith("WEBP"u8))
            || (Header.Length >= 8 && Header[0] is 137 && Header[1..].StartsWith("PNG\r\n\x1a\n"u8))
            || (Header.Length >= 3 && Header[0] is 255 && Header[1] is 216 && Header[2] is 255)
            || Header.StartsWith("GIF87a"u8) || Header.StartsWith("GIF89a"u8);

        if (!Image)
        {
            await UploadLog.WriteAsync("Verify displayed profile image", "InvalidImageSignature", (int)Response.StatusCode);

            throw new InvalidDataException("Profile settings saved, but its image URL did not return a supported image.");
        }

        await UploadLog.WriteAsync("Verify displayed profile image", Status: (int)Response.StatusCode);
    }
}
