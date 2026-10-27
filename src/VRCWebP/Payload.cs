using System.ComponentModel;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace VRCWebP;

/// <summary>Extracts embedded native dependencies into a versioned, hash-verified per-user cache.</summary>
internal static partial class Payload
{
    private static readonly Lock Gate = new();

    private static readonly Dictionary<string, string> VerifiedPaths = new(StringComparer.Ordinal);

    private static bool GraphicsLoaded;

    /// <summary>Loads the two graphics dependencies before Avalonia starts.</summary>
    internal static void InitializeGraphics()
    {
        lock (Gate)
        {
            if (GraphicsLoaded)
            {
                return;
            }

            // These libraries stay loaded for the process lifetime, including Avalonia's finalizers.
            foreach (string Name in new[] { "libSkiaSharp.dll", "libHarfBuzzSharp.dll" })
            {
                if (LoadLibraryW(Get(Name)) is 0)
                {
                    throw new Win32Exception(Marshal.GetLastPInvokeError(), "Unable to load a graphics dependency.");
                }
            }

            GraphicsLoaded = true;
        }
    }

    /// <summary>Returns a verified native payload, extracting atomically on first use.</summary>
    internal static string Get(string Name)
    {
        lock (Gate)
        {
            if (VerifiedPaths.TryGetValue(Name, out string? Verified) && File.Exists(Verified))
            {
                return Verified;
            }

            Assembly Assembly = typeof(Payload).Assembly;

            using Stream HashStream = Assembly.GetManifestResourceStream("Payload." + Name + ".sha256") ?? throw new FileNotFoundException("Missing payload checksum: " + Name);

            using StreamReader Reader = new(HashStream);

            string Hash = Reader.ReadToEnd().Trim();

            string DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VRCWebP", "runtime", Hash[..16]);

            _ = Directory.CreateDirectory(DirectoryPath);

            string Destination = Path.Combine(DirectoryPath, Name);

            if (File.Exists(Destination) && HasHash(Destination, Hash))
            {
                VerifiedPaths[Name] = Destination;

                return Destination;
            }

            string Temporary = Destination + "." + Guid.NewGuid().ToString("N") + ".tmp";

            try
            {
                using (Stream Resource = Assembly.GetManifestResourceStream("Payload." + Name + ".gz") ?? throw new FileNotFoundException("Missing native payload: " + Name))
                using (GZipStream Compressed = new(Resource, CompressionMode.Decompress))
                using (FileStream Output = File.Create(Temporary))
                {
                    Compressed.CopyTo(Output);
                }

                if (!HasHash(Temporary, Hash))
                {
                    throw new InvalidDataException("Native dependency checksum mismatch: " + Name);
                }

                File.Move(Temporary, Destination, true);
            }
            finally
            {
                File.Delete(Temporary);
            }

            VerifiedPaths[Name] = Destination;

            return Destination;
        }
    }

    private static bool HasHash(string Path, string Expected)
    {
        using FileStream Input = File.OpenRead(Path);

        return Convert.ToHexString(SHA256.HashData(Input)).Equals(Expected, StringComparison.OrdinalIgnoreCase);
    }

    [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    private static partial nint LoadLibraryW(string Filename);
}
