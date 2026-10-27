using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace VRCWebP;

/// <summary>Contains only the two VRChat session cookies explicitly shared by the browser extension.</summary>
internal sealed record BrowserSession(string Auth, string? TwoFactorAuth, DateTimeOffset Expires)
{
    /// <summary>Accepts bounded cookie tokens and an explicit expiration without exposing their values in errors.</summary>
    internal static BrowserSession Parse(ReadOnlyMemory<byte> Data)
    {
        using JsonDocument Document = JsonDocument.Parse(Data);

        JsonElement Root = Document.RootElement;

        string Auth = Root.GetProperty("auth").GetString() ?? "";

        string? TwoFactorAuth = Root.TryGetProperty("twoFactorAuth", out JsonElement Factor) ? Factor.GetString() : null;

        DateTimeOffset Expires = DateTimeOffset.FromUnixTimeSeconds(Root.GetProperty("expires").GetInt64());

        if (Auth.Length is < 8 or > 4096 || TwoFactorAuth?.Length > 4096 || !ValidToken(Auth) || (TwoFactorAuth is not null && !ValidToken(TwoFactorAuth)))
        {
            throw new InvalidDataException("The browser supplied an invalid VRChat session.");
        }

        return new BrowserSession(Auth, TwoFactorAuth, Expires);
    }

    /// <summary>Produces a transient UTF-8 buffer that callers must erase after protection or transport.</summary>
    internal byte[] Encode()
    {
        using MemoryStream Buffer = new();

        try
        {
            using (Utf8JsonWriter Writer = new(Buffer))
            {
                Writer.WriteStartObject();

                Writer.WriteString("auth", Auth);

                Writer.WriteString("twoFactorAuth", TwoFactorAuth);

                Writer.WriteNumber("expires", Expires.ToUnixTimeSeconds());

                Writer.WriteEndObject();
            }

            return Buffer.ToArray();
        }
        finally
        {
            CryptographicOperations.ZeroMemory(Buffer.GetBuffer().AsSpan());
        }
    }

    private static bool ValidToken(string Token) => Token.All(Character => char.IsAsciiLetterOrDigit(Character) || Character is '-' or '_' or '.' or '=');
}

/// <summary>Stores session data as a Windows DPAPI blob bound to the current Windows user.</summary>
internal static class SessionVault
{
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("VRCWebP.browser-session.v1");

    private static readonly string Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VRCWebP", "session.bin");

    /// <summary>Protects the session before any disk write and replaces the prior blob atomically.</summary>
    internal static void Save(BrowserSession Session)
    {
        byte[] Plaintext = Session.Encode();

        try
        {
            byte[] Protected = ProtectedData.Protect(Plaintext, Entropy, DataProtectionScope.CurrentUser);

            _ = Directory.CreateDirectory(Path.GetDirectoryName(Location)!);

            string Temporary = Location + ".tmp";

            File.WriteAllBytes(Temporary, Protected);

            File.Move(Temporary, Location, true);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(Plaintext);
        }
    }

    /// <summary>Decrypts for this Windows user and removes expired sessions.</summary>
    internal static BrowserSession? Load()
    {
        if (!File.Exists(Location))
        {
            return null;
        }

        byte[] Plaintext = ProtectedData.Unprotect(File.ReadAllBytes(Location), Entropy, DataProtectionScope.CurrentUser);

        try
        {
            BrowserSession Session = BrowserSession.Parse(Plaintext);

            if (Session.Expires <= DateTimeOffset.UtcNow)
            {
                Forget();

                return null;
            }

            return Session;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(Plaintext);
        }
    }

    internal static void Forget() => File.Delete(Location);
}
