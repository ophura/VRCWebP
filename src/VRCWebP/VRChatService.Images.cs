using System.Security.Cryptography;
using System.Text.Json;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    /// <summary>Uploads to an explicitly selected permitted inventory and verifies the actual downloaded bytes.</summary>
    private async Task<UploadResult> UploadImageAsync(ConversionResult Media, UploadDestination Destination, FileStream Source, IProgress<WorkProgress> Progress, CancellationToken Token, AvatarGalleryState? Gallery = null)
    {
        PendingFileId = null;

        await UploadStepAsync("Upload inventory image");

        using UploadContent Content = new(Source, Source.Length, VRChatImageRecord.MimeType, Bytes => Progress.Report(new WorkProgress(Source.Length > 0 ? Bytes * 100d / Source.Length : 100, "Uploading image")));

        using MultipartFormDataContent Form = new();

        Form.Add(new StringContent(Destination switch
        {
            UploadDestination.Gallery => "gallery",

            UploadDestination.AvatarGallery => "avatargallery",

            _ => "icon"
        }), "tag");

        if (Gallery is not null)
        {
            Form.Add(new StringContent(Gallery.AvatarId), "galleryId");
        }

        Form.Add(Content, "file", "animation" + VRChatImageRecord.Extension);

        using HttpRequestMessage Request = new(HttpMethod.Post, "https://api.vrchat.cloud/api/1/file/image") { Content = Form };

        using HttpResponseMessage Response = await Client.SendAsync(Request, HttpCompletionOption.ResponseHeadersRead, Token);

        if (!Response.IsSuccessStatusCode)
        {
            string Explanation = await ReadServerExplanationAsync(Response, Token);

            throw new ServerRequestException($"Image upload failed (HTTP {(int)Response.StatusCode}): {Explanation}.", Response.StatusCode, Explanation);
        }

        using PooledBuffer Buffer = new(1048577);

        await using Stream Body = await Response.Content.ReadAsStreamAsync(Token);

        int Count = await Body.ReadAtLeastAsync(Buffer.Memory, Buffer.Memory.Length, false, Token);

        if (Count > 1048576)
        {
            throw new InvalidDataException("The image upload response exceeds its supported length.");
        }

        using JsonDocument Document = JsonDocument.Parse(Buffer.Memory[..Count]);

        string Id = Document.RootElement.GetProperty("id").GetString() ?? throw new InvalidDataException("The server returned no file ID.");

        PendingFileId = Id;

        if (Text(Document.RootElement, "extension") is not VRChatImageRecord.Extension
            || Text(Document.RootElement, "mimeType") is not VRChatImageRecord.MimeType)
        {
            throw new InvalidDataException("VRChat returned a different image-record declaration. Keep the recovery ID; this file has not been selected for use.");
        }

        if (!Id.StartsWith("file_", StringComparison.Ordinal) || !Guid.TryParse(Id.AsSpan(5), out _))
        {
            throw new InvalidDataException("The server returned an invalid file ID.");
        }

        int Version = Document.RootElement.GetProperty("versions").EnumerateArray().Last().GetProperty("version").GetInt32();

        if (Version < 1)
        {
            throw new InvalidDataException("The server returned no uploaded image version.");
        }

        string Url = $"https://api.vrchat.cloud/api/1/file/{Id}/{Version}/file";

        await UploadStepAsync("Verify stored file");

        Progress.Report(new WorkProgress(100, "Verifying stored image"));

        await VerifyStoredFileAsync(Source, Url, Token);

        if (Gallery is not null)
        {
            await UploadStepAsync("Verify avatar gallery");

            AvatarGalleryState Confirmed = await ReadAvatarGalleryAsync(Gallery.AvatarId, Token, VerifyOwner: false);

            if (!Confirmed.FileIds.Contains(Id) || Confirmed.FileIds.Count > 6 || !Confirmed.FileIds.Where(File => File != Id).SequenceEqual(Gallery.FileIds))
            {
                throw new InvalidDataException("Uploaded, but the avatar gallery membership or ordering could not be confirmed. Keep the recovery ID and refresh before retrying.");
            }
        }

        PendingFileId = null;

        return new UploadResult(Id, Url, Version, "Uploaded: original verified", OwnerId: UserId, ProfileIconEligible: HasImageTag(Document.RootElement, "icon"));
    }

    /// <summary>Checks the stored original with bounded pooled reads and a cancellable body deadline.</summary>
    private async Task VerifyStoredFileAsync(Stream Source, string Url, CancellationToken Token)
    {
        Source.Position = 0;

        byte[] Expected = await SHA256.HashDataAsync(Source, Token);

        using CancellationTokenSource Deadline = CancellationTokenSource.CreateLinkedTokenSource(Token);

        Deadline.CancelAfter(TimeSpan.FromMinutes(10));

        using HttpResponseMessage Download = await Client.GetAsync(Url, HttpCompletionOption.ResponseHeadersRead, Deadline.Token);

        if (!Download.IsSuccessStatusCode)
        {
            string Explanation = await ReadServerExplanationAsync(Download, Token);

            throw new ServerRequestException($"Stored-file verification failed (HTTP {(int)Download.StatusCode}): {Explanation}.", Download.StatusCode, Explanation);
        }

        await using Stream Stored = await Download.Content.ReadAsStreamAsync(Deadline.Token);

        using IncrementalHash Hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        using PooledBuffer Buffer = new(65536);

        long Received = 0;

        while (true)
        {
            int Wanted = (int)Math.Min(Buffer.Memory.Length, Math.Max(1, Source.Length - Received));

            int Count = await Stored.ReadAsync(Buffer.Memory[..Wanted], Deadline.Token);

            if (Count is 0)
            {
                break;
            }

            if (Count > Source.Length - Received)
            {
                throw new InvalidDataException("Uploaded, but the stored image is longer than the original.");
            }

            Hash.AppendData(Buffer.Memory.Span[..Count]);

            Received += Count;
        }

        if (Received != Source.Length || !Expected.AsSpan().SequenceEqual(Hash.GetHashAndReset()))
        {
            throw new InvalidDataException("Uploaded, but the stored image differs from the original.");
        }
    }
}
