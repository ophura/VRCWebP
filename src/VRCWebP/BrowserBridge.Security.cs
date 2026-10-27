using Microsoft.Win32.SafeHandles;
using System.Buffers;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;

namespace VRCWebP;

internal sealed partial class BrowserBridge
{
    private static readonly string ChromiumOrigin = CreateChromiumOrigin();

    internal static bool IsHostOrigin(string Origin)
    {
        return Origin is "vrcwebp@local.bridge" || string.Equals(Origin, ChromiumOrigin, StringComparison.Ordinal);
    }

    /// <summary>Derives the trusted Chromium origin from the embedded manifest, never an exported file.</summary>
    private static string CreateChromiumOrigin()
    {
        using Stream Resource = typeof(BrowserBridge).Assembly.GetManifestResourceStream("Bridge.manifest.json")!;

        using JsonDocument Manifest = JsonDocument.Parse(Resource);

        Span<byte> Key = stackalloc byte[1024];

        if (!Convert.TryFromBase64String(Manifest.RootElement.GetProperty("key").GetString()!, Key, out int Length))
        {
            throw new InvalidDataException("Invalid embedded browser key.");
        }

        Span<byte> Digest = stackalloc byte[32];

        _ = SHA256.HashData(Key[..Length], Digest);

        Span<char> Identifier = stackalloc char[32];

        for (int Index = 0; Index < 16; ++Index)
        {
            Identifier[Index * 2] = (char)('a' + (Digest[Index] >> 4));

            Identifier[(Index * 2) + 1] = (char)('a' + (Digest[Index] & 15));
        }

        return "chrome-extension://" + new string(Identifier) + "/";
    }

    /// <summary>Refreshes previously installed bridge files when a new application is launched.</summary>
    internal static void RefreshInstalled()
    {
        string Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VRCWebP", "browser-extension");

        if (Directory.Exists(Folder))
        {
            _ = Setup();
        }
    }

    /// <summary>Returns the selected browser's refreshed export without duplicating source assets.</summary>
    internal static string ExtensionFolder(string Root, bool Firefox)
    {
        return Firefox ? Path.Combine(Root, "firefox") : Root;
    }

    /// <summary>Refreshes legacy Firefox exports using hard links, with a portable copy fallback.</summary>
    private static void RefreshFirefoxFolder(string Root)
    {
        string Folder = ExtensionFolder(Root, true);

        _ = Directory.CreateDirectory(Folder);

        if ((File.GetAttributes(Folder) & FileAttributes.ReparsePoint) != 0)
        {
            throw new IOException("The Firefox export folder must be a regular directory.");
        }

        foreach (string Source in Directory.EnumerateFiles(Root))
        {
            string Destination = Path.Combine(Folder, Path.GetFileName(Source));

            File.Delete(Destination);

            if (Path.GetFileName(Source) is "manifest.json")
            {
                using JsonDocument Manifest = JsonDocument.Parse(File.ReadAllBytes(Source));

                using MemoryStream Buffer = new();

                using (Utf8JsonWriter Writer = new(Buffer, new JsonWriterOptions { Indented = true }))
                {
                    Writer.WriteStartObject();

                    foreach (JsonProperty Property in Manifest.RootElement.EnumerateObject())
                    {
                        if (Property.Name is not "key")
                        {
                            Property.WriteTo(Writer);
                        }
                    }

                    Writer.WriteEndObject();
                }

                WriteText(Destination, System.Text.Encoding.UTF8.GetString(Buffer.ToArray()));

                continue;
            }

            if (!CreateHardLinkW(Destination, Source, 0))
            {
                File.Copy(Source, Destination);
            }
        }
    }

    /// <summary>Refuses to send or accept session frames through an unrelated local process.</summary>
    private static unsafe void RequirePeerProcess(PipeStream Pipe, bool Server)
    {
        bool Found = Server
            ? GetNamedPipeClientProcessId(Pipe.SafePipeHandle, out uint ProcessId)
            : GetNamedPipeServerProcessId(Pipe.SafePipeHandle, out ProcessId);

        if (!Found)
        {
            throw new UnauthorizedAccessException("Unable to verify the local bridge peer.");
        }

        using SafeProcessHandle Process = OpenProcess(0x1000, false, ProcessId);

        // QueryFullProcessImageNameW borrows this buffer only for the synchronous call.
        // PROCESS_QUERY_LIMITED_INFORMATION verifies image identity without opening memory.
        char[] PathBuffer = ArrayPool<char>.Shared.Rent(32768);

        try
        {
            Span<char> ImagePath = PathBuffer.AsSpan(0, 32768);

            uint Length = (uint)ImagePath.Length;

            fixed (char* Buffer = ImagePath)
            {
                if (Process.IsInvalid || !QueryFullProcessImageNameW(Process, 0, Buffer, ref Length)
                    || !ImagePath[..(int)Length].Equals(Environment.ProcessPath.AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedAccessException("The local bridge peer is not this application.");
                }
            }
        }
        finally
        {
            ArrayPool<char>.Shared.Return(PathBuffer, clearArray: true);
        }
    }

    [LibraryImport("kernel32.dll", EntryPoint = "CreateHardLinkW", StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CreateHardLinkW(string Filename, string ExistingFilename, nint SecurityAttributes);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetNamedPipeClientProcessId(SafePipeHandle Pipe, out uint ProcessId);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetNamedPipeServerProcessId(SafePipeHandle Pipe, out uint ProcessId);

    [LibraryImport("kernel32.dll")]
    private static partial SafeProcessHandle OpenProcess(uint Access, [MarshalAs(UnmanagedType.Bool)] bool Inherit, uint ProcessId);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static unsafe partial bool QueryFullProcessImageNameW(SafeProcessHandle Process, uint Flags, char* Filename, ref uint Size);
}
