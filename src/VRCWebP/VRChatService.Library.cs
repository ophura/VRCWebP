using System.Net;
using System.Text.Json;
using VRChat.API.Client;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    internal const int ImageBatchSize = 100;

    /// <summary>Lists account-owned image metadata in bounded batches; no image body is downloaded.</summary>
    internal async Task<RemoteImageBatch> ListImagesAsync(int Offset, CancellationToken Token)
    {
        RequireLogin();

        ArgumentOutOfRangeException.ThrowIfNegative(Offset);

        using JsonDocument Document = Read(await Files.GetFilesWithHttpInfoAsync(n: ImageBatchSize, offset: Offset, cancellationToken: Token));

        if (Document.RootElement.ValueKind is not JsonValueKind.Array)
        {
            throw new InvalidDataException("VRChat returned an invalid file list.");
        }

        List<RemoteImageFile> Images = [];

        foreach (JsonElement Entry in Document.RootElement.EnumerateArray())
        {
            if (Text(Entry, "ownerId") != UserId || Text(Entry, "mimeType") is not ("image/png" or "image/jpeg" or "image/webp") || !Entry.TryGetProperty("versions", out JsonElement Versions) || Versions.ValueKind is not JsonValueKind.Array)
            {
                continue;
            }

            string Id = Text(Entry, "id");

            if (Id.Length is not 41 || !Id.StartsWith("file_", StringComparison.Ordinal) || !Guid.TryParseExact(Id.AsSpan(5), "D", out _))
            {
                continue;
            }

            int Version = 0;

            long Bytes = 0;

            bool Complete = false;

            foreach (JsonElement Candidate in Versions.EnumerateArray())
            {
                if (!Candidate.TryGetProperty("version", out JsonElement Number) || !Number.TryGetInt32(out int Current) || Current <= Version || (Candidate.TryGetProperty("deleted", out JsonElement Deleted) && Deleted.ValueKind is JsonValueKind.True))
                {
                    continue;
                }

                Version = Current;

                Complete = Text(Candidate, "status") is "complete";

                if (Candidate.TryGetProperty("file", out JsonElement File) && File.ValueKind is JsonValueKind.Object)
                {
                    Complete &= Text(File, "status") is "complete";

                    Bytes = File.TryGetProperty("sizeInBytes", out JsonElement Size) && Size.TryGetInt64(out long Length) ? Length : 0;
                }
                else
                {
                    Complete = false;
                }
            }

            string Name = Text(Entry, "name");

            Images.Add(new RemoteImageFile(Id, Name.Length is 0 ? "Unnamed image" : Name[..Math.Min(256, Name.Length)], Text(Entry, "extension"), Bytes, Version, Complete));
        }

        return new RemoteImageBatch(Images, Offset, Document.RootElement.GetArrayLength() >= ImageBatchSize);
    }

    /// <summary>Deletes only an explicitly selected owned image record, then verifies that it is gone.</summary>
    internal async Task DeleteImageAsync(string FileId, CancellationToken Token)
    {
        RequireLogin();

        (string Id, _) = ParseImageReference(FileId);

        try
        {
            using JsonDocument Existing = Read(await Files.GetFileWithHttpInfoAsync(Id, Token));

            if (Text(Existing.RootElement, "id") != Id || Text(Existing.RootElement, "ownerId") != UserId
                || Text(Existing.RootElement, "mimeType") is not ("image/png" or "image/jpeg" or "image/webp"))
            {
                throw new InvalidOperationException("Only image files uploaded by the connected account can be deleted here.");
            }

            ApiResponse<VRChat.API.Model.File> Deleted = await Files.DeleteFileWithHttpInfoAsync(Id, Token);

            if ((int)Deleted.StatusCode is < 200 or >= 300)
            {
                using JsonDocument Failure = Read(Deleted);
            }

            ApiResponse<VRChat.API.Model.File> Confirmed = await Files.GetFileWithHttpInfoAsync(Id, Token);

            if (Confirmed.StatusCode is not HttpStatusCode.NotFound)
            {
                if ((int)Confirmed.StatusCode is < 200 or >= 300)
                {
                    using JsonDocument Failure = Read(Confirmed);
                }

                throw new InvalidOperationException("Deletion was requested but is not confirmed. Refresh before trying again.");
            }

            await UploadLog.WriteAsync("Delete owned image", Status: 200);

            try
            {
                await PreviewCache.RemoveAsync(UserId!, Id);
            }
            catch (Exception Error)
            {
                await UploadLog.WriteAsync("Deleted image preview cleanup", Error.GetType().Name);
            }
        }
        catch (Exception Error) when (Error is not OperationCanceledException)
        {
            await LogImageFailureAsync("Delete owned image", Error);

            throw;
        }
    }
}
