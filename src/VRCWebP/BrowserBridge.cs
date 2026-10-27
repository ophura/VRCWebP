using Microsoft.Win32;
using System.Buffers.Binary;
using System.IO.Pipes;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

namespace VRCWebP;

/// <summary>Connects an explicitly invoked browser extension through native messaging and a user-only pipe.</summary>
internal sealed partial class BrowserBridge : IDisposable, IAsyncDisposable
{
    private const string HostName = "community.vrcwebp.bridge";

    private readonly CancellationTokenSource Lifetime = new();

    private readonly Task Listener;

    private static readonly string PipeName = CreatePipeName();

    private Task? Shutdown;

    private readonly Lock Gate = new();

    private static string CreatePipeName()
    {
        using WindowsIdentity Identity = WindowsIdentity.GetCurrent();

        return "VRCWebP.Session." + Identity.User!.Value;
    }

    internal BrowserBridge(Func<BrowserSession, Task> Accept, string? PipeOverride = null)
    {
        Listener = ListenAsync(Accept, PipeOverride ?? PipeName, Lifetime.Token);
    }

    /// <summary>Exports the WebExtension and registers user-level hosts for Chrome, Brave, Edge and Firefox.</summary>
    internal static string Setup(string? ExportRoot = null, bool Register = true)
    {
        string DirectoryPath = ExportRoot ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VRCWebP", "browser-extension");

        _ = Directory.CreateDirectory(DirectoryPath);

        Assembly Assembly = typeof(BrowserBridge).Assembly;

        foreach (string Name in new[] { "manifest.json", "popup.html", "popup.js" })
        {
            using Stream Resource = Assembly.GetManifestResourceStream("Bridge." + Name) ?? throw new FileNotFoundException("Missing browser bridge resource.");

            using StreamReader Reader = new(Resource);

            WriteText(Path.Combine(DirectoryPath, Name), Reader.ReadToEnd());
        }

        using (Stream Icon = Assembly.GetManifestResourceStream("Bridge.icon.png") ?? throw new FileNotFoundException("Missing extension icon."))
        using (FileStream Destination = File.Create(Path.Combine(DirectoryPath, "icon.png")))
        {
            Icon.CopyTo(Destination);
        }

        foreach (string Name in new[] { "CascadiaCode.ttf", "CascadiaCode-SemiBold.ttf", "CascadiaCode.LICENSE.txt" })
        {
            string DestinationPath = Path.Combine(DirectoryPath, Name);

            if (Name.EndsWith(".txt", StringComparison.Ordinal))
            {
                using Stream Resource = Assembly.GetManifestResourceStream("Payload." + Name)!;

                using StreamReader Reader = new(Resource);

                WriteText(DestinationPath, Reader.ReadToEnd());
            }
            else
            {
                using Stream Resource = Assembly.GetManifestResourceStream("Payload." + Name)!;

                using FileStream Destination = File.Create(DestinationPath);

                Resource.CopyTo(Destination);
            }
        }

        using MemoryStream Buffer = new();

        using (Utf8JsonWriter Writer = new(Buffer, new JsonWriterOptions { Indented = true }))
        {
            Writer.WriteStartObject();

            Writer.WriteString("name", HostName);

            Writer.WriteString("description", "VRCWebP local browser session bridge");

            Writer.WriteString("path", Environment.ProcessPath);

            Writer.WriteString("type", "stdio");

            Writer.WriteStartArray("allowed_origins");

            Writer.WriteStringValue(ChromiumOrigin);

            Writer.WriteEndArray();

            Writer.WriteEndObject();
        }

        string HostManifest = Path.Combine(Path.GetDirectoryName(DirectoryPath)!, "native-host.json");

        WriteText(HostManifest, Encoding.UTF8.GetString(Buffer.ToArray()));

        if (Register)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Google\Chrome\NativeMessagingHosts\" + HostName, "", HostManifest);

            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Edge\NativeMessagingHosts\" + HostName, "", HostManifest);

            Registry.SetValue(@"HKEY_CURRENT_USER\Software\BraveSoftware\Brave-Browser\NativeMessagingHosts\" + HostName, "", HostManifest);
        }

        using MemoryStream FirefoxBuffer = new();

        using (Utf8JsonWriter Writer = new(FirefoxBuffer, new JsonWriterOptions { Indented = true }))
        {
            Writer.WriteStartObject();

            Writer.WriteString("name", HostName);

            Writer.WriteString("description", "VRCWebP local browser session bridge");

            Writer.WriteString("path", Environment.ProcessPath);

            Writer.WriteString("type", "stdio");

            Writer.WriteStartArray("allowed_extensions");

            Writer.WriteStringValue("vrcwebp@local.bridge");

            Writer.WriteEndArray();

            Writer.WriteEndObject();
        }

        string FirefoxHost = Path.Combine(Path.GetDirectoryName(DirectoryPath)!, "native-host-firefox.json");

        WriteText(FirefoxHost, Encoding.UTF8.GetString(FirefoxBuffer.ToArray()));

        if (Register)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Mozilla\NativeMessagingHosts\" + HostName, "", FirefoxHost);
        }

        RefreshFirefoxFolder(DirectoryPath);

        return DirectoryPath;
    }

    /// <summary>Runs as the browser's short-lived native host; no secret is written to stdout as text or logged.</summary>
    internal static async Task<int> RunHostAsync(string? PipeOverride = null)
    {
        using Stream Input = Console.OpenStandardInput();

        using Stream Output = Console.OpenStandardOutput();

        try
        {
            using CancellationTokenSource Timeout = new(TimeSpan.FromSeconds(45));

            using PooledBuffer Payload = await ReadFrameAsync(Input, Timeout.Token);

            using NamedPipeClientStream Pipe = new(".", PipeOverride ?? PipeName, PipeDirection.InOut, PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly);

            await Pipe.ConnectAsync(5000, Timeout.Token);

            RequirePeerProcess(Pipe, false);

            await Pipe.WriteAsync(Payload.Memory, Timeout.Token);

            using PooledBuffer Reply = await ReadFrameAsync(Pipe, Timeout.Token);

            await Output.WriteAsync(Reply.Memory, Timeout.Token);

            await Output.FlushAsync(Timeout.Token);

            return 0;
        }
        catch
        {
            try
            {
                using CancellationTokenSource ReplyTimeout = new(TimeSpan.FromSeconds(2));

                await WriteFrameAsync(Output, Encoding.UTF8.GetBytes("{\"ok\":false,\"message\":\"Open VRCWebP and reconnect from your signed-in browser profile.\"}"), ReplyTimeout.Token);
            }
            catch (IOException)
            {
                // Closing a browser popup may close stdout before the acknowledgement.
            }
            catch (ObjectDisposedException)
            {
                // A browser host shutdown must never open a desktop error dialog.
            }
            catch (OperationCanceledException)
            {
                // A stalled browser must not retain a native-host process indefinitely.
            }

            return 1;
        }
    }

    /// <summary>Accepts one bounded frame at a time, verifies it, then returns a secret-free acknowledgement.</summary>
    private static async Task ListenAsync(Func<BrowserSession, Task> Accept, string Name, CancellationToken CancellationToken)
    {
        while (!CancellationToken.IsCancellationRequested)
        {
            using NamedPipeServerStream Pipe = new(Name, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly);

            try
            {
                await Pipe.WaitForConnectionAsync(CancellationToken);

                RequirePeerProcess(Pipe, true);

                using CancellationTokenSource Timeout = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken);

                Timeout.CancelAfter(TimeSpan.FromSeconds(35));

                using PooledBuffer Payload = await ReadFrameAsync(Pipe, Timeout.Token);

                BrowserSession Session = BrowserSession.Parse(Payload.Memory[4..]);

                await Accept(Session).WaitAsync(Timeout.Token);

                await WriteFrameAsync(Pipe, Encoding.UTF8.GetBytes("{\"ok\":true}"), Timeout.Token);
            }
            catch (Exception) when (!CancellationToken.IsCancellationRequested)
            {
                try
                {
                    using CancellationTokenSource ReplyTimeout = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken);

                    ReplyTimeout.CancelAfter(TimeSpan.FromSeconds(2));

                    await WriteFrameAsync(Pipe, Encoding.UTF8.GetBytes("{\"ok\":false,\"message\":\"Session verification failed or the app is busy. Reconnect from a signed-in VRChat browser profile.\"}"), ReplyTimeout.Token);
                }
                catch (IOException)
                {
                    // The browser popup may have been closed during verification.
                }
                catch (OperationCanceledException)
                {
                    // A disconnected or stalled peer must not prevent shutdown.
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    /// <summary>Reads the native-messaging length prefix before allocating a strictly bounded payload.</summary>
    private static async Task<PooledBuffer> ReadFrameAsync(Stream Stream, CancellationToken CancellationToken)
    {
        PooledBuffer Frame = new(16388);

        try
        {
            await Stream.ReadExactlyAsync(Frame.Memory[..4], CancellationToken);

            int Length = BinaryPrimitives.ReadInt32LittleEndian(Frame.Memory.Span);

            if (Length is < 2 or > 16384)
            {
                throw new InvalidDataException("Invalid native messaging frame.");
            }

            Frame.Length = Length + 4;

            await Stream.ReadExactlyAsync(Frame.Memory[4..], CancellationToken);

            return Frame;
        }
        catch
        {
            Frame.Dispose();

            throw;
        }
    }

    /// <summary>Writes one framed native-messaging response and flushes the pipe.</summary>
    private static async Task WriteFrameAsync(Stream Stream, byte[] Payload, CancellationToken CancellationToken)
    {
        using PooledBuffer Frame = new(Payload.Length + 4);

        BinaryPrimitives.WriteInt32LittleEndian(Frame.Memory.Span, Payload.Length);

        Payload.AsMemory().CopyTo(Frame.Memory[4..]);

        await Stream.WriteAsync(Frame.Memory, CancellationToken);

        await Stream.FlushAsync(CancellationToken);
    }

    /// <summary>Normalizes exported extension text and verifies the saved UTF-8 bytes.</summary>
    private static void WriteText(string Path, string Text)
    {
        string Normalized = Text.Replace("\r\n", "\n").Replace('\r', '\n');

        File.WriteAllText(Path, Normalized, new UTF8Encoding(false));

        byte[] Bytes = File.ReadAllBytes(Path);

        _ = new UTF8Encoding(false, true).GetString(Bytes);

        if (Bytes.Contains((byte)13) || Bytes.AsSpan().StartsWith(new byte[] { 239, 187, 191 }))
        {
            throw new InvalidDataException("Browser bridge text encoding verification failed.");
        }
    }

    public void Dispose()
    {
        _ = StopAsync();
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(StopAsync());
    }

    private Task StopAsync()
    {
        lock (Gate)
        {
            return Shutdown ??= StopCoreAsync();
        }
    }

    private async Task StopCoreAsync()
    {
        Lifetime.Cancel();

        try
        {
            await Listener.ConfigureAwait(false);
        }
        finally
        {
            Lifetime.Dispose();
        }
    }
}
