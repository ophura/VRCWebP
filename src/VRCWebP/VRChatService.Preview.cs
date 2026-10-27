namespace VRCWebP;

internal sealed partial class VRChatService
{
    /// <summary>Retains the failed preview stage and safe error category without image links or local paths.</summary>
    internal static Task LogPreviewFailureAsync(string Stage, Exception Error) => LogImageFailureAsync(Stage, Error);

    private readonly ImagePreviewCache PreviewCache;


    /// <summary>Reuses persistent local derivatives and fetches at most one bounded server thumbnail on a cache miss.</summary>
    internal Task<string> ImagePreviewAsync(RemoteImageFile Image, CancellationToken Token)
    {
        RequireLogin();

        return PreviewCache.GetAsync(UserId!, Image.Id, Image.Version, async (Destination, Stop) =>
        {
            using HttpResponseMessage Response = await Client.GetAsync($"https://api.vrchat.cloud/api/1/image/{Image.Id}/{Image.Version}/256",
                HttpCompletionOption.ResponseHeadersRead, Stop);

            if (!Response.IsSuccessStatusCode)
            {
                string Explanation = await ReadServerExplanationAsync(Response, Stop);

                throw new ServerRequestException("Preview unavailable: " + Explanation, Response.StatusCode, Explanation);
            }

            const long MaximumThumbnail = UploadPolicy.AvatarGalleryMaximumBytes;

            if (Response.Content.Headers.ContentLength > MaximumThumbnail)
            {
                throw new InvalidDataException("The server image exceeds the bounded preview download limit (10 MiB).");
            }

            await using Stream Source = await Response.Content.ReadAsStreamAsync(Stop);

            await using FileStream Output = new(Destination, FileMode.CreateNew, FileAccess.Write, FileShare.None, 65536, FileOptions.Asynchronous);

            using PooledBuffer Buffer = new(65536);

            long Total = 0;

            while (true)
            {
                int Count = await Source.ReadAsync(Buffer.Memory, Stop);

                if (Count is 0)
                {
                    break;
                }

                Total += Count;

                if (Total > MaximumThumbnail)
                {
                    throw new InvalidDataException("The server image exceeded the bounded preview download limit (10 MiB).");
                }

                await Output.WriteAsync(Buffer.Memory[..Count], Stop);
            }

            return Destination;
        }, Token);
    }

    /// <summary>Captures a preview while the verified encoded original is still available, without a second image download.</summary>
    private async Task RememberLocalPreviewAsync(UploadResult Uploaded, string Source, CancellationToken Token)
    {
        try
        {
            _ = await PreviewCache.StoreLocalAsync(UserId!, Uploaded.FileId, Uploaded.Version, Source, Token);
        }
        catch (Exception Error)
        {
            // A cache problem must not turn a completed, verified upload into a failed upload.
            await UploadLog.WriteAsync("Local preview cache", Error.GetType().Name);
        }
    }
}
