using System.Net;
using System.Text.Json;
using VRChat.API.Model;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    /// <summary>Owns an Original destination checkpoint keyed by account, immutable digest/length and file/version; inventory recovery IDs are independent. Never persisted.</summary>
    private sealed class UploadCheckpoint(string UserId, string FileId, string Md5, long Bytes)
    {
        internal string UserId { get; } = UserId;

        internal string FileId { get; } = FileId;

        internal string Md5 { get; } = Md5;

        internal long Bytes { get; } = Bytes;

        internal int Version { get; set; }

        internal DescriptorCheckpoint File { get; } = new();

        internal DescriptorCheckpoint Signature { get; } = new();
    }

    /// <summary>Advances only after storage acknowledges a complete part and returns its ETag.</summary>
    private sealed class DescriptorCheckpoint
    {
        internal long Sent { get; set; }

        internal List<string> Etags { get; } = [];

        internal bool Finished { get; set; }

        internal string? UploadId { get; set; }
    }

    /// <summary>Reconciles server state before reusing an interrupted file or creating a missing version.</summary>
    private async Task<JsonDocument> PrepareUploadAsync(string Name, long Bytes, string Md5, int SignatureBytes, string SignatureMd5, CancellationToken Token)
    {
        if (Checkpoint is not null && (Checkpoint.UserId != UserId || Checkpoint.Md5 != Md5 || Checkpoint.Bytes != Bytes))
        {
            Checkpoint = null;
        }

        if (Checkpoint is null)
        {
            PendingFileId = null;

            await UploadStepAsync("Create file");

            string Extension = VRChatImageRecord.Extension;

            CreateFileRequestMIMEType Mime = CreateFileRequestMIMEType.ImagePng;


            using JsonDocument Created = Read(await Files.CreateFileWithHttpInfoAsync(
                new CreateFileRequest(Extension, Mime, Name, new List<string>()), Token));

            string Id = Created.RootElement.GetProperty("id").GetString() ?? throw new InvalidDataException("The server returned no file ID.");

            Checkpoint = new UploadCheckpoint(UserId!, Id, Md5, Bytes);

            PendingFileId = Id;
        }
        else
        {
            PendingFileId = Checkpoint.FileId;

            await UploadStepAsync("Resume file");

            JsonDocument Existing = Read(await Files.GetFileWithHttpInfoAsync(Checkpoint.FileId, Token));

            try
            {
                foreach (JsonElement Version in Existing.RootElement.GetProperty("versions").EnumerateArray())
                {
                    if ((Checkpoint.Version > 0 && Version.GetProperty("version").GetInt32() == Checkpoint.Version)
                        || (Checkpoint.Version is 0 && Version.TryGetProperty("file", out JsonElement Data)
                            && Data.ValueKind is JsonValueKind.Object && Text(Data, "md5") == Md5
                            && Data.TryGetProperty("sizeInBytes", out JsonElement Size) && Size.TryGetInt64(out long StoredBytes) && StoredBytes == Bytes))
                    {
                        Checkpoint.Version = Version.GetProperty("version").GetInt32();

                        ValidateDescriptor(Version.GetProperty("file"), Md5, Bytes);

                        ValidateDescriptor(Version.GetProperty("signature"), SignatureMd5, SignatureBytes);

                        return Existing;
                    }
                }

                if (Checkpoint.Version > 0)
                {
                    throw new InvalidOperationException("The unfinished upload version no longer exists. Its recovery ID has been retained.");
                }
            }
            catch
            {
                Existing.Dispose();

                throw;
            }

            Existing.Dispose();
        }

        await UploadStepAsync("Create version");

        JsonDocument CreatedVersion = Read(await Files.CreateFileVersionWithHttpInfoAsync(Checkpoint.FileId,
            new CreateFileVersionRequest(Md5, Bytes, SignatureMd5, SignatureBytes), Token));

        try
        {
            Checkpoint.Version = CreatedVersion.RootElement.GetProperty("versions").EnumerateArray().Last().GetProperty("version").GetInt32();

            return CreatedVersion;
        }
        catch
        {
            CreatedVersion.Dispose();

            throw;
        }
    }

    /// <summary>Rejects mismatched server metadata rather than mixing bytes or retrying a rejected version.</summary>
    private static void ValidateDescriptor(JsonElement Data, string Md5, long Bytes)
    {
        if (Text(Data, "status") is "error"
            || (Data.TryGetProperty("md5", out JsonElement Hash) && Hash.GetString() != Md5)
            || (Data.TryGetProperty("sizeInBytes", out JsonElement Size) && Size.GetInt64() != Bytes))
        {
            throw new InvalidDataException("The unfinished upload does not match the local file or was rejected by the server.");
        }
    }

    /// <summary>Checks the descriptor after a rejected finish before retrying the same version and ETags.</summary>
    private async Task FinishDescriptorAsync(string FileId, int Version, string Type, DescriptorCheckpoint Saved, bool Multipart, CancellationToken Token)
    {
        if (Saved.Finished)
        {
            return;
        }

        for (int Attempt = 0; ; ++Attempt)
        {
            await UploadStepAsync("Finish " + Type);

            try
            {
                using JsonDocument Finished = Read(await Files.FinishFileDataUploadWithHttpInfoAsync(FileId, Version, Type,
                    new FinishFileDataUploadRequest(etags: Multipart ? Saved.Etags : new List<string>()), Token));

                Saved.Finished = true;

                return;
            }
            catch (ServerRequestException Error) when (Error.StatusCode is HttpStatusCode.TooManyRequests && Attempt < 6)
            {
                using JsonDocument State = Read(await Files.GetFileWithHttpInfoAsync(FileId, Token));

                JsonElement Entry = State.RootElement.GetProperty("versions").EnumerateArray().Single(Item => Item.GetProperty("version").GetInt32() == Version);

                if (Text(Entry.GetProperty(Type), "status") is "complete")
                {
                    Saved.Finished = true;

                    return;
                }

                if (Text(Entry.GetProperty(Type), "status") is "error")
                {
                    throw new InvalidDataException("VRChat rejected the unfinished upload descriptor.");
                }
            }
        }
    }
}
