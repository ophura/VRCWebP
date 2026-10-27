using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VRChat.API.Model;

namespace VRChat.API.Client;

/// <summary>
/// NativeAOT transport for the upstream generated WithHttpInfoAsync endpoints.
/// RawContent is authoritative; typed Data is intentionally unused. JSON is written
/// explicitly with System.Text.Json without runtime reflection.
/// </summary>
public sealed class ApiClient : IAsynchronousClient, ISynchronousClient, IDisposable
{
    /// <summary>Maximum decoded response bytes, including error bodies and responses without a length header.</summary>
    public const int MaximumResponseBytes = 4 * 1024 * 1024;

    private readonly HttpClient Client;

    private readonly string BasePath;

    private readonly bool OwnsClient;

    public ApiClient(string BasePath = "https://api.vrchat.cloud/api/1")
    {
        this.BasePath = BasePath;

        Client = new HttpClient();

        OwnsClient = true;
    }

    public ApiClient(HttpClient Client, string BasePath, HttpClientHandler Handler = null)
    {
        this.Client = Client;

        this.BasePath = BasePath ?? "https://api.vrchat.cloud/api/1";
    }

    /// <summary>Builds one SDK request; the shared session transport owns pacing and safe retries.</summary>
    private async Task<ApiResponse<T>> SendAsync<T>(HttpMethod Method, string Path, RequestOptions Options, IReadableConfiguration Configuration, CancellationToken CancellationToken)
    {
        if (Options.FormParameters.Count is not 0 || Options.FileParameters.Count is not 0 || Options.Cookies.Count is not 0)
        {
            throw new NotSupportedException("Generated form, file and per-request cookie options are unsupported by this NativeAOT adapter. Use the application's streaming inventory uploader.");
        }

        foreach (KeyValuePair<string, string> Parameter in Options.PathParameters)
        {
            Path = Path.Replace("{" + Parameter.Key + "}", Uri.EscapeDataString(Parameter.Value), StringComparison.Ordinal);
        }

        List<string> Query = new();

        foreach (KeyValuePair<string, IList<string>> Parameter in Options.QueryParameters)
        {
            foreach (string Value in Parameter.Value)
            {
                Query.Add(Uri.EscapeDataString(Parameter.Key) + "=" + Uri.EscapeDataString(Value));
            }
        }

        string Address = BasePath.TrimEnd('/') + Path + (Query.Count is 0 ? "" : "?" + string.Join("&", Query));

        {
            using HttpRequestMessage Request = new(Method, Address);

            Request.Headers.TryAddWithoutValidation("User-Agent", Configuration.UserAgent);

            foreach (KeyValuePair<string, IList<string>> Header in Options.HeaderParameters)
            {
                if (!Header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase) && !Header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                {
                    Request.Headers.TryAddWithoutValidation(Header.Key, Header.Value);
                }
            }

            foreach (KeyValuePair<string, string> Header in Configuration.DefaultHeaders)
            {
                Request.Headers.TryAddWithoutValidation(Header.Key, Header.Value);
            }

            if (Options.Data is not null)
            {
                Request.Content = new ByteArrayContent(WriteBody(Options.Data));

                Request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            using HttpResponseMessage Response = await Client.SendAsync(Request, HttpCompletionOption.ResponseHeadersRead, CancellationToken).ConfigureAwait(false);

            string Raw = await ReadResponseAsync(Response.Content, CancellationToken).ConfigureAwait(false);

            Multimap<string, string> Headers = new();

            foreach (KeyValuePair<string, IEnumerable<string>> Header in Response.Headers)
            {
                foreach (string Value in Header.Value)
                {
                    Headers.Add(Header.Key, Value);
                }
            }

            return new ApiResponse<T>(Response.StatusCode, Headers, default, Raw);
        }
    }

    /// <summary>Bounds decoded UTF-8 bytes before creating the authoritative raw JSON string.</summary>
    private static async Task<string> ReadResponseAsync(HttpContent Content, CancellationToken Token)
    {
        if (Content.Headers.ContentLength > MaximumResponseBytes)
        {
            throw new InvalidDataException("The API response exceeds its supported byte limit.");
        }

        // Headers-first completion ends the HttpClient timeout at headers. This body
        // deadline starts after pacing/retries, so cooldowns retain their full duration.
        using CancellationTokenSource Deadline = CancellationTokenSource.CreateLinkedTokenSource(Token);
        Deadline.CancelAfter(TimeSpan.FromMinutes(2));

        using ResponseBuffer Buffer = new(MaximumResponseBytes);

        await Content.CopyToAsync(Buffer, Deadline.Token).ConfigureAwait(false);

        return Buffer.ReadUtf8();
    }

    /// <summary>Serializes only fields edited by this application, preserving all other server values.</summary>
    private static byte[] WriteBody(object Body)
    {
        using MemoryStream Buffer = new();

        using (Utf8JsonWriter Writer = new(Buffer))
        {
            Writer.WriteStartObject();

            switch (Body)
            {
                case CreateFileRequest Request:
                {
                    Writer.WriteString("extension", Request.Extension);

                    Writer.WriteString("mimeType", Request.MimeType switch
                    {
                        CreateFileRequestMIMEType.ImagePng => "image/png",

                        CreateFileRequestMIMEType.ImageWebp => "image/webp",

                        _ => throw new NotSupportedException("This image MIME type has no NativeAOT serializer.")
                    });

                    Writer.WriteString("name", Request.Name);

                    Writer.WriteStartArray("tags");

                    foreach (string Tag in Request.Tags ?? new List<string>())
                    {
                        Writer.WriteStringValue(Tag);
                    }

                    Writer.WriteEndArray();

                    break;
                }
                case CreateFileVersionRequest Request:
                {
                    Writer.WriteString("fileMd5", Request.FileMd5);

                    Writer.WriteNumber("fileSizeInBytes", Request.FileSizeInBytes);

                    Writer.WriteString("signatureMd5", Request.SignatureMd5);

                    Writer.WriteNumber("signatureSizeInBytes", Request.SignatureSizeInBytes);

                    break;
                }
                case TwoFactorAuthCode Request:
                {
                    Writer.WriteString("code", Request.Code);

                    break;
                }
                case TwoFactorEmailCode Request:
                {
                    Writer.WriteString("code", Request.Code);

                    break;
                }
                case UpdateGroupRequest Request:
                {
                    if (Request.BannerId is not null)
                    {
                        Writer.WriteString("bannerId", Request.BannerId);
                    }

                    if (Request.IconId is not null)
                    {
                        Writer.WriteString("iconId", Request.IconId);
                    }

                    break;
                }
                case UpdateAvatarRequest Request:
                {
                    Writer.WriteString("imageUrl", Request.ImageUrl);

                    break;
                }
                case FinishFileDataUploadRequest Request:
                {
                    Writer.WriteString("maxParts", "0");

                    Writer.WriteString("nextPartNumber", "0");

                    if (Request.Etags is not null && Request.Etags.Count > 0)
                    {
                        Writer.WriteStartArray("etags");

                        foreach (string Tag in Request.Etags)
                        {
                            Writer.WriteStringValue(Tag);
                        }

                        Writer.WriteEndArray();
                    }

                    break;
                }
                default:
                {
                    throw new NotSupportedException("This SDK request has no NativeAOT serializer.");
                }
            }

            Writer.WriteEndObject();
        }

        return Buffer.ToArray();
    }

    public void Dispose()
    {
        if (OwnsClient)
        {
            Client.Dispose();
        }
    }

    public Task<ApiResponse<T>> GetAsync<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null, CancellationToken CancellationToken = default)
    {
        return SendAsync<T>(HttpMethod.Get, Path, Options, Configuration ?? GlobalConfiguration.Instance, CancellationToken);
    }

    public ApiResponse<T> Get<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null)
    {
        return GetAsync<T>(Path, Options, Configuration).GetAwaiter().GetResult();
    }

    public Task<ApiResponse<T>> PostAsync<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null, CancellationToken CancellationToken = default)
    {
        return SendAsync<T>(HttpMethod.Post, Path, Options, Configuration ?? GlobalConfiguration.Instance, CancellationToken);
    }

    public ApiResponse<T> Post<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null)
    {
        return PostAsync<T>(Path, Options, Configuration).GetAwaiter().GetResult();
    }

    public Task<ApiResponse<T>> PutAsync<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null, CancellationToken CancellationToken = default)
    {
        return SendAsync<T>(HttpMethod.Put, Path, Options, Configuration ?? GlobalConfiguration.Instance, CancellationToken);
    }

    public ApiResponse<T> Put<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null)
    {
        return PutAsync<T>(Path, Options, Configuration).GetAwaiter().GetResult();
    }

    public Task<ApiResponse<T>> DeleteAsync<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null, CancellationToken CancellationToken = default)
    {
        return SendAsync<T>(HttpMethod.Delete, Path, Options, Configuration ?? GlobalConfiguration.Instance, CancellationToken);
    }

    public ApiResponse<T> Delete<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null)
    {
        return DeleteAsync<T>(Path, Options, Configuration).GetAwaiter().GetResult();
    }

    public Task<ApiResponse<T>> HeadAsync<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null, CancellationToken CancellationToken = default)
    {
        return SendAsync<T>(HttpMethod.Head, Path, Options, Configuration ?? GlobalConfiguration.Instance, CancellationToken);
    }

    public ApiResponse<T> Head<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null)
    {
        return HeadAsync<T>(Path, Options, Configuration).GetAwaiter().GetResult();
    }

    public Task<ApiResponse<T>> OptionsAsync<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null, CancellationToken CancellationToken = default)
    {
        return SendAsync<T>(HttpMethod.Options, Path, Options, Configuration ?? GlobalConfiguration.Instance, CancellationToken);
    }

    public ApiResponse<T> Options<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null)
    {
        return OptionsAsync<T>(Path, Options, Configuration).GetAwaiter().GetResult();
    }

    public Task<ApiResponse<T>> PatchAsync<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null, CancellationToken CancellationToken = default)
    {
        return SendAsync<T>(HttpMethod.Patch, Path, Options, Configuration ?? GlobalConfiguration.Instance, CancellationToken);
    }

    public ApiResponse<T> Patch<T>(string Path, RequestOptions Options, IReadableConfiguration Configuration = null)
    {
        return PatchAsync<T>(Path, Options, Configuration).GetAwaiter().GetResult();
    }
}

