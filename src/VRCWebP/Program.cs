using Avalonia;
using System.Runtime.InteropServices;
using System.Text;

namespace VRCWebP;

/// <summary>Bootstraps the application and its native browser messaging host.</summary>
internal static partial class Program
{

    [STAThread]
    private static int Main(string[] Arguments)
    {

        try
        {
            if (Arguments is ["--setup-browser"])
            {
                _ = BrowserBridge.Setup();

                return 0;
            }


            if (Arguments.Any(BrowserBridge.IsHostOrigin))
            {
                return BrowserBridge.RunHostAsync().GetAwaiter().GetResult();
            }

            if (Arguments.Length is not 0)
            {
                return 2;
            }

            Payload.InitializeGraphics();

            int ExitCode = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new Avalonia.Media.FontManagerOptions
                {
                    DefaultFamilyName = "avares://VRCWebP/Assets/Fonts#Cascadia Code"
                })
                .With(new Win32PlatformOptions
                {
                    RenderingMode = [Win32RenderingMode.Wgl, Win32RenderingMode.Software]
                })
                .StartWithClassicDesktopLifetime(Arguments);


            return Environment.ExitCode is 0 ? ExitCode : Environment.ExitCode;
        }
        catch (Exception Error)
        {
            string Log = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VRCWebP", "last-error.txt");

            Directory.CreateDirectory(Path.GetDirectoryName(Log)!);

            string Detail = $"{Error.GetType().Name}\nHResult: 0x{Error.HResult:X8}\n";

            if (Error is ServerRequestException Server)
            {
                Detail += $"HTTP: {(int?)Server.StatusCode}\nServer explanation: {Server.SafeExplanation}\n";
            }

            File.WriteAllText(Log, Detail, new UTF8Encoding(false));

            _ = MessageBoxW(0, Error.Message + "\n\n" + Log, "", 0);

            return 1;
        }
    }

    [LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16)]
    private static partial int MessageBoxW(nint Window, string Text, string Caption, uint Type);
}
