using System.Text;
using System.Text.Json;

namespace VRCWebP;

/// <summary>Retains bounded local upload diagnostics without response bodies, URLs, identifiers or credentials.</summary>
internal static class UploadLog
{
    private static readonly SemaphoreSlim Gate = new(1);


    /// <summary>Appends fixed fields and allowlisted public server reasons; unknown response text is never written.</summary>
    internal static async Task WriteAsync(string Stage, string? Failure = null, int? Status = null, string? Explanation = null, long? FileBytes = null,
        string? Method = null, string? Route = null, int? Part = null, int? Attempt = null, double? ElapsedMilliseconds = null, double? RetryAfterMilliseconds = null,
        double? ServerRetryAfterSeconds = null, DateTimeOffset? ServerRetryAt = null)
    {
        await Gate.WaitAsync();

        try
        {
            string Path =
                System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VRCWebP", "upload.log");

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path)!);

            if (File.Exists(Path) && new FileInfo(Path).Length > 65536)
            {
                File.Move(Path, Path + ".previous", true);
            }

            using MemoryStream Buffer = new();

            using (Utf8JsonWriter Writer = new(Buffer))
            {
                Writer.WriteStartObject();

                Writer.WriteString("utc", DateTimeOffset.UtcNow);

                Writer.WriteString("stage", Stage);

                if (Failure is not null)
                {
                    Writer.WriteString("failure", Failure);
                }

                if (Status is int Code)
                {
                    Writer.WriteNumber("httpStatus", Code);
                }

                if (Explanation is not null)
                {
                    Writer.WriteString("serverExplanation", ServerExplanation.Sanitize(Explanation));
                }

                if (FileBytes is long Bytes)
                {
                    Writer.WriteNumber("fileBytes", Bytes);
                }

                if (Method is not null && Route is not null)
                {
                    Writer.WriteString("method", Method);

                    Writer.WriteString("route", Route);
                }

                if (Part is int Number)
                {
                    Writer.WriteNumber("part", Number);
                }

                if (Attempt is int Retry)
                {
                    Writer.WriteNumber("attempt", Retry);
                }

                if (ElapsedMilliseconds is double Elapsed)
                {
                    Writer.WriteNumber("elapsedMs", Math.Round(Elapsed, 2));
                }

                if (RetryAfterMilliseconds is double Delay)
                {
                    Writer.WriteNumber("retryAfterMs", Math.Ceiling(Delay));
                }

                if (ServerRetryAfterSeconds is double Seconds)
                {
                    Writer.WriteNumber("serverRetryAfterSeconds", Seconds);
                }

                if (ServerRetryAt is DateTimeOffset Date)
                {
                    Writer.WriteString("serverRetryAtUtc", Date.ToUniversalTime());
                }

                Writer.WriteEndObject();
            }

            Buffer.WriteByte(10);

            ReadOnlyMemory<byte> Line = Buffer.GetBuffer().AsMemory(0, (int)Buffer.Length);

            _ = new UTF8Encoding(false, true).GetCharCount(Line.Span);

            if (Line.Span.Contains((byte)13) || Line.Span.StartsWith(new byte[] { 239, 187, 191 }))
            {
                throw new IOException("Upload log encoding is invalid.");
            }

            await using FileStream Output = new(Path, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, FileOptions.Asynchronous);

            await Output.WriteAsync(Line);
        }
        catch (Exception Error) when (Error is IOException or UnauthorizedAccessException or DecoderFallbackException)
        {
            // Logging must not prevent an upload or obscure its original failure.
        }
        finally
        {
            Gate.Release();
        }
    }
}
