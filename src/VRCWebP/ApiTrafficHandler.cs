using System.Globalization;
using System.Net;

namespace VRCWebP;

/// <summary>Serializes API admission, paces upload starts and shares server cooldowns across the session.</summary>
internal sealed class ApiTrafficHandler : DelegatingHandler
{
    private readonly SemaphoreSlim Gate = new(1);

    private readonly TimeProvider Clock;

    private readonly long Epoch;

    private double NextRequest;

    private double NextUploadStart;

    private double CooldownUntil;

    private readonly Dictionary<string, int> Backoffs = new(StringComparer.Ordinal);

    internal Action<TimeSpan>? Paused { get; set; }


    internal ApiTrafficHandler(HttpMessageHandler Transport, TimeProvider? Clock = null) : base(Transport)
    {
        this.Clock = Clock ?? TimeProvider.System;

        Epoch = this.Clock.GetTimestamp();
    }

    private double Now => Clock.GetElapsedTime(Epoch).TotalSeconds;

    /// <summary>Honors both Retry-After forms in full; only the fallback backoff has an upper bound.</summary>
    internal static TimeSpan RetryDelay(HttpResponseMessage Response, DateTimeOffset Now, int Attempt)
    {
        TimeSpan? Supplied = Response.Headers.RetryAfter?.Delta ?? (Response.Headers.RetryAfter?.Date - Now);

        double Seconds = Supplied?.TotalSeconds ?? Math.Min(120, Math.Pow(2, Math.Min(Attempt + 1, 7)));

        return TimeSpan.FromSeconds(Math.Max(1, Seconds) + (Random.Shared.NextDouble() * 0.25));
    }

    /// <summary>Also pauses API traffic after a storage-side 429 without logging its signed address.</summary>
    internal void ObserveCooldown(HttpResponseMessage Response, int Attempt)
    {
        double Until = Now + RetryDelay(Response, Clock.GetUtcNow(), Attempt).TotalSeconds;

        double Previous;

        do
        {
            Previous = Volatile.Read(ref CooldownUntil);
        }
        while (Until > Previous && Interlocked.CompareExchange(ref CooldownUntil, Until, Previous) != Previous);
    }

    /// <summary>Retries only reads and the same upload-part URL acquisition; mutations are reconciled by their workflow.</summary>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage Request, CancellationToken Token)
    {
        await Gate.WaitAsync(Token).ConfigureAwait(false);

        try
        {
            string Route = NormalizedRoute(Request.RequestUri!);

            string BackoffKey = Request.Method.Method + ":" + Route;

            bool Start = Request.Method == HttpMethod.Put && Route.EndsWith("/start", StringComparison.Ordinal);

            bool Retryable = Request.Method == HttpMethod.Get || Request.Method == HttpMethod.Head || Start;

            int? Part = PartNumber(Request.RequestUri!);

            for (int Attempt = 0; ; ++Attempt)
            {
                await WaitAsync(Start, Token).ConfigureAwait(false);

                NextRequest = Now + 0.25;

                if (Start)
                {
                    NextUploadStart = Now + 1.5;
                }

                using HttpRequestMessage? Replay = Attempt is 0 ? null : CopyReadRequest(Request);

                long Started = Clock.GetTimestamp();

                HttpResponseMessage Response;

                try
                {
                    CancellationTokenSource Deadline = CancellationTokenSource.CreateLinkedTokenSource(Token);

                    try
                    {
                        Deadline.CancelAfter(Route is "/file/image" ? TimeSpan.FromMinutes(10) : TimeSpan.FromMinutes(2));

                        Response =
                            await base.SendAsync(Replay ?? Request, Deadline.Token).ConfigureAwait(false);

                        // Headers-first completion must transfer ownership through the last body read.
                        Response.Content = new DeadlineContent(Response.Content, Deadline);
                    }
                    catch
                    {
                        Deadline.Dispose();

                        throw;
                    }
                }
                catch (Exception Error) when (Error is HttpRequestException or OperationCanceledException)
                {
                    await UploadLog.WriteAsync("API request", Error.GetType().Name, Method: Request.Method.Method,
                        Route: Route, Part: Part, Attempt: Attempt + 1,
                        ElapsedMilliseconds: Clock.GetElapsedTime(Started).TotalMilliseconds).ConfigureAwait(false);

                    throw;
                }

                TimeSpan? Cooldown = null;

                if (Response.StatusCode is HttpStatusCode.TooManyRequests)
                {
                    Backoffs.TryGetValue(BackoffKey, out int Consecutive);

                    ObserveCooldown(Response, Consecutive);

                    Backoffs[BackoffKey] = Math.Min(Consecutive + 1, 7);

                    Cooldown = TimeSpan.FromSeconds(Math.Max(0, Volatile.Read(ref CooldownUntil) - Now));
                }
                else if (Response.IsSuccessStatusCode)
                {
                    Backoffs.Remove(BackoffKey);
                }

                await UploadLog.WriteAsync("API request", Status: (int)Response.StatusCode,
                    Explanation: Cooldown is null ? null : "Rate limit exceeded", Method: Request.Method.Method,
                    Route: Route, Part: Part, Attempt: Attempt + 1,
                    ElapsedMilliseconds: Clock.GetElapsedTime(Started).TotalMilliseconds,
                    RetryAfterMilliseconds: Cooldown?.TotalMilliseconds,
                    ServerRetryAfterSeconds: Response.Headers.RetryAfter?.Delta?.TotalSeconds,
                    ServerRetryAt: Response.Headers.RetryAfter?.Date).ConfigureAwait(false);

                if (Cooldown is null || !Retryable || Attempt >= 6)
                {
                    return Response;
                }

                Response.Dispose();
            }
        }
        finally
        {
            Gate.Release();
        }
    }

    private async Task WaitAsync(bool UploadStart, CancellationToken Token)
    {
        while (true)
        {
            Token.ThrowIfCancellationRequested();

            double Cooldown = Volatile.Read(ref CooldownUntil) - Now;

            double Remaining = Math.Max(Math.Max(NextRequest, UploadStart ? NextUploadStart : 0), Volatile.Read(ref CooldownUntil)) - Now;

            if (Remaining <= 0)
            {
                return;
            }

            if (Cooldown > 0)
            {
                Paused?.Invoke(TimeSpan.FromSeconds(Cooldown));
            }

            await Task.Delay(TimeSpan.FromSeconds(Math.Min(Remaining, 1)), Clock, Token).ConfigureAwait(false);
        }
    }

    private static HttpRequestMessage CopyReadRequest(HttpRequestMessage Original)
    {
        if (Original.Content is not null)
        {
            throw new InvalidOperationException("This API request body cannot be replayed automatically.");
        }

        HttpRequestMessage Copy = new(Original.Method, Original.RequestUri)
        {
            Version = Original.Version,

            VersionPolicy = Original.VersionPolicy
        };

        foreach (KeyValuePair<string, IEnumerable<string>> Header in Original.Headers)
        {
            _ = Copy.Headers.TryAddWithoutValidation(Header.Key, Header.Value);
        }

        return Copy;
    }

    /// <summary>Produces a fixed route template; resource IDs and arbitrary path/query text never reach the log.</summary>
    internal static string NormalizedRoute(Uri Address)
    {
        string[] Parts = Address.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (Parts is ["api", "1", "auth", "user" or "permissions"])
        {
            return "/auth/" + Parts[3];
        }

        return Parts switch
        {
            ["api", "1", "file"] => "/file",

            ["api", "1", "files"] => "/files",

            ["api", "1", "profile", _] => "/profile/{userId}",

            ["api", "1", "image", _, _, _] => "/image/{fileId}/{versionId}/{size}",

            ["api", "1", "file", "image"] => "/file/image",

            ["api", "1", "file", _] => "/file/{fileId}",

            ["api", "1", "file", _, _] => "/file/{fileId}/{versionId}",

            ["api", "1", "file", _, _, "file" or "signature" or "delta"] => "/file/{fileId}/{versionId}/" + Parts[5],

            ["api", "1", "file", _, _, "file" or "signature" or "delta", "start" or "finish" or "status"]
                => "/file/{fileId}/{versionId}/" + Parts[5] + "/" + Parts[6],

            ["api", "1", "groups", _] => "/groups/{groupId}",

            ["api", "1", "avatars", _] => "/avatars/{avatarId}",

            ["api", "1", "avatars", _, "select"] => "/avatars/{avatarId}/select",

            _ => "/other"
        };
    }

    private static int? PartNumber(Uri Address)
    {
        foreach (string Parameter in Address.Query.TrimStart('?').Split('&'))
        {
            if (Parameter.StartsWith("partNumber=", StringComparison.Ordinal)
                && int.TryParse(Parameter.AsSpan(11), NumberStyles.None, CultureInfo.InvariantCulture, out int Part) && Part is > 0 and <= 10000)
            {
                return Part;
            }
        }

        return null;
    }
}
