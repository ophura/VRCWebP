using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using VRChat.API.Api;
using VRChat.API.Client;
using VRChat.API.Model;

namespace VRCWebP;

/// <summary>Owns an in-memory authenticated session and the reusable versioned-file upload workflow.</summary>
internal sealed partial class VRChatService : IDisposable
{
    private readonly HttpClientHandler Handler = new()
    {
        CookieContainer = new CookieContainer(),

        AllowAutoRedirect = true
    };

    private readonly HttpClient Client;

    private readonly HttpClient Storage;

    private readonly ApiTrafficHandler Traffic;

    private UploadCheckpoint? Checkpoint;

    private IProgress<WorkProgress>? ActiveUploadProgress;

    private double UploadedPercent;

    internal const long MultipartPartBytes = 64L * 1024 * 1024;


    private string UploadStage = "Preparation";

    private readonly Configuration Configuration = new();

    private readonly AuthenticationApi Authentication;

    private readonly FilesApi Files;

    private readonly GroupsApi Groups;

    private readonly AvatarsApi Avatars;

    internal string? UserId { get; private set; }

    internal string? DisplayName { get; private set; }

    internal string? PendingFileId { get; private set; }

    /// <summary>True only when the displayed recovery record belongs to this account's original-upload checkpoint.</summary>
    internal bool CanResumeUpload => Checkpoint is not null && Checkpoint.UserId == UserId && PendingFileId == Checkpoint.FileId;

    internal bool PermissionsKnown { get; private set; }

    internal bool GalleryUploadAllowed { get; private set; }

    internal bool UserIconUploadAllowed { get; private set; }

    internal VRChatService()
    {
        PreviewCache =
            new ImagePreviewCache();

        Traffic = new ApiTrafficHandler(
            Handler
            );


        Traffic.Paused = Delay => ActiveUploadProgress?.Report(new WorkProgress(UploadedPercent,
            $"Rate limited · resumes in {Math.Ceiling(Delay.TotalSeconds):0} s"));

        Client = new HttpClient(Traffic)
        {
            Timeout = Timeout.InfiniteTimeSpan
        };

        Storage = new HttpClient(
            new HttpClientHandler
            {
                UseCookies = false,

                AllowAutoRedirect = false
            })
        {
            Timeout = TimeSpan.FromMinutes(10)
        };

        Authentication = new AuthenticationApi(Client, Configuration, Handler);

        Files = new FilesApi(Client, Configuration, Handler);

        Groups = new GroupsApi(Client, Configuration, Handler);

        Avatars = new AvatarsApi(Client, Configuration, Handler);
    }

    /// <summary>Accepts only an explicitly shared browser session and verifies its identity with VRChat.</summary>
    internal async Task ConnectAsync(BrowserSession Session, CancellationToken CancellationToken)
    {
        if (Session.Expires <= DateTimeOffset.UtcNow || string.IsNullOrWhiteSpace(Session.Auth))
        {
            throw new InvalidOperationException("The shared browser session has expired. Connect your browser again.");
        }

        Handler.CookieContainer.Add(new Cookie("auth", Session.Auth, "/", "api.vrchat.cloud")
        {
            Secure = true,

            HttpOnly = true
        });

        if (!string.IsNullOrEmpty(Session.TwoFactorAuth))
        {
            Handler.CookieContainer.Add(new Cookie("twoFactorAuth", Session.TwoFactorAuth, "/", "api.vrchat.cloud")
            {
                Secure = true,

                HttpOnly = true
            });
        }
        else
        {
            Handler.CookieContainer.Add(new Cookie("twoFactorAuth", "", "/", "api.vrchat.cloud") { Expired = true });
        }

        SetUserAgent("VRCWebP/1.1.11 (+https://github.com/ophura)");

        using JsonDocument Response = Read(await Authentication.GetCurrentUserWithHttpInfoAsync(CancellationToken));

        if (Response.RootElement.TryGetProperty("requiresTwoFactorAuth", out _))
        {
            throw new InvalidOperationException("Complete VRChat sign-in in your browser, then connect it again.");
        }

        AcceptUser(Response.RootElement);

        await ReadUploadPermissionsAsync(CancellationToken);

        await ReadProfileAccessAsync(CancellationToken);
    }

    /// <summary>Uploads an explicitly selected animation to an allowed original-file or inventory destination.</summary>
    internal async Task<UploadResult> UploadAsync(ConversionResult Media, string Name, IProgress<WorkProgress> Progress, CancellationToken CancellationToken,
        UploadDestination Destination = UploadDestination.Original, string? GalleryAvatar = null)
    {
        UploadStage = "Preparation";

        long? ActualBytes = null;

        ActiveUploadProgress = Progress;

        UploadedPercent = 0;

        try
        {
            if (!CanUpload(Destination))
            {
                throw new InvalidOperationException("This session does not permit the selected upload destination. Connect a session with the required upload permission.");
            }

            await UploadStepAsync("Check upload size");

            await using FileStream Source = new(Media.Path, FileMode.Open, FileAccess.Read, FileShare.Read, 65536, FileOptions.Asynchronous | FileOptions.SequentialScan);

            ActualBytes = Source.Length;

            {
                UploadPolicy.Validate(ActualBytes.Value, Destination);
            }

            AvatarGalleryState? Gallery = null;

            if (Destination is UploadDestination.AvatarGallery)
            {
                await UploadStepAsync("Check avatar gallery");

                Progress.Report(new WorkProgress(0, "Checking avatar gallery"));

                Gallery = await ReadAvatarGalleryAsync(GalleryAvatar ?? "", CancellationToken);
            }

            if (Gallery?.FileIds.Count >= 6)
            {
                throw new InvalidOperationException("This avatar gallery already has six images. Manage its existing files before adding another.");
            }

            UploadResult Result = Destination is UploadDestination.Original
                ? await UploadCoreAsync(Media, Name, Source, Progress, CancellationToken)
                : await UploadImageAsync(Media, Destination, Source, Progress, CancellationToken, Gallery);

            await UploadLog.WriteAsync("Complete");

            await RememberLocalPreviewAsync(Result, Media.Path, CancellationToken);

            return Result;
        }
        catch (OperationCanceledException)
        {
            await UploadLog.WriteAsync(UploadStage, "Cancelled");

            throw;
        }
        catch (Exception Error)
        {
            int? Status = Error switch
            {
                HttpRequestException Http => (int?)Http.StatusCode,

                ApiException Api => Api.ErrorCode,

                _ => null
            };

            await UploadLog.WriteAsync(UploadStage, Error.GetType().Name, Status,
                Error is ServerRequestException Server ? Server.SafeExplanation : null, ActualBytes ?? Media.Bytes);

            throw new IOException($"Upload failed at {UploadStage}: {Error.Message}", Error);
        }
        finally
        {
            ActiveUploadProgress = null;
        }
    }

    /// <summary>Streams the original and signature through the server-selected upload categories.</summary>
    private async Task<UploadResult> UploadCoreAsync(ConversionResult Media, string Name, FileStream Source, IProgress<WorkProgress> Progress, CancellationToken CancellationToken)
    {
        RequireLogin();

        Progress.Report(new WorkProgress(0, "Preparing file"));

        byte[] SignatureBytes = await Task.Run(() => Signature.Create(Media.Path, CancellationToken), CancellationToken);

        string FileMd5 = Convert.ToBase64String(await MD5.HashDataAsync(Source, CancellationToken));

        string SignatureMd5 = Convert.ToBase64String(MD5.HashData(SignatureBytes));

        using JsonDocument Version = await PrepareUploadAsync(Name, Source.Length, FileMd5, SignatureBytes.Length, SignatureMd5, CancellationToken);

        UploadCheckpoint Upload = Checkpoint!;

        string FileId = Upload.FileId;

        int VersionNumber = Upload.Version;

        JsonElement Entry = Version.RootElement.GetProperty("versions").EnumerateArray().Single(Item => Item.GetProperty("version").GetInt32() == VersionNumber);

        long Total = Source.Length + SignatureBytes.Length;

        Source.Position = 0;

        await UploadPartAsync(FileId, VersionNumber, "file", Source, Entry.GetProperty("file"), FileMd5, VRChatImageRecord.MimeType, 0, Total, Upload.File, Progress, CancellationToken);

        using MemoryStream SignatureStream = new(SignatureBytes);

        await UploadPartAsync(FileId, VersionNumber, "signature", SignatureStream, Entry.GetProperty("signature"), SignatureMd5, "application/octet-stream", Source.Length, Total, Upload.Signature, Progress, CancellationToken);

        Progress.Report(new WorkProgress(100, "Server processing"));

        await UploadStepAsync("Server processing");

        for (int Attempt = 0; Attempt < 8; ++Attempt)
        {
            using JsonDocument State = Read(await Files.GetFileWithHttpInfoAsync(FileId, CancellationToken));

            JsonElement Current = State.RootElement.GetProperty("versions").EnumerateArray().Last(Item => Item.GetProperty("version").GetInt32() == VersionNumber);

            string Status = Text(Current, "status");

            string FileStatus = Text(Current.GetProperty("file"), "status");

            string SignatureStatus = Text(Current.GetProperty("signature"), "status");

            if (Status is "error" || FileStatus is "error" || SignatureStatus is "error")
            {
                throw new InvalidOperationException("VRChat rejected the uploaded file during processing. File ID: " + FileId);
            }

            if (Status is "complete" && FileStatus is "complete" && SignatureStatus is "complete")
            {
                string Url = "https://api.vrchat.cloud/api/1/file/" + FileId + "/" + VersionNumber + "/file";

                Progress.Report(new WorkProgress(100, "Verifying stored image"));

                await UploadStepAsync("Verify stored file");

                await VerifyStoredFileAsync(Source, Url, CancellationToken);

                PendingFileId = null;

                Checkpoint = null;

                UploadResult Result = new(FileId, Url, VersionNumber, "Uploaded: original verified", OwnerId: UserId);


                return Result;
            }

            await Task.Delay(TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, Attempt + 1))), CancellationToken);
        }

        throw new TimeoutException("The upload is stored but processing has not completed. File ID: " + FileId);
    }

    /// <summary>Applies an already uploaded file only when the user chooses a target and presses Apply.</summary>
    internal async Task<string> ApplyAsync(UploadResult? Upload, string Purpose, string TargetId, CancellationToken CancellationToken)
    {
        try
        {
            return await ApplyImageCoreAsync(Upload, Purpose, TargetId, CancellationToken);
        }
        catch (Exception Error) when (Error is not OperationCanceledException)
        {
            await LogImageFailureAsync("Apply image", Error);

            throw;
        }
    }

    /// <summary>Checks destination compatibility and ownership before changing an explicitly selected image target.</summary>
    private async Task<string> ApplyImageCoreAsync(UploadResult? Upload, string Purpose, string TargetId, CancellationToken CancellationToken)
    {
        RequireLogin();

        if (Purpose is not ("Group banner" or "Group icon" or "Avatar thumbnail" or "Profile icon" or "Profile banner"
            or "Profile icon from avatar" or "Profile banner from avatar"))
        {
            throw new ArgumentException("Choose a supported image destination.");
        }

        if (Purpose is "Profile icon" or "Profile banner" or "Profile icon from avatar" or "Profile banner from avatar")
        {
            return await ApplyProfileImageAsync(Upload, Purpose, CancellationToken);
        }

        ArgumentNullException.ThrowIfNull(Upload);

        if (Purpose is "Avatar thumbnail" && AvatarThumbnailUnavailableReason(Upload) is string Reason)
        {
            throw new InvalidOperationException(Reason);
        }

        string Prefix = Purpose is "Avatar thumbnail" ? "avtr_" : "grp_";

        if (!IsTargetId(TargetId, Prefix))
        {
            throw new ArgumentException("Enter a valid " + Prefix + " ID.");
        }

        if (Purpose is "Avatar thumbnail")
        {
            using JsonDocument Existing = Read(await Avatars.GetAvatarWithHttpInfoAsync(TargetId, CancellationToken));

            if (Text(Existing.RootElement, "authorId") != UserId)
            {
                throw new InvalidOperationException("You can only change images on your own avatars.");
            }

            using JsonDocument Updated = Read(await Avatars.UpdateAvatarWithHttpInfoAsync(TargetId, new UpdateAvatarRequest(imageUrl: Upload.Url), CancellationToken));

            using JsonDocument Confirmed = Read(await Avatars.GetAvatarWithHttpInfoAsync(TargetId, CancellationToken));

            if (!ProfileImageMatches(Text(Confirmed.RootElement, "imageUrl"), Upload))
            {
                throw new InvalidOperationException("Update accepted; the avatar thumbnail is not confirmed yet.");
            }

            try
            {
                await RefreshAvatarProfileReferenceAsync(TargetId, CancellationToken);
            }
            catch (Exception Error) when (Error is not OperationCanceledException || !CancellationToken.IsCancellationRequested)
            {
                await LogImageFailureAsync("Refresh current avatar thumbnail", Error);

                return "Avatar thumbnail updated; its profile reference could not be refreshed. Select Profile icon/banner from avatar to retry.";
            }

            return "Applied: " + Text(Confirmed.RootElement, "name");
        }

        UpdateGroupRequest Request = Purpose is "Group icon" ? new UpdateGroupRequest(iconId: Upload.FileId) : new UpdateGroupRequest(bannerId: Upload.FileId);

        using JsonDocument Group = Read(await Groups.UpdateGroupWithHttpInfoAsync(TargetId, Request, CancellationToken));

        string Property = Purpose is "Group icon" ? "iconId" : "bannerId";

        using JsonDocument Verification = Read(await Groups.GetGroupWithHttpInfoAsync(TargetId, cancellationToken: CancellationToken));

        if (Text(Verification.RootElement, Property) != Upload.FileId)
        {
            throw new InvalidOperationException("Update accepted; the group image is not confirmed yet.");
        }

        return "Applied: " + Text(Verification.RootElement, "name");
    }

    /// <summary>Supports simple and multipart uploads without forwarding VRChat cookies to storage.</summary>
    private async Task UploadPartAsync(string FileId, int Version, string Type, Stream Source, JsonElement Metadata, string Md5, string Mime, long Offset, long Total, DescriptorCheckpoint Saved, IProgress<WorkProgress> Progress, CancellationToken CancellationToken)
    {
        if (Text(Metadata, "status") is "complete")
        {
            Saved.Finished = true;

            return;
        }

        bool Multipart = Text(Metadata, "category") is "multipart";

        string UploadId = Text(Metadata, "uploadId");

        if (Saved.UploadId is not null && Saved.UploadId != UploadId)
        {
            throw new InvalidDataException("The server's multipart session changed. Its recovery ID has been retained.");
        }

        Saved.UploadId = UploadId;

        long Sent = Saved.Sent;

        Source.Position = Sent;

        UploadedPercent = (Offset + Sent) * 100d / Total;

        int Failures = 0;

        while (Sent < Source.Length)
        {
            await UploadStepAsync("Start " + Type);

            using JsonDocument Started = Read(await Files.StartFileDataUploadWithHttpInfoAsync(FileId, Version, Type, Multipart ? Saved.Etags.Count + 1 : null, CancellationToken));

            Uri SignedUrl = new(Started.RootElement.GetProperty("url").GetString() ?? "");

            if (SignedUrl.Scheme is not "https")
            {
                throw new InvalidDataException("VRChat returned an insecure storage URL.");
            }

            bool ImageProxy = IsImageUploadProxy(SignedUrl);

            long Count = Multipart ? Math.Min(MultipartPartBytes, Source.Length - Sent) : Source.Length;

            if (ImageProxy && Source.Length > UploadPolicy.OriginalImageMaximumBytes)
            {
                throw new InvalidDataException("The PNG-compatible image proxy does not support this multipart image. Reduce the output to 10 MB or less.");
            }

            long PartStart = Sent;

            Source.Position = Sent;

            using UploadContent Content = new(Source, Count, Mime, Bytes =>
            {
                UploadedPercent = (Offset + PartStart + Bytes) * 100d / Total;

                Progress.Report(new WorkProgress(UploadedPercent, Type is "file" ? "Uploading image" : "Uploading signature"));
            });

            if (!Multipart)
            {
                Content.Headers.ContentMD5 = Convert.FromBase64String(Md5);
            }
            else if (!ImageProxy)
            {
                Content.Headers.ContentType = null;
            }

            await UploadStepAsync("Transfer " + Type);

            using HttpRequestMessage Request = new(HttpMethod.Put, SignedUrl) { Content = Content };

            HttpResponseMessage Response;

            long TransferStarted = System.Diagnostics.Stopwatch.GetTimestamp();

            try
            {
                Response = await Storage.SendAsync(Request, HttpCompletionOption.ResponseHeadersRead, CancellationToken);
            }
            catch (HttpRequestException) when (Failures++ < 3)
            {
                UploadedPercent = (Offset + Sent) * 100d / Total;

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, Failures)), CancellationToken);

                continue;
            }

            using IDisposable ReleaseResponse = Response;

            await UploadLog.WriteAsync("Storage request", Status: (int)Response.StatusCode, Method: "PUT",
                Route: ImageProxy ? "/file/image/code/{uploadCode}" : "/storage/{part}", FileBytes: Count,
                Part: Multipart ? Saved.Etags.Count + 1 : null, Attempt: Failures + 1,
                ElapsedMilliseconds: System.Diagnostics.Stopwatch.GetElapsedTime(TransferStarted).TotalMilliseconds);

            if (Response.StatusCode is HttpStatusCode.TooManyRequests)
            {
                UploadedPercent = (Offset + Sent) * 100d / Total;

                Traffic.ObserveCooldown(Response, Failures);

                await UploadLog.WriteAsync("Storage cooldown", Status: 429, Explanation: "Rate limit exceeded", Part: Saved.Etags.Count + 1,
                    ServerRetryAfterSeconds: Response.Headers.RetryAfter?.Delta?.TotalSeconds, ServerRetryAt: Response.Headers.RetryAfter?.Date);

                if (Failures++ < 6)
                {
                    continue;
                }
            }

            if (!Response.IsSuccessStatusCode)
            {
                string Explanation = await ReadServerExplanationAsync(Response, CancellationToken);

                throw new ServerRequestException($"Storage upload failed (HTTP {(int)Response.StatusCode}): {Explanation}. File ID: {FileId}", Response.StatusCode, Explanation);
            }

            if (Multipart)
            {
                Saved.Etags.Add(Response.Headers.ETag?.Tag.Trim('"') ?? throw new InvalidDataException("Storage returned no multipart ETag."));
            }

            Sent += Count;

            Saved.Sent = Sent;

            Failures = 0;
        }

        await FinishDescriptorAsync(FileId, Version, Type, Saved, Multipart, CancellationToken);
    }

    /// <summary>Recognizes the credential-free image proxy without retaining or logging its short-lived upload code.</summary>
    private static bool IsImageUploadProxy(Uri Address)
    {
        const string Prefix = "/api/1/file/image/code/";

        if (Address.Host is not ("api.vrchat.cloud" or "api.vrchat.com" or "vrchat.com"))
        {
            return false;
        }

        if (!Address.IsDefaultPort || Address.UserInfo.Length is not 0 || Address.Query.Length is not 0 || Address.Fragment.Length is not 0
            || !Address.AbsolutePath.StartsWith(Prefix, StringComparison.Ordinal)
            || !Guid.TryParseExact(Address.AbsolutePath.AsSpan(Prefix.Length), "D", out _))
        {
            throw new InvalidDataException("VRChat returned an unsupported image upload endpoint. Its recovery ID has been retained.");
        }

        return true;
    }

    /// <summary>Records the next phase before it can fail, without persisting request or response data.</summary>
    private Task UploadStepAsync(string Stage)
    {
        UploadStage = Stage;

        return UploadLog.WriteAsync(Stage);
    }

    private void SetUserAgent(string Value)
    {
        ((Configuration)Authentication.Configuration).UserAgent = Value;

        ((Configuration)Files.Configuration).UserAgent = Value;

        ((Configuration)Groups.Configuration).UserAgent = Value;

        ((Configuration)Avatars.Configuration).UserAgent = Value;

        Client.DefaultRequestHeaders.UserAgent.Clear();

        Client.DefaultRequestHeaders.UserAgent.ParseAdd(Value);

        Storage.DefaultRequestHeaders.UserAgent.Clear();

        Storage.DefaultRequestHeaders.UserAgent.ParseAdd(Value);
    }

    private void AcceptUser(JsonElement User)
    {
        UserId = User.GetProperty("id").GetString() ?? throw new InvalidDataException("No user identity was returned.");

        DisplayName = User.GetProperty("displayName").GetString() ?? UserId;
    }

    private void RequireLogin()
    {
        if (UserId is null)
        {
            throw new InvalidOperationException("Sign in to VRChat first.");
        }
    }

    /// <summary>Uses the SDK response status and extracts only the API's user-facing error message.</summary>
    private static JsonDocument Read<T>(ApiResponse<T> Response)
    {
        if ((int)Response.StatusCode is < 200 or >= 300)
        {
            string Message = $"VRChat returned HTTP {(int)Response.StatusCode}.";

            try
            {
                using JsonDocument Error = JsonDocument.Parse(Response.RawContent);

                Message = ServerExplanation.GetMessage(Error.RootElement) ?? Message;
            }
            catch (JsonException)
            {
                // HTML and proxy responses are represented by the HTTP status alone.
            }

            if (Response.StatusCode is HttpStatusCode.TooManyRequests)
            {
                Message = "Rate limited by VRChat. Wait before trying again. " + Message;
            }

            string Explanation = Response.StatusCode is HttpStatusCode.TooManyRequests ? "Rate limit exceeded" : Message;

            throw new ServerRequestException(Message[..Math.Min(600, Message.Length)], Response.StatusCode, Explanation);
        }

        return JsonDocument.Parse(Response.RawContent);
    }

    private static string Text(JsonElement Element, string Name) => Element.TryGetProperty(Name, out JsonElement Value) ? Value.GetString() ?? "" : "";

    public void Dispose()
    {
        PreviewCache.Dispose();

        Authentication.Dispose();

        Files.Dispose();

        Groups.Dispose();

        Avatars.Dispose();

        Client.Dispose();

        Storage.Dispose();

        Handler.Dispose();
    }
}
