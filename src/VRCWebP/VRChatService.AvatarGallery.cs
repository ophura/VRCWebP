using System.Text.Json;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    /// <summary>Validates an owned avatar and reads its ordered gallery once without downloading image bodies.</summary>
    internal async Task<AvatarGalleryState> ReadAvatarGalleryAsync(string AvatarId, CancellationToken Token, bool VerifyOwner = true)
    {
        RequireLogin();

        using CancellationTokenSource Deadline = CancellationTokenSource.CreateLinkedTokenSource(Token);

        Deadline.CancelAfter(TimeSpan.FromSeconds(25));

        Token = Deadline.Token;

        AvatarId = AvatarId.Trim();

        if (!IsAvatarId(AvatarId))
        {
            throw new ArgumentException("Enter a complete avtr_ ID for an avatar you own.");
        }

        try
        {
            if (VerifyOwner)
            {
                using JsonDocument Avatar = Read(await Avatars.GetAvatarWithHttpInfoAsync(AvatarId, Token));

                if (Text(Avatar.RootElement, "id") != AvatarId || Text(Avatar.RootElement, "authorId") != UserId)
                {
                    throw new InvalidOperationException("Only the connected account's own avatar gallery can be changed.");
                }
            }

            using HttpResponseMessage Response = await Client.GetAsync($"https://api.vrchat.cloud/api/1/files?n=100&offset=0&tag=avatargallery&galleryId={AvatarId}", HttpCompletionOption.ResponseHeadersRead, Token);

            if (!Response.IsSuccessStatusCode)
            {
                string Explanation = await ReadServerExplanationAsync(Response, Token);

                throw new ServerRequestException($"Avatar gallery could not be read (HTTP {(int)Response.StatusCode}): {Explanation}.", Response.StatusCode, Explanation);
            }

            using PooledBuffer Buffer = new(1048577);

            await using Stream Body = await Response.Content.ReadAsStreamAsync(Token);

            int Count = await Body.ReadAtLeastAsync(Buffer.Memory, Buffer.Memory.Length, false, Token);

            if (Count > 1048576)
            {
                throw new InvalidDataException("The avatar gallery response exceeds its supported length.");
            }

            using JsonDocument Document = JsonDocument.Parse(Buffer.Memory[..Count]);

            if (Document.RootElement.ValueKind is not JsonValueKind.Array)
            {
                throw new InvalidDataException("VRChat returned an invalid avatar gallery.");
            }

            List<string> Ids = [];

            foreach (JsonElement Image in Document.RootElement.EnumerateArray())
            {
                string Id = Text(Image, "id");

                if (Id.Length is not 41 || !Id.StartsWith("file_", StringComparison.Ordinal) || !Guid.TryParseExact(Id.AsSpan(5), "D", out _) || Text(Image, "ownerId") != UserId || Ids.Contains(Id))
                {
                    throw new InvalidDataException("VRChat returned an unexpected avatar gallery record.");
                }

                Ids.Add(Id);
            }

            return new AvatarGalleryState(AvatarId, Ids);
        }
        catch (Exception Error) when (Error is not OperationCanceledException)
        {
            await LogImageFailureAsync("Read avatar gallery", Error);

            throw;
        }
    }

    /// <summary>Accepts only the canonical avatar identifier, never an arbitrary request URL.</summary>
    internal static bool IsAvatarId(string Id) => IsTargetId(Id, "avtr_");
}
