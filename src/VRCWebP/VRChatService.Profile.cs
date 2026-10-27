using System.Text.Json;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    internal const string ForeignProfileImageExplanation = "Use an image uploaded by the connected account for its profile.";

    internal const string ProfileIconFileExplanation = "Profile icons require an image from the User icon inventory. This original file cannot be used as an icon. Choose an existing icon, or upload to User icon when your account permits it.";

    internal const string ProfileBannerSubscriptionExplanation = "Custom profile banners require VRC+. The connected account does not have VRC+; its avatar-linked banner can still follow its avatar thumbnail.";

    internal bool ProfileAccessKnown { get; private set; }

    internal bool HasVrcPlus { get; private set; }

    /// <summary>Checks the actual record purpose separately from access to a custom profile banner.</summary>
    internal string? ProfileImageUnavailableReason(UploadResult? Image, string Purpose)
    {
        if (Purpose is "Profile icon from avatar" or "Profile banner from avatar")
        {
            return null;
        }

        if (Image?.OwnerId is string Owner && Owner != UserId)
        {
            return ForeignProfileImageExplanation;
        }

        if (Purpose is "Profile icon" && Image is { ProfileIconEligible: false })
        {
            return ProfileIconFileExplanation;
        }

        if (Purpose is "Profile banner")
        {
            return !ProfileAccessKnown ? "Profile access could not be verified. Reconnect your browser before setting a custom banner."
                : !HasVrcPlus ? ProfileBannerSubscriptionExplanation : null;
        }

        return null;
    }

    private static bool HasImageTag(JsonElement Image, string Tag) => Image.TryGetProperty("tags", out JsonElement Tags)
        && Tags.ValueKind is JsonValueKind.Array && Tags.EnumerateArray().Any(Value => Value.ValueKind is JsonValueKind.String && Value.ValueEquals(Tag));

    /// <summary>Reads profile entitlement once per connection without retaining unrelated profile fields.</summary>
    private async Task ReadProfileAccessAsync(CancellationToken Token)
    {
        ProfileAccessKnown = false;

        HasVrcPlus = false;

        using CancellationTokenSource Timeout = CancellationTokenSource.CreateLinkedTokenSource(Token);

        Timeout.CancelAfter(TimeSpan.FromSeconds(10));

        try
        {
            using HttpResponseMessage Response = await Client.GetAsync("https://api.vrchat.cloud/api/1/profile/" + UserId + "?asSelf=true",
                HttpCompletionOption.ResponseHeadersRead, Timeout.Token);

            if (!Response.IsSuccessStatusCode)
            {
                await UploadLog.WriteAsync("Check profile access", Status: (int)Response.StatusCode,
                    Explanation: await ReadServerExplanationAsync(Response, Timeout.Token));

                return;
            }

            using PooledBuffer Buffer = new(262145);

            await using Stream Source = await Response.Content.ReadAsStreamAsync(Timeout.Token);

            int Count = await Source.ReadAtLeastAsync(Buffer.Memory, Buffer.Memory.Length, false, Timeout.Token);

            if (Count > 262144)
            {
                throw new InvalidDataException("The profile response exceeds its supported length.");
            }

            using JsonDocument Profile = JsonDocument.Parse(Buffer.Memory[..Count]);

            if (Profile.RootElement.ValueKind is JsonValueKind.Object
                && Profile.RootElement.TryGetProperty("hasVrcPlus", out JsonElement Access) && Access.ValueKind is JsonValueKind.True or JsonValueKind.False)
            {
                HasVrcPlus = Access.GetBoolean();

                ProfileAccessKnown = true;
            }
        }
        catch (Exception Error) when (Error is HttpRequestException or IOException or JsonException
            || (Error is OperationCanceledException && !Token.IsCancellationRequested))
        {
            await UploadLog.WriteAsync("Check profile access", Error.GetType().Name);
        }
    }

    /// <summary>Updates only the selected image on the connected account, preserving every unrelated profile field.</summary>
    private async Task<string> ApplyProfileImageAsync(UploadResult? Image, string Purpose, CancellationToken Token)
    {
        if (ProfileImageUnavailableReason(Image, Purpose) is string Reason)
        {
            throw new InvalidOperationException(Reason);
        }

        using CancellationTokenSource Deadline = CancellationTokenSource.CreateLinkedTokenSource(Token);

        Deadline.CancelAfter(TimeSpan.FromSeconds(60));

        Token = Deadline.Token;

        bool AvatarIcon = Purpose is "Profile icon from avatar";

        bool AvatarBanner = Purpose is "Profile banner from avatar";

        bool FromAvatar = AvatarIcon || AvatarBanner;

        if (!FromAvatar)
        {
            ArgumentNullException.ThrowIfNull(Image);

            (string Id, int? Version) = ParseImageReference(Image.Url);

            if (Id != Image.FileId || Version != Image.Version)
            {
                throw new InvalidDataException("The selected image link does not match its verified file version.");
            }
        }

        using MemoryStream Body = new();

        using (Utf8JsonWriter Writer = new(Body))
        {
            Writer.WriteStartObject();

            if (AvatarIcon)
            {
                Writer.WriteString("userIcon", "");
            }
            else if (AvatarBanner)
            {
                Writer.WriteString("bannerType", "avatarBanner");
            }
            else if (Purpose is "Profile icon")
            {
                Writer.WriteString("userIcon", Image!.Url);
            }
            else
            {
                Writer.WriteString("bannerCustomUrl", Image!.Url);

                Writer.WriteString("bannerType", "customImage");
            }

            Writer.WriteEndObject();
        }

        using HttpRequestMessage Request = new(HttpMethod.Put, "https://api.vrchat.cloud/api/1/profile/" + UserId)
        {
            Content = new ByteArrayContent(Body.GetBuffer(), 0, (int)Body.Length)
        };

        Request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        using (HttpResponseMessage Response = await Client.SendAsync(Request, HttpCompletionOption.ResponseHeadersRead, Token))
        {
            if (!Response.IsSuccessStatusCode)
            {
                string Explanation = await ReadServerExplanationAsync(Response, Token);

                throw new ServerRequestException($"Profile image update failed (HTTP {(int)Response.StatusCode}): {Explanation}.", Response.StatusCode, Explanation);
            }
        }

        await VerifyProfileImageAsync(Image, Purpose, true, Token);

        await UploadLog.WriteAsync("Apply " + Purpose.ToLowerInvariant(), Status: 200);

        return Purpose + " updated; image URL verified.";
    }

    /// <summary>Reads the bounded self profile; selected modes alone do not establish that its resolved image exists.</summary>
    private async Task<JsonDocument> ReadOwnProfileAsync(CancellationToken Token)
    {
        using HttpResponseMessage Response = await Client.GetAsync("https://api.vrchat.cloud/api/1/profile/" + UserId + "?asSelf=true",
            HttpCompletionOption.ResponseHeadersRead, Token);

        if (!Response.IsSuccessStatusCode)
        {
            string Explanation = await ReadServerExplanationAsync(Response, Token);

            throw new ServerRequestException($"Profile verification failed (HTTP {(int)Response.StatusCode}): {Explanation}.", Response.StatusCode, Explanation);
        }

        using PooledBuffer Buffer = new(262145);

        await using Stream Source = await Response.Content.ReadAsStreamAsync(Token);

        int Count = await Source.ReadAtLeastAsync(Buffer.Memory, Buffer.Memory.Length, false, Token);

        if (Count > 262144)
        {
            throw new InvalidDataException("The profile response exceeds its supported length.");
        }

        Utf8JsonReader Reader = new(Buffer.Memory.Span[..Count]);

        return JsonDocument.ParseValue(ref Reader);
    }

    /// <summary>Confirms effective image references, repairing a stale current-avatar snapshot at most once.</summary>
    private async Task VerifyProfileImageAsync(UploadResult? Image, string Purpose, bool AllowRefresh, CancellationToken Token)
    {
        using JsonDocument Profile = await ReadOwnProfileAsync(Token);

        JsonElement Root = Profile.RootElement;

        bool AvatarIcon = Purpose is "Profile icon from avatar";

        bool AvatarBanner = Purpose is "Profile banner from avatar";

        bool Icon = Purpose is "Profile icon" || AvatarIcon;

        string Actual = Text(Root, Icon ? "iconUrl" : "bannerUrl");

        bool ModeMatches = AvatarIcon ? Text(Root, "iconType") is "avatarThumbnail" && Text(Root, "userIcon").Length is 0
            : AvatarBanner ? Text(Root, "bannerType") is "avatarBanner"
            : ProfileImageMatches(Text(Root, Icon ? "userIcon" : "bannerCustomUrl"), Image)
                && (Icon || Text(Root, "bannerType") is "customImage");

        if (!ModeMatches)
        {
            throw new InvalidOperationException("Update accepted; the selected profile image is not confirmed yet. Refresh your profile before trying again.");
        }

        if (AvatarIcon || AvatarBanner)
        {
            string Expected = Text(Root, Icon ? "currentAvatarThumbnailImageUrl" : "currentAvatarImageUrl");

            if (!SameProfileImageReference(Actual, Expected))
            {
                string AvatarId = Text(Root, "currentAvatar");

                if (AllowRefresh && IsAvatarId(AvatarId))
                {
                    await RefreshAvatarProfileReferenceAsync(AvatarId, Token);

                    await VerifyProfileImageAsync(Image, Purpose, false, Token);

                    return;
                }

                throw new InvalidOperationException("Profile mode saved, but VRChat still resolves an outdated avatar thumbnail. Reselect your current avatar and try again.");
            }
        }
        else if (!ProfileImageMatches(Actual, Image))
        {
            throw new InvalidOperationException("Profile setting saved, but its displayed image does not match the selected file yet.");
        }

        await VerifyProfileImageAvailableAsync(Actual, Token);
    }

    /// <summary>Accepts the website's version link, original-file link or derived thumbnail without confusing adjacent version numbers.</summary>
    private static bool ProfileImageMatches(string Value, UploadResult? Image) => Image is not null && ProfileImageMatches(Value, Image.FileId, Image.Version);

    private static bool ProfileImageMatches(string Value, string FileId, int VersionId)
    {
        if (!Uri.TryCreate(Value, UriKind.Absolute, out Uri? Address) || Address.Scheme is not "https"
            || Address.Host is not ("api.vrchat.cloud" or "api.vrchat.com") || Address.UserInfo.Length is not 0 || !Address.IsDefaultPort
            || Address.Query.Length is not 0 || Address.Fragment.Length is not 0)
        {
            return false;
        }

        string Version = VersionId.ToString(System.Globalization.CultureInfo.InvariantCulture);

        string FilePath = $"/api/1/file/{FileId}/{Version}";

        return Address.AbsolutePath == FilePath || Address.AbsolutePath == FilePath + "/file" || Address.AbsolutePath.StartsWith($"/api/1/image/{FileId}/{Version}/", StringComparison.Ordinal);
    }
}
