using System.Text;
using System.Text.Json;
using System.Xml;

namespace VRCWebP;

internal sealed partial class VRChatService
{
    /// <summary>Reads bounded API reasons or S3 codes, never persisting XML messages or signed request details.</summary>
    private static async Task<string> ReadServerExplanationAsync(HttpResponseMessage Response, CancellationToken Token)
    {
        using CancellationTokenSource Timeout = CancellationTokenSource.CreateLinkedTokenSource(Token);

        Timeout.CancelAfter(TimeSpan.FromSeconds(10));

        try
        {
            using PooledBuffer Lease = new(8192);

            await using Stream Source = await Response.Content.ReadAsStreamAsync(Timeout.Token);

            int Count = await Source.ReadAtLeastAsync(Lease.Memory, Lease.Memory.Length, false, Timeout.Token);

            try
            {
                using JsonDocument Error = JsonDocument.Parse(Lease.Memory[..Count]);

                if (ServerExplanation.GetMessage(Error.RootElement) is string Message)
                {
                    return ServerExplanation.Sanitize(Message);
                }
            }
            catch (JsonException)
            {
                // Object storage normally returns XML rather than an API JSON envelope.
            }

            using StringReader Text = new(Encoding.UTF8.GetString(Lease.Memory.Span[..Count]));

            using XmlReader Reader = XmlReader.Create(Text, new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,

                XmlResolver = null,

                MaxCharactersInDocument = 8192
            });

            while (Reader.Read())
            {
                if (Reader.NodeType is XmlNodeType.Element && Reader.LocalName is "Code")
                {
                    return ServerExplanation.Sanitize(Reader.ReadElementContentAsString());
                }
            }
        }
        catch (XmlException)
        {
            // Proxies may return HTML, empty data or a truncated document.
        }
        catch (Exception Error) when (Error is IOException or HttpRequestException)
        {
            // An unreadable error body must not replace the original HTTP failure.
        }
        catch (OperationCanceledException) when (!Token.IsCancellationRequested)
        {
            // A stalled error body is bounded independently of the completed headers.
        }

        return ServerExplanation.Sanitize(Response.ReasonPhrase);
    }
}
