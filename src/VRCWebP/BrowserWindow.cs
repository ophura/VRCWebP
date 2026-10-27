using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Layout;
using Avalonia.Media;
using Microsoft.Win32;
using System.Diagnostics;

namespace VRCWebP;

/// <summary>Explains the one-time browser bridge installation without requesting credentials or cookie text.</summary>
internal sealed class BrowserWindow : Window
{
    internal BrowserWindow(bool FirefoxInstructions = false)
    {
        Title = "Connect browser";

        CanResize = false;

        WindowStartupLocation = WindowStartupLocation.CenterOwner;

        WindowChrome.Configure(this, 640, 590);

        ComboBox Browser = new MotionChoice()
        {
            ItemsSource = new[] { "Chrome", "Brave", "Firefox", "Edge" },

            SelectedIndex = FirefoxInstructions ? 2 : 0
        };

        ToolTip.SetTip(Browser, "Browser containing your signed-in VRChat profile");

        Browser.ContainerPrepared += (_, Event) => ToolTip.SetTip(Event.Container, "Set up the bridge in " + Browser.Items[Event.Index]);

        TextBlock Steps = new()
        {
            TextWrapping = TextWrapping.Wrap,

            Foreground = Brush.Parse("#C2C2C2"),

            LineHeight = 24
        };

        TextBlock Notice = new()
        {
            TextWrapping = TextWrapping.Wrap,

            Foreground = Brush.Parse("#BFBFBF"),

            FontSize = 12
        };

        TextBlock Status = new()
        {
            TextWrapping = TextWrapping.Wrap,

            Foreground = Brush.Parse("#AEAEAE"),

            FontSize = 12
        };

        string? ExtensionPath = null;

        void Update()
        {
            bool Firefox = Browser.SelectedItem as string is "Firefox";

            Steps.Text = Firefox
                ? "1   Open about:debugging → This Firefox.\n2   Load manifest.json from the copied folder.\n3   Open your signed-in VRChat tab → extension → Connect."
                : "1   Open Extensions and enable Developer mode.\n2   Load the copied folder.\n3   Open your signed-in VRChat tab → extension → Connect.";

            UiMotion.SetText(Notice, Firefox ? "Temporary add-ons reset when Firefox restarts. Permanent installation requires Mozilla signing." : "Use the browser profile where you already use VRChat.");
        }

        Browser.SelectionChanged += (_, _) => Update();

        Update();

        Button Open = Icons.Action("link", "Open extensions", true);

        Button Copy = Icons.Action("copy", "Copy folder");

        Open.Click += async (_, _) =>
        {
            try
            {
                ExtensionPath = await Task.Run(() => BrowserBridge.Setup());

                Launch(Browser.SelectedItem as string ?? "Chrome");

                UiMotion.SetText(Status, "Bridge ready. Load the extension, then press Connect in its popup.");

                Status.Foreground = Brush.Parse("#CACACA");
            }
            catch (Exception Error)
            {
                UiMotion.SetText(Status, Error.Message);
            }
        };

        Copy.Click += async (_, _) =>
        {
            try
            {
                ExtensionPath ??= await Task.Run(() => BrowserBridge.Setup());

                string Path = BrowserBridge.ExtensionFolder(ExtensionPath, Browser.SelectedItem as string is "Firefox");

                if (Clipboard is not null)
                {
                    await Clipboard.SetTextAsync(Path);

                    UiMotion.SetText(Status, "Extension folder copied.");
                }
            }
            catch (Exception Error)
            {
                UiMotion.SetText(Status, Error.Message);
            }
        };

        StackPanel Body = new()
        {
            Margin = new Thickness(28, 12, 28, 24),

            Spacing = 18,

            Children =
            {
                new TextBlock
                {
                    Text = "Connect your browser",

                    FontSize = 24,

                    FontWeight = FontWeight.SemiBold
                },

                Browser,

                Steps,

                new StackPanel
                {
                    Orientation = Orientation.Horizontal,

                    Spacing = 10,

                    Children = { Open, Copy }
                },

                Notice,

                new TextBlock
                {
                    Text = "Only VRChat’s two session cookies are shared locally. Saved sessions use Windows DPAPI. Disconnect erases the saved session.",

                    Foreground = Brush.Parse("#949494"),

                    TextWrapping = TextWrapping.Wrap,

                    FontSize = 12
                },

                Status
            }
        };

        Grid Layout = new()
        {
            RowDefinitions = new RowDefinitions("64,*")
        };

        Layout.Children.Add(WindowChrome.Header(this));

        Grid.SetRow(Body, 1);

        Layout.Children.Add(Body);

        Content = Layout;
    }

    /// <summary>Opens the installed browser's extension manager without creating another profile.</summary>
    private static void Launch(string Browser)
    {
        string Filename = Browser switch
        {
            "Brave" => "brave.exe",
            "Firefox" => "firefox.exe",
            "Edge" => "msedge.exe",
            _ => "chrome.exe"
        };

        string? Registered = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + Filename, "", null) as string
            ?? Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + Filename, "", null) as string;

        string Relative = Browser switch
        {
            "Brave" => @"BraveSoftware\Brave-Browser\Application\brave.exe",
            "Firefox" => @"Mozilla Firefox\firefox.exe",
            "Edge" => @"Microsoft\Edge\Application\msedge.exe",
            _ => @"Google\Chrome\Application\chrome.exe"
        };

        string? Path = Registered?.Trim('"');

        if (Path is not null && !File.Exists(Path))
        {
            Path = null;
        }

        foreach (string Root in new[] { Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) })
        {
            string Candidate = System.IO.Path.Combine(Root, Relative);

            if (File.Exists(Candidate))
            {
                Path ??= Candidate;
            }
        }

        if (Path is null || !File.Exists(Path))
        {
            throw new FileNotFoundException("Open your browser’s extension manager, then use Copy folder below.");
        }

        ProcessStartInfo Start = new(Path)
        {
            UseShellExecute = true
        };

        Start.ArgumentList.Add(Browser switch
        {
            "Brave" => "brave://extensions/",
            "Firefox" => "about:debugging#/runtime/this-firefox",
            "Edge" => "edge://extensions/",
            _ => "chrome://extensions/"
        });

        using Process? BrowserProcess = Process.Start(Start);
    }
}
