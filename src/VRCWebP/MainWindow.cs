using Avalonia;
using Avalonia.Animation;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace VRCWebP;

/// <summary>Coordinates media editing, animated preview, reusable uploads and optional publication.</summary>
internal sealed partial class MainWindow : Window
{
    private readonly MediaService Media = new();

    private VRChatService Api = new();

    private readonly BrowserBridge Bridge;

    private readonly CancellationTokenSource Lifetime = new();

    private CancellationTokenSource? Operation;

    private CancellationTokenSource? PreviewOperation;

    private readonly SemaphoreSlim WorkGate = new(1, 1);

    private Task? ForegroundTask;

    private Task? PreviewTask;

    private Task? SessionTask;

    private bool ClosingRequested;

    private bool ResourcesReleased;

    private readonly DispatcherTimer Playback = new()
    {
        Interval = TimeSpan.FromSeconds(1d / 12)
    };

    private readonly DispatcherTimer Debounce = new()
    {
        Interval = TimeSpan.FromMilliseconds(550)
    };

    private readonly List<Bitmap> Frames = [];

    private int FrameIndex;

    private bool Busy;

    private bool Updating;

    private string? InputPath;

    private MediaInfo? InputInfo;

    private ConversionResult? Result;

    private UploadResult? Uploaded;

    private readonly FramingPreview PreviewImage = new()
    {
        Margin = new Thickness(0)
    };

    private readonly TextBlock Filename = Label("", 18);

    private readonly TextBlock SourceDetails = Label("", 12, true);

    private readonly TextBlock Status = Label("Ready", 12, true);

    private readonly TextBlock OutputDetails = Label("", 12, true);

    private readonly TextBlock CloudStatus = Label("Connect your browser to upload.", 12, true);

    private readonly TextBlock ConnectedAccount = Label("Browser not connected", 11, true);

    private readonly TextBlock UploadCaption = Label("Upload to (10,000,000 maximum bytes)", 11, true);

    private readonly TextBlock CreatedFileDetails = Label("", 11);

    private string? UploadNotice;

    private (string? UserId, string? DisplayName, bool Known, bool Gallery, bool Icon, UploadDestination Destination, long? Bytes, bool Working, bool Current, string? Notice)? UploadAccessState;

    private readonly TextBlock PreviewDetails = Label("Preview", 12, true);

    private readonly ProgressBar Progress = new()
    {
        Minimum = 0,

        Maximum = 100,

        Value = 0,

        Width = 100,

        MinWidth = 100,

        VerticalAlignment = VerticalAlignment.Center,

        ShowProgressText = false,

        Transitions =
        [
            new DoubleTransition
            {
                Property = ProgressBar.ValueProperty,

                Duration = UiMotion.Duration
            }
        ]
    };

    private readonly NumericUpDown CanvasWidth = Number(512, 16, 4096, "Width in pixels");

    private readonly NumericUpDown CanvasHeight = Number(512, 16, 4096, "Height in pixels");

    private readonly NumericUpDown Start = Number(0, 0, 86400, "Start time in seconds", 0.1m);

    private readonly NumericUpDown Duration = Number(6, 0.1m, 60, "Clip duration in seconds", 0.1m);

    private readonly NumericUpDown Loops = Number(0, 0, 65535, "Playback count; zero loops forever");

    private readonly NumericUpDown PreviewLength = Number(4, 0.1m, 60, "Editing preview duration in seconds, up to the clip length.\nEncoded playback uses the full created file at its original resolution and cadence.", 0.1m);

    private readonly Slider FrameRate = new()
    {
        Height = 40,

        Minimum = 1,

        Maximum = 60,

        Value = 24,

        TickFrequency = 1,

        IsSnapToTickEnabled = true
    };

    private readonly Slider Compression = new()
    {
        Height = 40,

        Minimum = 0,

        Maximum = 6,

        Value = 0,

        TickFrequency = 1,

        IsSnapToTickEnabled = true
    };

    private readonly ComboBox Fit = Choice("Contain", "Fill");

    private readonly ComboBox CompressionMode = Choice("Lossless", "Lossy");

    private readonly TextBlock CompressionValue = Label("0 / 6", 12, true);

    private double LosslessEffort;

    private double LossyQuality = 80;

    private readonly TextBox GalleryAvatar = new()
    {
        PlaceholderText = "avtr_…",

        MaxLength = 41
    };

    private readonly Button CheckGallery = Icons.TextAction("Check");

    private Control? GalleryTargetField;

    private Control? ExistingImageField;

    private AvatarGalleryState? CheckedGallery;

    private readonly ComboBox UploadLocation = Choice("Original file", "Photo gallery", "User icon", "Avatar gallery");

    private readonly ComboBoxItem[] UploadDestinations =
    [
        new()
        {
            Content = "Original file"
        },
        new()
        {
            Content = "Photo gallery"
        },
        new()
        {
            Content = "User icon"
        },
        new()
        {
            Content = "Avatar gallery"
        }
    ];

    private readonly ComboBox Preset = Choice([.. CanvasPreset.All.Select(Preset => Preset.Label)]);

    private readonly ComboBox Purpose = Choice("Group banner", "Group icon", "Avatar thumbnail");

    private readonly ComboBoxItem[] ImagePurposes =
    [
        new()
        {
            Content = "Group banner"
        },
        new()
        {
            Content = "Group icon"
        },
        new()
        {
            Content = "Avatar thumbnail"
        },
        new()
        {
            Content = "Profile icon"
        },
        new()
        {
            Content = "Profile banner"
        },
        new()
        {
            Content = "Profile icon from avatar"
        },
        new()
        {
            Content = "Profile banner from avatar"
        }
    ];

    private readonly TextBlock ProfileTarget = new()
    {
        Text = "Your connected profile",

        FontSize = 12,

        TextTrimming = TextTrimming.CharacterEllipsis,

        MinHeight = 38,

        VerticalAlignment = VerticalAlignment.Center,

        IsVisible = false
    };

    private readonly TextBlock TargetCaption = Label("Target ID", 11, true);

    private readonly TextBox ExistingImage = new()
    {
        PlaceholderText = "file_ ID or VRChat file URL",

        MaxLength = 256
    };

    private readonly Button Reuse = Icons.TextAction("Reuse");

    private readonly Button ManageFiles = Icons.Button("folder", "Manage uploaded images");

    private readonly TextBox Target = new()
    {
        PlaceholderText = "grp_…",

        MaxLength = 64
    };

    private readonly TextBlock FileId = new()
    {
        TextTrimming = TextTrimming.CharacterEllipsis,

        VerticalAlignment = VerticalAlignment.Center,

        FontSize = 11
    };

    private readonly Button Browse = Icons.Button("add", "Open media: Ctrl+O", true);

    private readonly Button Refresh = Icons.Button("refresh", "Refresh preview");

    private readonly Button Play = Icons.Button("pause", "Pause / play preview: Space");

    private readonly Control PlayIcon = Icons.Make("play");

    private readonly Control PauseIcon = Icons.Make("pause");

    private readonly Dictionary<string, Control> SurfaceGlyphs = new(StringComparer.Ordinal);

    private readonly Button Convert = Icons.TextAction("Create WebP");

    private readonly Button Save = Icons.Button("download", "Save WebP: Ctrl+S");

    private readonly Button Upload = Icons.Action("upload", "Upload", true);

    private readonly Button Apply = Icons.Button("check", "Apply uploaded image to the selected target");

    private readonly Button CopyId = Icons.Button("copy", "Copy file ID");

    private readonly Button CopyUrl = Icons.Button("link", "Copy reusable file URL");

    private MotionExpander ImageUse = null!;

    private Grid RecoveryLinks = null!;

    private readonly Button Account = Icons.Action("user", "Connect browser");

    private readonly Button Cancel = Icons.Button("close", "Cancel current operation");

    private readonly StackPanel SettingsPanel = new()
    {
        Spacing = 10,

        VerticalAlignment = VerticalAlignment.Top
    };

    private readonly TabControl InspectorTabs = new();

    private Grid LayoutRoot = null!;

    private Grid BodyLayout = null!;

    private Grid CloudLayout = null!;

    private StackPanel CloudFields = null!;

    private Grid ImageUseFields = null!;

    private Grid CanvasLayout = null!;

    private Grid SourceHeader = null!;

    private Border InspectorCard = null!;

    private Grid InspectorOverlay = null!;

    private Grid ExportActions = null!;

    private Grid ExportLayout = null!;

    private readonly TextBlock PresetCaption = Label("Canvas", 11, true);

    private Control[] StandardSettings = [];

    private readonly Grid CompactTop = new()
    {
        ColumnDefinitions = new ColumnDefinitions("*,*,*,*"),

        ColumnSpacing = 10
    };

    private readonly Grid CompactBottom = new()
    {
        ColumnDefinitions = new ColumnDefinitions("*,*,*"),

        ColumnSpacing = 10
    };

    private bool CompactLayout;

    private readonly StackPanel Empty = new()
    {
        HorizontalAlignment = HorizontalAlignment.Center,

        VerticalAlignment = VerticalAlignment.Center,

        Spacing = 18
    };

    private readonly StackPanel UploadedPanel = new()
    {
        Spacing = 12,

        IsVisible = false
    };

    internal MainWindow()
    {
        Title = "";

        MinWidth = 1120;

        MinHeight = 820;

        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        WindowChrome.Configure(this, 1200, 860);

        Content = BuildLayout();

        SizeChanged += (_, _) => AdaptLayout();

        Filename.TextTrimming = TextTrimming.CharacterEllipsis;

        SourceDetails.TextTrimming = TextTrimming.CharacterEllipsis;

        Status.MaxLines = 1;

        Status.TextTrimming = TextTrimming.CharacterEllipsis;

        Status.Bind(ToolTip.TipProperty, Status.GetObservable(TextBlock.TextProperty));

        Status.Classes.Add("announcement");

        CloudStatus.Classes.Add("announcement");

        AutomationProperties.SetName(Progress, "Operation progress");

        ToolTip.SetTip(Progress, "0%");

        OutputDetails.TextWrapping = TextWrapping.Wrap;

        OutputDetails.MaxLines = 2;

        ToolTip.SetTip(Preset, CanvasPreset.All[0].Description);

        ToolTip.SetTip(Fit, "Resize to fit or fill canvas");

        ToolTip.SetTip(UploadLocation, "Choose an upload destination. Hover unavailable options to see what is required.");

        UploadLocation.ContainerPrepared += (_, Event) =>
        {
            ToolTip.SetTip(Event.Container, UploadDestinationDescription((UploadDestination)Event.Index));

            ToolTip.SetShowOnDisabled(Event.Container, true);
        };

        ToolTip.SetTip(Purpose, "Choose image use");

        ToolTip.SetTip(Target, "Target ID");

        ToolTip.SetTip(FileId, "Reusable file ID");

        Preset.ContainerPrepared += (_, Event) => ToolTip.SetTip(Event.Container, CanvasPreset.All[Event.Index].Description);

        Fit.ContainerPrepared += (_, Event) => ToolTip.SetTip(Event.Container, Event.Index is 0
            ? "Keep the entire image; add transparent margins where needed."
            : "Fill the canvas; crop overflow without stretching pixels.");

        Purpose.ContainerPrepared += (_, Event) =>
        {
            ToolTip.SetTip(Event.Container, ImagePurposeDescription(Event.Index));

            ToolTip.SetShowOnDisabled(Event.Container, true);
        };

        ToolTip.SetTip(ExistingImage, "Enter a file_ ID or an HTTPS VRChat original-file URL.\nA versioned URL keeps that image version.");

        AutomationProperties.SetName(ExistingImage, "Existing VRChat image ID or URL");

        ExistingImage.PropertyChanged += (_, Event) =>
        {
            if (Event.Property == TextBox.TextProperty)
            {
                UpdateButtons();
            }
        };

        Target.PropertyChanged += (_, Event) =>
        {
            if (Event.Property == TextBox.TextProperty)
            {
                UpdateButtons();
            }
        };

        ExistingImage.KeyDown += async (_, Event) =>
        {
            if (Event.Key is Key.Enter && Reuse.IsEnabled)
            {
                Event.Handled = true;

                await RunWorkAsync(ReuseAsync);
            }
        };

        Browse.Click += async (_, _) => await BrowseAsync();

        Refresh.Click += async (_, _) => await RefreshPreviewAsync();

        Play.Click += (_, _) => TogglePlayback();

        Convert.Click += async (_, _) => await RunWorkAsync(ConvertAsync);

        Save.Click += async (_, _) => await RunWorkAsync(SaveAsync);

        Upload.Click += async (_, _) => await RunWorkAsync(UploadAsync);

        Apply.Click += async (_, _) => await RunWorkAsync(ApplyAsync);

        Reuse.Click += async (_, _) => await RunWorkAsync(ReuseAsync);

        ManageFiles.Click += async (_, _) => await RunWorkAsync(ManageImagesAsync);

        Cancel.Click += async (_, _) =>
        {
            UiMotion.SetText(Status, "Cancelling…");

            if (Operation is not null)
            {
                await Operation.CancelAsync();
            }

            if (EncodedSeekOperation is not null)
            {
                await EncodedSeekOperation.CancelAsync();

                EncodedPlayback?.CancelSeek();
            }

            if (PreviewOperation is not null)
            {
                UserCancelledPreview = true;

                await PreviewOperation.CancelAsync();
            }
        };

        Account.Click += async (_, _) => await ShowBrowserAsync();

        foreach (NumericUpDown Control in new[] { CanvasWidth, CanvasHeight, Start, Duration, Loops })
        {
            Control.PropertyChanged += (_, Event) =>
            {
                if (Event.Property == NumericUpDown.TextProperty && !Updating)
                {
                    UpdateButtons();
                }
            };

            Control.ValueChanged += (_, _) =>
            {
                if (!Updating && (Control == CanvasWidth || Control == CanvasHeight))
                {
                    Updating = true;

                    Preset.SelectedIndex = CanvasPreset.All.Length - 1;

                    Updating = false;
                }

                SettingsChanged();
            };
        }

        PreviewLength.ValueChanged += (_, _) =>
        {
            if (!Updating && !Busy)
            {
                Debounce.Stop();

                Debounce.Start();
            }
        };

        FrameRate.ValueChanged += (_, _) => SettingsChanged();

        Compression.ValueChanged += (_, _) => SettingsChanged();

        Compression.ValueChanged += (_, _) => UpdateCompressionValue();

        CompressionMode.SelectionChanged += (_, _) =>
        {
            bool PreviousUpdating = Updating;

            Updating = true;

            if (Compression.Maximum is 6)
            {
                LosslessEffort = Compression.Value;
            }
            else
            {
                LossyQuality = Compression.Value;
            }

            Compression.Maximum = CompressionMode.SelectedIndex is 0 ? 6 : 100;

            Compression.Value = CompressionMode.SelectedIndex is 0 ? LosslessEffort : LossyQuality;

            Updating = PreviousUpdating;

            UpdateCompressionValue();

            SettingsChanged();
        };

        PreviewImage.FramingChanged += SettingsChanged;

        ToolTip.SetTip(PreviewImage, "Inside the light frame is the exported picture; dimmed pixels are excluded. Drag to position the image. Double-click to center it. Checkerboard areas remain transparent.");

        Fit.SelectionChanged += (_, _) => SettingsChanged();

        CheckGallery.Click += async (_, _) => await RunWorkAsync(async Token =>
        {
            string AvatarId = GalleryAvatar.Text ?? "";

            CheckedGallery = await Task.Run(() => Api.ReadAvatarGalleryAsync(AvatarId, Token), Token);

            UploadAccessState = null;

            UiMotion.SetText(Status, $"Avatar gallery: {CheckedGallery.FileIds.Count} of 6 slots used");
        });

        GalleryAvatar.PropertyChanged += (_, Event) =>
        {
            if (Event.Property != TextBox.TextProperty)
            {
                return;
            }

            CheckedGallery = null;

            UploadAccessState = null;

            UpdateButtons();
        };

        ToolTip.SetTip(GalleryAvatar, "The avtr_ ID of an avatar you own. Upload adds a gallery image and keeps its main thumbnail unchanged.");

        AutomationProperties.SetName(GalleryAvatar, "Avatar gallery target ID");

        UploadLocation.SelectionChanged += (_, _) =>
        {
            UploadNotice = null;

            UpdateButtons();
        };

        Preset.SelectionChanged += (_, _) => SetPreset();

        Purpose.SelectionChanged += (_, _) =>
        {
            Target.PlaceholderText = Purpose.SelectedIndex is 2 ? "avtr_…" : "grp_…";

            UpdateButtons();
        };

        Debounce.Tick += async (_, _) =>
        {
            Debounce.Stop();

            await RefreshPreviewAsync();
        };

        Playback.Tick += (_, _) =>
        {
            if (EncodedPlayback is not null)
            {
                SetPlaybackPosition(EncodedPlayback.Position);
            }
            else if (Frames.Count > 0)
            {
                double Position = (PlaybackOffset + System.Diagnostics.Stopwatch.GetElapsedTime(PlaybackStarted).TotalSeconds) % DecodedSeconds;

                int Current = Math.Min(Frames.Count - 1, (int)(Position / DecodedSeconds * Frames.Count));

                if (Current != FrameIndex)
                {
                    FrameIndex = Current;

                    PreviewImage.Source = Frames[FrameIndex];
                }

                SetPlaybackPosition(Position);
            }
        };

        DragDrop.SetAllowDrop(this, true);

        AddHandler(DragDrop.DragOverEvent, (_, Event) => Event.DragEffects = Busy ? DragDropEffects.None : DragDropEffects.Copy);

        AddHandler(DragDrop.DropEvent, async (_, Event) =>
        {
            IStorageItem? Item = Event.DataTransfer.TryGetFiles()?.FirstOrDefault();

            if (!Busy && Item?.TryGetLocalPath() is string Path)
            {
                await ImportAsync(Path);
            }
        });

        KeyDown += async (_, Event) =>
        {
            if (Event.Key is Key.O && Event.KeyModifiers.HasFlag(KeyModifiers.Control))
            {
                await BrowseAsync();

                Event.Handled = true;
            }
            else if (Event.Key is Key.S && Event.KeyModifiers.HasFlag(KeyModifiers.Control))
            {
                await RunWorkAsync(SaveAsync);

                Event.Handled = true;
            }
            else if (Event.Key is Key.Space && Event.Source is Visual Source &&
                !Source.GetSelfAndVisualAncestors().Any(Ancestor => Ancestor is TextBox or NumericUpDown or Button or ComboBox))
            {
                TogglePlayback();

                Event.Handled = true;
            }
        };

        string? BridgeName = null;


        Bridge = new BrowserBridge(async Session =>
        {
            await Dispatcher.UIThread.InvokeAsync(() => AcceptSessionAsync(Session));
        }, BridgeName);

        Opened += async (_, _) => await OnOpenedAsync();

        Closing += async (_, Event) =>
        {
            if (!ResourcesReleased)
            {
                Event.Cancel = true;

                if (!ClosingRequested)
                {
                    ClosingRequested = true;

                    await ShutdownAsync();
                }
            }
        };

        UpdateButtons();
    }

    /// <summary>Builds the compact editing canvas and independent reusable-file upload card.</summary>
    private Grid BuildLayout()
    {
        Grid Root = new()
        {
            RowDefinitions = new RowDefinitions("64,*"),

            MinWidth = 1120,

            MinHeight = 820
        };

        LayoutRoot = Root;

        Button About = Icons.Button("info", "Open-source notices");

        About.Click += async (_, _) => await ShowAboutAsync();

        StackPanel Toolbar = new()
        {
            Orientation = Orientation.Horizontal,

            Spacing = 10,

            Children = { Browse, Refresh, About }
        };

        Root.Children.Add(WindowChrome.Header(this, Account, Toolbar));

        Grid Body = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,360"),

            ColumnSpacing = 16,

            Margin = new Thickness(20, 14, 20, 16)
        };

        BodyLayout = Body;

        Grid.SetRow(Body, 1);

        Root.Children.Add(Body);

        Grid Canvas = new()
        {
            RowDefinitions = new RowDefinitions("Auto,*,Auto,Auto"),

            RowSpacing = 16
        };

        CanvasLayout = Canvas;

        Body.Children.Add(Canvas);

        SourceHeader = new Grid
        {
            IsVisible = false,

            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            ColumnSpacing = 20,

            Margin = new Thickness(2, 2, 0, 0),

            Children =
            {
                new StackPanel
                {
                    Spacing = 6,

                    Children = { Filename, SourceDetails }
                },

                OutputDetails
            }
        };

        Filename.TextTrimming = TextTrimming.CharacterEllipsis;

        OutputDetails.MaxWidth = 300;

        OutputDetails.VerticalAlignment = VerticalAlignment.Bottom;

        OutputDetails.TextAlignment = TextAlignment.Right;

        Grid.SetColumn(OutputDetails, 1);

        Canvas.Children.Add(SourceHeader);

        SurfaceGlyphs.Add("film", Icons.Make("film", 50, Brush.Parse("#898989")));

        Empty.Children.Add(SurfaceGlyphs["film"]);

        Empty.Children.Add(Label("Drop media", 23));

        Grid Preview = new()
        {
            Children = { PreviewImage, Empty }
        };

        Border Surface = Card(Preview, 0);

        Surface.Background = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),

            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),

            GradientStops =
            {
                new GradientStop(Color.Parse("#242424"), 0),

                new GradientStop(Color.Parse("#151515"), 0.65),

                new GradientStop(Color.Parse("#282828"), 1)
            }
        };

        // Clip the child once, inside the border stroke. A rectangular viewport clip
        // over the border's rounded clip produces visible seams at the corners.
        RectangleGeometry ViewportClip = new()
        {
            RadiusX = 17,

            RadiusY = 17
        };

        Preview.Clip = ViewportClip;

        Preview.SizeChanged += (_, Event) => ViewportClip.Rect = new Rect(Event.NewSize);

        Grid.SetRow(Surface, 1);

        Canvas.Children.Add(Surface);

        Grid PlaybackBar = new()
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"),

            ColumnSpacing = 14,

            Margin = new Thickness(0, 0, 0, 4)
        };

        PlaybackBar.Children.Add(Play);

        PreviewDetails.VerticalAlignment = VerticalAlignment.Center;

        PreviewDetails.Width = 140;

        PreviewDetails.Margin = new Thickness(12, 0, 0, 0);

        PreviewDetails.TextAlignment = TextAlignment.Right;

        Grid.SetColumn(PreviewDetails, 2);

        Grid.SetColumn(PreviewPosition, 1);

        PlaybackBar.Children.Add(PreviewPosition);

        ToolTip.SetTip(PreviewPosition, "Seek within the decoded preview");

        PreviewPosition.ValueChanged += (_, _) => SeekPreview();

        PlaybackBar.Children.Add(PreviewDetails);

        Grid.SetRow(PlaybackBar, 2);

        Canvas.Children.Add(PlaybackBar);

        SettingsPanel.Children.Add(Section("crop", "Canvas"));

        PresetCaption.IsVisible = true;

        SettingsPanel.Children.Add(new StackPanel
        {
            Spacing = 7,

            Children = { new Grid { Children = { PresetCaption } }, Preset }
        });

        SettingsPanel.Children.Add(Pair(Field("Width", CanvasWidth), Field("Height", CanvasHeight)));

        SettingsPanel.Children.Add(Field("Fit", Fit));

        SettingsPanel.Children.Add(Pair(Field("Start", Start), Field("Length", Duration)));

        SettingsPanel.Children.Add(SliderField("Frame rate", FrameRate, "fps"));

        ToolTip.SetTip(FrameRate, "Output frames per second; higher values increase motion detail and file size.");

        CompressionMode.MinHeight = 28;

        CompressionMode.Height = 28;

        CompressionMode.Width = 124;

        CompressionMode.FontSize = 11;

        CompressionMode.Padding = new Thickness(8, 0);

        CompressionMode.HorizontalAlignment = HorizontalAlignment.Left;

        CompressionValue.VerticalAlignment = VerticalAlignment.Center;

        Grid CompressionHeader = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            Children = { CompressionMode, CompressionValue }
        };

        Grid.SetColumn(CompressionValue, 1);

        SettingsPanel.Children.Add(new StackPanel
        {
            Spacing = 7,

            Children = { CompressionHeader, Compression }
        });

        ToolTip.SetTip(CompressionMode, "Lossless preserves decoded pixels. Lossy trades image detail for faster encoding and smaller files.");

        CompressionMode.ContainerPrepared += (_, Event) => ToolTip.SetTip(Event.Container, Event.Index is 0
            ? "Preserve decoded pixels. Effort zero favors speed; higher effort can reduce file size."
            : "Fast lossy compression. Adjust quality from 0 to 100; higher values retain more detail.");

        UpdateCompressionValue();

        SettingsPanel.Children.Add(Pair(Field("Loops", Loops), Field("Preview", PreviewLength)));

        StandardSettings = [.. SettingsPanel.Children];

        ExportActions = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            ColumnSpacing = 10,

            Children = { Convert, Save }
        };

        Grid.SetColumn(Save, 1);

        Grid ExportCard = new()
        {
            RowDefinitions = new RowDefinitions("*,Auto"),

            RowSpacing = 12,

            Children =
            {
                SettingsPanel,

                ExportActions
            }
        };

        Grid.SetRow(ExportActions, 1);

        ExportLayout = ExportCard;

        UploadLocation.ItemsSource = UploadDestinations;

        UploadLocation.SelectedIndex = 0;

        Grid UploadActions = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            ColumnSpacing = 8,

            RowDefinitions = new RowDefinitions("Auto,Auto"),

            RowSpacing = 7,

            Children = { UploadCaption, UploadLocation, Upload }
        };

        Grid.SetColumnSpan(UploadCaption, 2);

        Grid.SetRow(UploadLocation, 1);

        Grid.SetRow(Upload, 1);

        Grid.SetColumn(Upload, 1);

        Upload.VerticalAlignment = VerticalAlignment.Bottom;

        Grid ReuseRow = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            ColumnSpacing = 8,

            Children = { ExistingImage, Reuse }
        };

        Grid.SetColumn(Reuse, 1);

        ToolTip.SetShowOnDisabled(Reuse, true);

        ManageFiles.Width = 30;

        ManageFiles.Height = 30;

        ManageFiles.MinWidth = 30;

        ManageFiles.MinHeight = 30;

        Grid CloudHeading = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            Children = { Section("cloud", "VRChat Files"), ManageFiles }
        };

        Grid.SetColumn(ManageFiles, 1);

        ExistingImageField = Field("Existing image", ReuseRow);

        Grid GalleryRow = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            ColumnSpacing = 8,

            Children = { GalleryAvatar, CheckGallery }
        };

        Grid.SetColumn(CheckGallery, 1);

        GalleryTargetField = Field("Avatar gallery", GalleryRow);

        GalleryTargetField.IsVisible = false;

        Grid ImageSelector = new()
        {
            Children = { ExistingImageField, GalleryTargetField }
        };

        StackPanel Cloud = new()
        {
            Spacing = 12,

            Children = { CloudHeading, ConnectedAccount, CreatedFileDetails, UploadActions, CloudStatus, ImageSelector }
        };

        CloudFields = Cloud;

        CloudStatus.TextWrapping = TextWrapping.Wrap;

        UploadCaption.TextWrapping = TextWrapping.Wrap;

        ConnectedAccount.TextWrapping = TextWrapping.Wrap;

        ToolTip.SetTip(UploadCaption, UploadPolicy.MaximumDescription);

        ToolTip.SetShowOnDisabled(Upload, true);

        CopyId.Click += async (_, _) => await CopyAsync(Uploaded?.FileId ?? Api.PendingFileId);

        CopyUrl.Click += async (_, _) => await CopyAsync(Uploaded?.Url);

        Grid Links = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto,Auto"),

            ColumnSpacing = 6,

            Children = { FileId, CopyId, CopyUrl }
        };

        RecoveryLinks = Links;

        Grid.SetColumn(CopyId, 1);

        Grid.SetColumn(CopyUrl, 2);

        UploadedPanel.Children.Add(Links);

        Purpose.ItemsSource = ImagePurposes;

        Purpose.SelectedIndex = 0;

        Grid ApplyRow = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            ColumnSpacing = 8,

            Children = { new Grid { Children = { Target, ProfileTarget } }, Apply }
        };

        Grid.SetColumn(Apply, 1);

        ImageUseFields = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,Auto"),

            RowSpacing = 10,

            ColumnSpacing = 10,

            Children =
            {
                Field("Destination", Purpose),
                new StackPanel { Spacing = 7, Children = { new Grid { Children = { TargetCaption } }, ApplyRow } }
            }
        };

        Grid.SetRow(ImageUseFields.Children[1], 1);

        ImageUse = new MotionExpander
        {
            Header = "Use image",

            HorizontalAlignment = HorizontalAlignment.Stretch,

            Content = ImageUseFields
        };

        UploadedPanel.Children.Add(ImageUse);

        CloudLayout = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,Auto"),

            RowSpacing = 12,

            ColumnSpacing = 16,

            Children = { Cloud, UploadedPanel }
        };

        Grid.SetRow(UploadedPanel, 1);

        TabItem EditorTab = new()
        {
            Header = Icons.Make("sliders", 19, Brush.Parse("#BFBFBF")),

            Content = ExportCard
        };

        ToolTip.SetTip(EditorTab, "Conversion settings");

        AutomationProperties.SetName(EditorTab, "Conversion settings");

        TabItem CloudTab = new()
        {
            Header = Icons.Make("cloud", 19, Brush.Parse("#BFBFBF")),

            Content = CloudLayout
        };

        ToolTip.SetTip(CloudTab, "Upload & use image");

        AutomationProperties.SetName(CloudTab, "Upload & use image");

        _ = InspectorTabs.Items.Add(EditorTab);

        _ = InspectorTabs.Items.Add(CloudTab);

        InspectorTabs.SelectedIndex = 0;

        InspectorTabs.SelectionChanged += (_, _) => ExportActions.IsVisible = !CompactLayout || InspectorTabs.SelectedIndex is 0;

        InspectorOverlay = new()
        {
            Children = { InspectorTabs }
        };

        Border Inspector = Card(InspectorOverlay, 16);

        InspectorCard = Inspector;

        Grid.SetColumn(Inspector, 1);

        Body.Children.Add(Inspector);

        Grid Footer = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            ColumnSpacing = 12,

            MinHeight = 30,

            Margin = new Thickness(0)
        };

        Status.TextWrapping = TextWrapping.NoWrap;

        Status.VerticalAlignment = VerticalAlignment.Center;

        Grid StatusLine = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            ColumnSpacing = 12,

            HorizontalAlignment = HorizontalAlignment.Left,

            VerticalAlignment = VerticalAlignment.Center,

            Children = { Status, Progress }
        };

        Footer.Children.Add(StatusLine);

        Grid.SetColumn(Progress, 1);

        Cancel.Height = 30;

        Cancel.Width = 30;

        Cancel.MinHeight = 30;

        Grid.SetColumn(Cancel, 1);

        Footer.Children.Add(Cancel);

        Grid.SetRow(Footer, 3);

        Canvas.Children.Add(Footer);

        return Root;
    }

    /// <summary>Reflows settings into two horizontal rows for short shell snap zones.</summary>
    private void AdaptLayout()
    {
        if (ClosingRequested || WindowState is WindowState.Minimized || ClientSize.Height <= 0)
        {
            return;
        }

        bool Compact = ClientSize.Height < (CompactLayout ? 760 : 720);

        if (Compact == CompactLayout)
        {
            return;
        }

        CompactLayout = Compact;

        BeginLayoutTransition();

        CloudFields.Spacing = Compact ? 8 : 12;

        InspectorCard.Padding = new Thickness(Compact ? 12 : 16);

        CloudLayout.ColumnDefinitions = new ColumnDefinitions(Compact ? "*,*" : "*");

        CloudLayout.RowDefinitions = new RowDefinitions(Compact ? "Auto" : "Auto,Auto");

        Grid.SetColumn(UploadedPanel, Compact ? 1 : 0);

        Grid.SetRow(UploadedPanel, Compact ? 0 : 1);

        ImageUseFields.ColumnDefinitions = new ColumnDefinitions(Compact ? "*,*" : "*");

        ImageUseFields.RowDefinitions = new RowDefinitions(Compact ? "Auto" : "Auto,Auto");

        Grid.SetColumn(ImageUseFields.Children[1], Compact ? 1 : 0);

        Grid.SetRow(ImageUseFields.Children[1], Compact ? 0 : 1);

        SettingsPanel.Children.Clear();

        CompactTop.Children.Clear();

        CompactBottom.Children.Clear();

        BodyLayout.ColumnDefinitions = new ColumnDefinitions(Compact ? "*" : "*,360");

        BodyLayout.RowDefinitions = new RowDefinitions(Compact ? "*,Auto" : "*");

        BodyLayout.RowSpacing = Compact ? 12 : 0;

        Grid.SetColumn(InspectorCard, Compact ? 0 : 1);

        Grid.SetRow(InspectorCard, Compact ? 1 : 0);

        LayoutRoot.MinWidth = Compact ? 900 : 1120;

        LayoutRoot.MinHeight = Compact ? 580 : 820;

        CanvasLayout.RowSpacing = Compact ? 8 : 16;

        Empty.Orientation = Compact ? Orientation.Horizontal : Orientation.Vertical;

        Empty.Spacing = Compact ? 12 : 18;

        PresetCaption.IsVisible = true;

        foreach (int Index in new[] { 1, 3, 5, 6 })
        {
            StackPanel Field = (StackPanel)StandardSettings[Index];

            Field.Children[0].Height = Compact ? 28 : double.NaN;

            if (Field.Children[0] is Grid Header)
            {
                foreach (Control Child in Header.Children)
                {
                    Child.VerticalAlignment = VerticalAlignment.Center;
                }
            }

            Field.Spacing = 7;
        }

        CompressionMode.Width = Compact ? 110 : 124;

        CompressionMode.VerticalAlignment = VerticalAlignment.Center;

        if (Compact)
        {
            _ = ExportLayout.Children.Remove(ExportActions);

            Grid.SetRow(ExportActions, 0);

            ExportActions.VerticalAlignment = VerticalAlignment.Top;

            ExportActions.HorizontalAlignment = HorizontalAlignment.Right;

            ExportActions.Margin = new Thickness(12, 4, 12, 0);

            InspectorOverlay.Children.Add(ExportActions);
        }
        else
        {
            _ = InspectorOverlay.Children.Remove(ExportActions);

            Grid.SetRow(ExportActions, 1);

            ExportActions.VerticalAlignment = VerticalAlignment.Stretch;

            ExportActions.HorizontalAlignment = HorizontalAlignment.Stretch;

            ExportActions.Margin = default;

            ExportLayout.Children.Add(ExportActions);
        }

        ExportActions.IsVisible = !Compact || InspectorTabs.SelectedIndex is 0;

        ExportLayout.RowDefinitions = new RowDefinitions(Compact ? "*" : "*,Auto");

        ExportLayout.RowSpacing = Compact ? 8 : 12;

        foreach (Control Child in Empty.Children)
        {
            Child.VerticalAlignment = VerticalAlignment.Center;
        }

        if (Compact)
        {
            ReadOnlySpan<int> Top = [1, 3, 5, 6];

            ReadOnlySpan<int> Bottom = [2, 4, 7];

            for (int Index = 0; Index < Top.Length; ++Index)
            {
                Control Control = StandardSettings[Top[Index]];

                Grid.SetColumn(Control, Index);

                CompactTop.Children.Add(Control);
            }

            for (int Index = 0; Index < Bottom.Length; ++Index)
            {
                Control Control = StandardSettings[Bottom[Index]];

                Grid.SetColumn(Control, Index);

                CompactBottom.Children.Add(Control);
            }

            SettingsPanel.Children.Add(CompactTop);

            SettingsPanel.Children.Add(CompactBottom);
        }
        else
        {
            foreach (Control Control in StandardSettings)
            {
                Grid.SetColumn(Control, 0);

                SettingsPanel.Children.Add(Control);
            }
        }
    }

    /// <summary>Opens the native media picker and imports one local source.</summary>
    private async Task BrowseAsync()
    {
        if (Busy)
        {
            return;
        }

        try
        {
            IReadOnlyList<IStorageFile> Files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open animated media",

                AllowMultiple = false,

                FileTypeFilter =
                [
                    new FilePickerFileType("Animated media")
                    {
                        Patterns = ["*.mp4", "*.webm", "*.mkv", "*.mov", "*.avi", "*.m4v", "*.gif", "*.webp", "*.png", "*.avif", "*.mng"]
                    },

                    FilePickerFileTypes.All
                ]
            });

            if (Files.Count >= 1 && Files[0].TryGetLocalPath() is string Path)
            {
                await ImportAsync(Path);
            }
        }
        catch (Exception Error)
        {
            ShowError(Error.Message);
        }
    }

    /// <summary>Copies the verified animation to the destination explicitly chosen in the save dialog.</summary>
    private async Task SaveAsync(CancellationToken Token)
    {
        if (Result is null)
        {
            return;
        }

        IStorageFile? Destination = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save " + WebPFormat.Name,

            SuggestedFileName = System.IO.Path.GetFileNameWithoutExtension(InputPath) + WebPFormat.Extension,

            DefaultExtension = WebPFormat.Extension[1..],

            ShowOverwritePrompt = true,

            FileTypeChoices =
            [
                new FilePickerFileType(WebPFormat.Name)
                {
                    Patterns = ["*.webp"]
                }
            ]
        });

        if (Destination is null)
        {
            return;
        }

        string DestinationPath = Destination.TryGetLocalPath() ?? throw new InvalidOperationException("Choose a local Windows file destination.");

        await SaveCreatedFileAsync(Result.Path, DestinationPath, Reporter(Stage: "Saving file", Token: Token), Token);

        UiMotion.SetText(Status, "Saved: " + Destination.Name);
    }

    /// <summary>Commits a staged copy atomically and removes partial output even when its work token has been cancelled.</summary>
    private static async Task SaveCreatedFileAsync(string Source, string Destination, IProgress<WorkProgress> Progress, CancellationToken Token)
    {
        string Temporary = Destination + "." + Guid.NewGuid().ToString("N") + ".tmp";

        try
        {
            await Task.Run(async () =>
            {
                await CopyFileAsync(Source, Temporary, Progress, Token);

                Token.ThrowIfCancellationRequested();

                File.Move(Temporary, Destination, true);
            }, Token);
        }
        finally
        {
            await Task.Run(() => File.Delete(Temporary), CancellationToken.None);
        }
    }

    /// <summary>Uploads a reusable file and exposes its independent ID and URL.</summary>
    private async Task UploadAsync(CancellationToken Token)
    {
        if (Result is null)
        {
            return;
        }

        Uploaded = null;

        UiMotion.SetVisible(UploadedPanel, false);

        FileId.Text = "";

        UploadNotice = "Uploading…";

        UpdateUploadAccess();

        IProgress<WorkProgress> UploadProgress = Reporter(Token: Token);

        ConversionResult File = Result;

        string Name = "VRCWebP: " + System.IO.Path.GetFileNameWithoutExtension(InputPath);

        UploadDestination Destination = SelectedUploadDestination;

        string GalleryId = GalleryAvatar.Text?.Trim() ?? "";

        try
        {
            Uploaded = await Task.Run(() => Api.UploadAsync(File, Name, UploadProgress, Token, Destination, GalleryId), Token);
        }
        catch (OperationCanceledException)
        {
            UploadNotice = "Upload cancelled. You can try again.";

            throw;
        }
        catch
        {
            UploadNotice = "Upload failed. You can try again.";

            throw;
        }

        CheckedGallery = null;

        UploadAccessState = null;

        FileId.Text = Uploaded.FileId;

        UiMotion.SetVisible(UploadedPanel, true);

        UploadNotice = "Uploaded.";

        CloudStatus.Foreground = Brush.Parse("#CACACA");

        UiMotion.SetText(Status, "Image uploaded: copy its ID or URL");
    }

    /// <summary>Applies a selected image or a file-independent avatar profile mode after an explicit target action.</summary>
    private async Task ApplyAsync(CancellationToken Token)
    {
        if (Uploaded is null && Purpose.SelectedIndex is not (5 or 6))
        {
            return;
        }

        string PurposeName = Purpose.SelectedIndex switch
        {
            1 => "Group icon",

            2 => "Avatar thumbnail",

            3 => "Profile icon",

            4 => "Profile banner",

            5 => "Profile icon from avatar",

            6 => "Profile banner from avatar",

            _ => "Group banner"
        };

        UploadResult? Selected = Uploaded;

        string TargetId = Target.Text?.Trim() ?? "";

        UiMotion.SetText(Status, "Applying image");

        string State = await Task.Run(() => Api.ApplyAsync(Selected, PurposeName, TargetId, Token), Token);

        UploadNotice = State;

        UiMotion.SetText(Status, State);
    }

    /// <summary>Selects an existing complete server image independently of the current local media or recipe.</summary>
    private async Task ReuseAsync(CancellationToken Token)
    {
        string Reference = ExistingImage.Text?.Trim() ?? "";

        UiMotion.SetText(Status, "Checking existing image");

        UploadResult Selected = await Task.Run(() => Api.ReuseImageAsync(Reference, Token), Token);

        Uploaded = Selected;

        FileId.Text = Selected.FileId;

        UiMotion.SetVisible(UploadedPanel, true);

        UploadNotice = "Uploaded image selected.";

        ImageUse.IsExpanded = true;

        UiMotion.SetText(Status, "Existing image ready; no upload needed");
    }

    /// <summary>Keeps inventory management under the same account and operation gate as encoding and uploads.</summary>
    private async Task ManageImagesAsync(CancellationToken Token)
    {
        UiMotion.SetText(Status, "Managing uploaded images");

        ImageManagerWindow Manager = new(Api, Token);

        Manager.FileDeleted += OnImageDeleted;

        string? Selected = await Manager.ShowDialog<string?>(this);

        if (Selected is not null)
        {
            ExistingImage.Text = Selected;

            await ReuseAsync(Token);
        }
        else
        {
            UiMotion.SetText(Status, "Image manager closed");
        }
    }

    /// <summary>Invalidates a selected server image immediately after confirmed manual deletion.</summary>
    private void OnImageDeleted(string Id)
    {
        if (Uploaded?.FileId == Id)
        {
            Uploaded = null;

            FileId.Text = Api.PendingFileId ?? "";

            UploadNotice = "Selected server image deleted.";

            UpdateButtons();
        }
    }

    /// <summary>Explains destination-specific compatibility without confusing successful storage with avatar support.</summary>
    private string ImagePurposeDescription(int Index) => Index switch
    {
        0 => "Set a group banner using this image.",

        1 => "Set a group icon using this image.",

        3 => Api.ProfileImageUnavailableReason(Uploaded, "Profile icon") is string IconReason ? IconReason
            : "Set your profile icon using this image.",

        4 => Api.ProfileImageUnavailableReason(Uploaded, "Profile banner") is string BannerReason ? BannerReason
            : "Set your profile banner using this image.",

        5 => "Use your current avatar's thumbnail as your profile icon.",

        6 => "Use your current avatar's thumbnail as your profile banner.",

        _ => Uploaded is not null && Api.AvatarThumbnailUnavailableReason(Uploaded) is string Reason ? Reason
            : "Set the thumbnail of an avatar you own."
    };

    /// <summary>Checks file ownership/declaration and profile entitlements independently from file-free avatar modes.</summary>
    private void UpdateImagePurposes()
    {
        ImagePurposes[2].IsEnabled = Uploaded is not null && Api.AvatarThumbnailUnavailableReason(Uploaded) is null;

        if (!ImagePurposes[2].IsEnabled && Purpose.SelectedIndex is 2)
        {
            Purpose.SelectedIndex = 0;
        }

        ImagePurposes[0].IsEnabled = Uploaded is not null;

        ImagePurposes[1].IsEnabled = Uploaded is not null;

        ImagePurposes[3].IsEnabled = Uploaded is not null && Api.ProfileImageUnavailableReason(Uploaded, "Profile icon") is null;

        ImagePurposes[4].IsEnabled = Uploaded is not null && Api.ProfileImageUnavailableReason(Uploaded, "Profile banner") is null;

        if (Purpose.SelectedIndex is 3 or 4 && !ImagePurposes[Purpose.SelectedIndex].IsEnabled)
        {
            Purpose.SelectedIndex = 0;
        }

        for (int Index = 0; Index < ImagePurposes.Length; ++Index)
        {
            ToolTip.SetShowOnDisabled(ImagePurposes[Index], true);

            ToolTip.SetTip(ImagePurposes[Index], Uploaded is null && Index <= 4
                ? "Select a complete uploaded image first."
                : ImagePurposeDescription(Index));
        }

        bool Personal = Purpose.SelectedIndex is >= 3 and <= 6;

        UiMotion.SetVisible(Target, !Personal);

        UiMotion.SetVisible(ProfileTarget, Personal);

        UiMotion.SetText(ProfileTarget, Api.DisplayName ?? "Your connected profile");

        UiMotion.SetText(TargetCaption, Personal ? "Profile" : Purpose.SelectedIndex is 2 ? "Avatar ID" : "Group ID");

        ToolTip.SetTip(Target, Purpose.SelectedIndex is 2 ? "Enter the avtr_ ID of an avatar you own." : "Enter the grp_ ID of a group you can edit.");

        ToolTip.SetTip(Purpose, ImagePurposeDescription(Purpose.SelectedIndex));

        ToolTip.SetTip(ProfileTarget, "Update your connected profile.");
    }

    /// <summary>Allows a single foreground operation and keeps preview and mutable recipes out of its lifetime.</summary>
    private Task RunWorkAsync(Func<CancellationToken, Task> Work)
    {
        if (Busy || ClosingRequested)
        {
            return Task.CompletedTask;
        }

        ForegroundTask = RunWorkCoreAsync(Work);

        return ForegroundTask;
    }

    private async Task RunWorkCoreAsync(Func<CancellationToken, Task> Work)
    {
        if (Busy)
        {
            return;
        }

        Busy = true;

        using CancellationTokenSource Pending = CancellationTokenSource.CreateLinkedTokenSource(Lifetime.Token);

        Operation = Pending;

        ++ProgressRevision;

        UiMotion.SetText(Status, "Preparing operation");

        Progress.IsIndeterminate = true;

        if (PreviewOperation is not null)
        {
            await PreviewOperation.CancelAsync();
        }

        Debounce.Stop();

        Progress.Value = 0;

        Status.Foreground = Brush.Parse("#B2B2B2");

        UpdateButtons();

        try
        {
            await WorkGate.WaitAsync(Pending.Token);

            try
            {
                await Work(Pending.Token);
            }
            finally
            {
                _ = WorkGate.Release();
            }
        }
        catch (OperationCanceledException)
        {
            UiMotion.SetText(Status, "Cancelled");

            ShowPendingFile();
        }
        catch (Exception Error)
        {
            ShowError(Error.Message);

            ShowPendingFile();
        }
        finally
        {
            ++ProgressRevision;

            await Pending.CancelAsync();

            Operation = null;

            Busy = false;

            Progress.IsIndeterminate = false;

            UpdateButtons();
        }
    }

    /// <summary>Verifies a shared browser session before replacing and protecting the current connection.</summary>
    private Task AcceptSessionAsync(BrowserSession Session)
    {
        if (Busy || ClosingRequested)
        {
            return Task.FromException(new InvalidOperationException("An operation is already running."));
        }

        SessionTask = AcceptSessionCoreAsync(Session);

        return SessionTask;
    }

    private async Task AcceptSessionCoreAsync(BrowserSession Session)
    {
        if (Busy || ClosingRequested)
        {
            throw new InvalidOperationException("An operation is already running.");
        }

        Busy = true;

        UpdateButtons();

        VRChatService Candidate = new();

        try
        {
            UiMotion.SetText(Status, "Verifying browser session");

            using CancellationTokenSource Timeout = CancellationTokenSource.CreateLinkedTokenSource(Lifetime.Token);

            Operation = Timeout;

            Timeout.CancelAfter(TimeSpan.FromSeconds(25));

            await Candidate.ConnectAsync(Session, Timeout.Token);

            await Task.Run(() => SessionVault.Save(Session));

            Api.Dispose();

            Api = Candidate;

            CheckedGallery = null;

            Uploaded = null;

            UiMotion.SetVisible(UploadedPanel, false);

            UploadNotice = null;

            UpdateUploadAccess();

            UiMotion.SetText(Status, "Browser connected.");

            ToolTip.SetTip(Account, Api.DisplayName + ": manage browser connection");
        }
        catch
        {
            Candidate.Dispose();

            UiMotion.SetText(Status, "Reconnect from a signed-in VRChat browser profile");

            throw;
        }
        finally
        {
            Operation = null;

            Busy = false;

            UpdateButtons();
        }
    }

    /// <summary>Shows browser setup or an explicit action to erase the protected session.</summary>
    private async Task ShowBrowserAsync()
    {
        if (Busy)
        {
            return;
        }

        if (Api.UserId is not null)
        {
            Window Dialog = DialogWindow("Browser connection", 480, 280);

            Button Disconnect = Icons.Action("close", "Disconnect & forget session");

            Disconnect.Click += async (_, _) =>
            {
                await Task.Run(SessionVault.Forget);

                Api.Dispose();

                Api = new VRChatService();

                Uploaded = null;

                UiMotion.SetVisible(UploadedPanel, false);

                UploadNotice = null;

                UpdateUploadAccess();

                UiMotion.SetText(Status, "Saved session erased: browser stays connected to VRChat");

                UpdateButtons();

                Dialog.Close();
            };

            SetDialogBody(Dialog, new StackPanel
            {
                Spacing = 20,

                Children = { Label(Api.DisplayName ?? "Connected", 21), Label("Saved session protected with Windows DPAPI.", 13, true), Disconnect }
            });

            await Dialog.ShowDialog(this);

            return;
        }

        await new BrowserWindow().ShowDialog(this);
    }

    /// <summary>Disables unavailable destinations and shares one eligibility reason with the button, tooltip and status.</summary>
    private void UpdateUploadAccess()
    {
        if (UploadLocation.SelectedIndex > 0 && !Api.CanUpload(SelectedUploadDestination))
        {
            UploadLocation.SelectedIndex = 0;
        }

        (string? UserId, string? DisplayName, bool Known, bool Gallery, bool Icon, UploadDestination Destination, long? Bytes, bool Working, bool Current, string? Notice) State =
            (Api.UserId, Api.DisplayName, Api.PermissionsKnown, Api.GalleryUploadAllowed, Api.UserIconUploadAllowed,
                SelectedUploadDestination, Result?.Bytes, Busy, HasCurrentOutput(), UploadNotice);

        if (UploadAccessState == State)
        {
            return;
        }

        UploadAccessState = State;

        bool AvatarGallery = SelectedUploadDestination is UploadDestination.AvatarGallery;

        if (GalleryTargetField is not null && ExistingImageField is not null)
        {
            UiMotion.SetVisible(GalleryTargetField, AvatarGallery);

            UiMotion.SetVisible(ExistingImageField, !AvatarGallery);
        }

        UiMotion.SetText(UploadCaption, FormatUploadCaption(UploadPolicy.MaximumFor(SelectedUploadDestination)));

        ToolTip.SetTip(UploadCaption, UploadPolicy.DescriptionFor(SelectedUploadDestination));

        UiMotion.SetText(ConnectedAccount, Api.UserId is null ? "Browser not connected" : "Connected as " + (Api.DisplayName ?? "VRChat user"));

        for (int Index = 0; Index < UploadDestinations.Length; ++Index)
        {
            UploadDestination Destination = (UploadDestination)Index;

            ComboBoxItem Item = UploadDestinations[Index];

            Item.IsEnabled = Destination is UploadDestination.Original || Api.CanUpload(Destination);

            ToolTip.SetShowOnDisabled(Item, true);

            ToolTip.SetTip(Item, UploadDestinationDescription(Destination));
        }

        bool TooLarge = Result is not null && !UploadPolicy.IsWithinLimit(Result.Bytes, SelectedUploadDestination);

        string? Reason = Busy ? "Wait for the current task to finish."
            : TooLarge ? (AvatarGallery ? "File exceeds the avatar-gallery limit of 10 MiB."
                : SelectedUploadDestination is UploadDestination.Original ? "File exceeds the supported image-upload limit of 10 MB. Save locally or reduce the output."
                : "File exceeds the image-inventory limit of 10 MiB.")
            : Api.UserId is null ? "Connect your browser to upload."
            : !Api.CanUpload(SelectedUploadDestination) ? UploadDestinationDescription(SelectedUploadDestination)
            : AvatarGallery && !VRChatService.IsAvatarId(GalleryAvatar.Text?.Trim() ?? "") ? "Enter the avtr_ ID of an avatar you own."
            : AvatarGallery && CheckedGallery?.FileIds.Count >= 6 ? "This avatar gallery already has six images."
            : Result is null ? "Create a WebP before uploading."
            : null;

        string Explanation = !Busy && TooLarge
            ? UploadPolicy.DescribeRejection(Result!.Bytes, SelectedUploadDestination) : Reason ?? UploadDestinationDescription(SelectedUploadDestination);

        Upload.IsEnabled = Reason is null;

        ToolTip.SetTip(UploadLocation, UploadDestinationDescription(SelectedUploadDestination));

        ToolTip.SetTip(Upload, Reason is null ? "Upload the created WebP." : Explanation);

        UiMotion.SetText(CreatedFileDetails, Result is null ? "" : FormatCreatedFileBytes(Result.Bytes));

        UiMotion.SetVisible(CreatedFileDetails, Result is not null);

        ToolTip.SetTip(CreatedFileDetails, HasCurrentOutput()
            ? "The created WebP is ready to save or upload."
            : "The last created WebP is retained.\nUpload and Save use this file until you create a replacement.");

        string Displayed = !Busy && Uploaded is not null && UploadNotice is not null ? UploadNotice : Reason ?? UploadNotice ?? "Ready to upload.";

        ToolTip.SetTip(CloudStatus, Displayed == Reason ? Explanation : Displayed);

        UiMotion.SetText(CloudStatus, Displayed);
    }

    /// <summary>Explains only this destination's purpose or missing prerequisite.</summary>
    private string UploadDestinationDescription(UploadDestination Destination)
    {
        if (Destination is UploadDestination.AvatarGallery)
        {
            if (Api.UserId is null)
            {
                return "Connect your browser before uploading to an owned avatar's gallery.";
            }

            return "Add an image to an avatar's gallery.\nIts main thumbnail stays unchanged.";
        }

        if (Destination is not UploadDestination.Original)
        {
            if (Api.UserId is null)
            {
                return "Connect your browser to check access to this destination.";
            }

            if (!Api.PermissionsKnown)
            {
                return "Access could not be verified. Reconnect your browser to check again.";
            }

            if (!Api.CanUpload(Destination))
            {
                return Destination is UploadDestination.Gallery
                    ? "Unavailable: this account does not have photo-gallery upload permission, a VRC+ website feature."
                    : "Unavailable: this account does not have website user-icon upload permission, a VRC+ website feature.";
            }
        }

        return Destination switch
        {
            UploadDestination.Gallery => "Add this image to your VRChat photo gallery.",

            UploadDestination.UserIcon => "Add a website user icon to your VRChat account (VRC+ feature).",

            _ => "Store a reusable image in VRChat."
        };
    }

    private UploadDestination SelectedUploadDestination => UploadLocation.SelectedIndex switch
    {
        1 => UploadDestination.Gallery,

        2 => UploadDestination.UserIcon,

        3 => UploadDestination.AvatarGallery,

        _ => UploadDestination.Original
    };

    /// <summary>Restores a protected session after the window is ready.</summary>
    private async Task OnOpenedAsync()
    {
        try
        {

            await Task.Run(BrowserBridge.RefreshInstalled);

            BrowserSession? Session = await Task.Run(SessionVault.Load);

            if (Session is not null)
            {
                await AcceptSessionAsync(Session);
            }
        }
        catch (Exception Error)
        {

            ShowError(Error is System.Security.Cryptography.CryptographicException ? "The saved session cannot be decrypted. Connect your browser again." : Error.Message);
        }
    }

    /// <summary>Retains the server file ID when an upload is interrupted.</summary>
    private void ShowPendingFile()
    {
        if (Uploaded is null && Api.PendingFileId is not null)
        {
            FileId.Text = Api.PendingFileId;

            UiMotion.SetVisible(UploadedPanel, true);

            UploadNotice = Api.CanResumeUpload && UploadLocation.SelectedIndex is 0
                ? "Upload incomplete. Retry Upload to resume in this app session."
                : "Upload incomplete. Keep the file ID below for recovery.";

            UpdateUploadAccess();
        }
    }

    private void SettingsChanged()
    {
        if (Updating || Busy)
        {
            return;
        }

        ActivateEditingStill();

        ConfigureFraming();

        ++RecipeVersion;

        UserCancelledPreview = false;

        if (PreviewOperation is not null)
        {
            _ = PreviewOperation.CancelAsync();
        }

        InvalidateOutput();

        Debounce.Stop();

        Debounce.Start();
    }

    /// <summary>Refreshes editing state without discarding the last successfully created file.</summary>
    private void InvalidateOutput()
    {
        ShowOutputStatistics();

        UpdateButtons();
    }

    /// <summary>Releases an output associated with a source that has been explicitly replaced.</summary>
    private void ResetOutput()
    {
        if (Result is not null)
        {
            QueueDelete(Result.Path);
        }

        Result = null;

        UiMotion.SetText(OutputDetails, "");

        UpdateButtons();
    }

    /// <summary>Sets a canvas ratio without stretching source pixels.</summary>
    private void SetPreset()
    {
        CanvasPreset Selected = CanvasPreset.All[Math.Clamp(Preset.SelectedIndex, 0, CanvasPreset.All.Length - 1)];

        ToolTip.SetTip(Preset, Selected.Description);

        if (Updating)
        {
            return;
        }

        Updating = true;

        if (Selected.Width < 0)
        {
            Updating = false;

            return;
        }

        (int Width, int Height) Size = Selected.Width is 0
            ? CanvasPreset.SourceSize(InputInfo?.Width ?? 512, InputInfo?.Height ?? 512)
            : (Selected.Width, Selected.Height);

        CanvasWidth.Value = Size.Width;

        CanvasHeight.Value = Size.Height;

        Updating = false;

        SettingsChanged();
    }

    /// <summary>Validates the selected clip against the known source duration.</summary>
    private ConversionSettings ReadSettings()
    {
        double ClipStart = (double)(Start.Value ?? 0);

        double ClipDuration = (double)(Duration.Value ?? 6);

        if (InputInfo?.Duration > 0 && ClipStart + ClipDuration > InputInfo.Duration + 0.05)
        {
            throw new InvalidOperationException("The clip extends past the source. Reduce its start time or length.");
        }

        bool Lossless = CompressionMode.SelectedIndex is 0;

        return new ConversionSettings((int)(CanvasWidth.Value ?? 512), (int)(CanvasHeight.Value ?? 512), (int)FrameRate.Value, ClipStart, ClipDuration,
            Fit.SelectedItem as string ?? "Contain", Lossless ? (int)Compression.Value : 0, (int)(Loops.Value ?? 0), Lossless, Lossless ? 80 : (int)Compression.Value, PreviewImage.PanX, PreviewImage.PanY);
    }

    /// <summary>Explains the mode-specific slider without adding another settings row.</summary>
    private void UpdateCompressionValue()
    {
        bool Lossless = CompressionMode.SelectedIndex is 0;

        UiMotion.SetText(CompressionValue, Lossless ? $"{Compression.Value:0} / 6" : $"{Compression.Value:0}%");

        ToolTip.SetTip(Compression, Lossless
            ? "Lossless compression effort: 0–6. Zero is fastest; higher effort can reduce file size."
            : "Lossy quality: 0–100. Higher values preserve more detail and usually increase file size.");

        AutomationProperties.SetName(Compression, Lossless ? "Compression effort" : "Image quality");
    }

    /// <summary>Enables actions only when their media, account and operation prerequisites are satisfied.</summary>
    private void UpdateButtons()
    {
        if (Empty.IsVisible)
        {
            ushort Phase = (ushort)((((NumberField)CanvasWidth).Phase << 8) | ((NumberField)CanvasHeight).Phase);

            string Surface = Icons.Surface(Phase);

            if (!SurfaceGlyphs.TryGetValue(Surface, out Control? Glyph))
            {
                Glyph = Icons.Make(Surface, 50, Brush.Parse("#898989"));

                SurfaceGlyphs.Add(Surface, Glyph);
            }

            if (Empty.Children[0] != Glyph)
            {
                Empty.Children[0] = Glyph;

                Glyph.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        if (ClosingRequested)
        {
            return;
        }

        Browse.IsEnabled = !Busy;

        Refresh.IsEnabled = !Busy && InputPath is not null;

        bool WasUpdating = Updating;

        Updating = true;

        PreviewLength.Maximum = Math.Clamp(Duration.Value ?? 6, 0.1m, 60m);

        Updating = WasUpdating;

        PreviewImage.IsEnabled = !Busy;

        if (Busy)
        {
            PreviewImage.ReleaseDrag();
        }

        Play.IsEnabled = Frames.Count > 0 || (EncodedPlayback is not null && HasCurrentOutput());

        ToolTip.SetTip(Play, Playback.IsEnabled ? "Pause preview: Space" : "Play preview: Space");

        string? CreateReason = CreateUnavailableReason();

        Convert.IsEnabled = CreateReason is null;

        ToolTip.SetShowOnDisabled(Convert, true);

        ToolTip.SetTip(Convert, CreateReason ?? "Create a WebP from the current parameters.");

        ToolTip.SetShowOnDisabled(Save, true);

        ToolTip.SetTip(Save, Busy ? "Wait for the current task to finish." : Result is null ? "Create a WebP before saving." : "Save the created WebP: Ctrl+S");

        Save.IsEnabled = !Busy && Result is not null;

        UpdateUploadAccess();

        UploadLocation.IsEnabled = !Busy;

        ExistingImage.IsEnabled = !Busy;

        GalleryAvatar.IsEnabled = !Busy;

        CheckGallery.IsEnabled = !Busy && Api.UserId is not null && VRChatService.IsAvatarId(GalleryAvatar.Text?.Trim() ?? "");

        ToolTip.SetShowOnDisabled(CheckGallery, true);

        ToolTip.SetTip(CheckGallery, Busy ? "Wait for the current task to finish." : Api.UserId is null ? "Connect your browser to check a gallery."
            : !VRChatService.IsAvatarId(GalleryAvatar.Text?.Trim() ?? "") ? "Enter a complete avtr_ ID for an avatar you own."
            : "Check ownership and gallery slots without uploading an image.");

        bool ValidReference = VRChatService.TryParseImageReference(ExistingImage.Text ?? "", out _, out string? ReferenceReason);

        string? ReuseReason = Busy ? "Wait for the current task to finish."
            : Api.UserId is null ? "Connect your browser to reuse an existing image."
            : !ValidReference ? ReferenceReason : null;

        Reuse.IsEnabled = ReuseReason is null;

        ManageFiles.IsEnabled = !Busy && Api.UserId is not null;

        ToolTip.SetTip(ManageFiles, Api.UserId is null ? "Connect your browser to manage uploaded images" : "Manage uploaded images");

        ToolTip.SetTip(Reuse, ReuseReason ?? "Reuse this uploaded image.");

        UpdateImagePurposes();

        bool AvatarMode = Purpose.SelectedIndex is 5 or 6;

        string? ApplyReason = ImageApplyUnavailableReason();

        Apply.IsEnabled = ApplyReason is null;

        ImageUse.IsEnabled = !Busy && Api.UserId is not null;

        UiMotion.SetVisible(UploadedPanel, Api.UserId is not null || Uploaded is not null || Api.PendingFileId is not null);

        UiMotion.SetVisible(RecoveryLinks, Uploaded is not null || Api.PendingFileId is not null);

        UiMotion.SetVisible(ImageUse, Api.UserId is not null || Uploaded is not null);

        ToolTip.SetShowOnDisabled(Apply, true);

        ToolTip.SetTip(Apply, ApplyReason ?? (AvatarMode ? "Use your current avatar's thumbnail on your profile." : "Use this image at the selected destination."));

        if (Api.UserId is null)
        {
            ImageUse.IsExpanded = false;
        }

        CopyId.IsEnabled = !Busy && (Uploaded is not null || Api.PendingFileId is not null);

        CopyUrl.IsEnabled = !Busy && Uploaded is not null;

        UiMotion.SetVisible(CopyUrl, Uploaded is not null);

        Account.IsEnabled = !Busy;

        SettingsPanel.IsEnabled = !Busy;

        UiMotion.SetVisible(Cancel, Busy || PreviewOperation is not null || EncodedSeekOperation is not null);

        UiMotion.SetVisible(Progress, Busy || PreviewOperation is not null || EncodedSeekOperation is not null);

        PreviewPosition.IsEnabled = Frames.Count > 1 || (EncodedPlayback is not null && HasCurrentOutput());
    }

    private void TogglePlayback()
    {
        if (EncodedPlayback is not null)
        {
            bool Resume = !EncodedPlayback.IsPlaying;

            EncodedPlayback.SetPlaying(Resume);

            if (Resume)
            {
                Playback.Start();
            }
            else
            {
                Playback.Stop();
            }

            UiMotion.SetContent(Play, Resume ? PauseIcon : PlayIcon);

            return;
        }

        if (Playback.IsEnabled)
        {
            Playback.Stop();

            UiMotion.SetContent(Play, PlayIcon);
        }
        else if (Frames.Count > 0)
        {
            StartPreviewPlayback();

            UiMotion.SetContent(Play, PauseIcon);
        }

        ToolTip.SetTip(Play, Playback.IsEnabled ? "Pause preview: Space" : "Play preview: Space");
    }

    /// <summary>Stops playback before releasing every owned preview bitmap.</summary>
    private void ClearFrames(bool KeepStill = false)
    {
        if (!KeepStill)
        {
            ReleaseEditingStill();
        }
        DecodedSettings = null;

        DecodedPath = null;

        PreviewImage.EncodedSource = false;

        Playback.Stop();

        PreviewImage.Source = null;

        Bitmap[] Retired = [.. Frames];

        CleanupWork = CleanupWork.ContinueWith(_ =>
        {
            foreach (Bitmap Bitmap in Retired)
            {
                Bitmap.Dispose();
            }
        }, TaskScheduler.Default);

        Frames.Clear();

        FrameIndex = 0;
    }

    private async Task CopyAsync(string? Text)
    {
        if (Text is not null && Clipboard is not null)
        {
            await Clipboard.SetTextAsync(Text);

            UiMotion.SetText(Status, "Copied");
        }
    }

    private void ShowError(string Message)
    {
        if (ClosingRequested)
        {
            return;
        }

        UiMotion.SetText(Status, Message);

        Status.Foreground = Brush.Parse("#AEAEAE");
    }

    /// <summary>Drains operations before disposing native frames, HTTP clients and the temporary workspace.</summary>
    private async Task ShutdownAsync()
    {
        PreviewImage.ReleaseDrag();

        await StopEncodedPreviewAsync();

        await Lifetime.CancelAsync();

        if (Operation is not null)
        {
            await Operation.CancelAsync();
        }

        if (PreviewOperation is not null)
        {
            await PreviewOperation.CancelAsync();
        }

        Playback.Stop();

        Debounce.Stop();

        try
        {
            await Bridge.DisposeAsync();

            await Task.WhenAll(ForegroundTask ?? Task.CompletedTask, PreviewTask ?? Task.CompletedTask, SessionTask ?? Task.CompletedTask, EncodedSeekTask ?? Task.CompletedTask);
        }
        catch (Exception) when (ClosingRequested)
        {
            // Closing still releases owned resources if an already-observed operation failed.
        }
        finally
        {

            await WorkGate.WaitAsync();

            WorkGate.Release();

            WorkGate.Dispose();

            ClearFrames();

            Api.Dispose();

            await CleanupWork;

            await Task.Run(Media.Dispose, CancellationToken.None);

            Lifetime.Dispose();

            ResourcesReleased = true;

            Dispatcher.UIThread.Post(Close);
        }
    }

    /// <summary>Displays embedded third-party notices and license terms.</summary>
    private async Task ShowAboutAsync()
    {
        string Notices = await Task.Run(ReadNoticeText);

        await CreateAboutWindow(Notices).ShowDialog(this);
    }

    private static Window CreateAboutWindow(string? Notices = null)
    {
        Window Dialog = DialogWindow("About", 660, 540);

        SetDialogBody(Dialog, new NoticeReader(Notices ?? ReadNoticeText()));

        return Dialog;
    }

    /// <summary>Assembles original and third-party license text from embedded resources.</summary>
    private static string ReadNoticeText()
    {
        using Stream Resource = typeof(MainWindow).Assembly.GetManifestResourceStream("Payload.notices.txt") ?? new MemoryStream();

        using StreamReader Reader = new(Resource);

        using Stream Licenses = typeof(MainWindow).Assembly.GetManifestResourceStream("Payload.dependency-licenses.txt") ?? new MemoryStream();

        using StreamReader LicenseReader = new(Licenses);

        using Stream ApplicationLicense = typeof(MainWindow).Assembly.GetManifestResourceStream("Payload.application-license.txt")!;

        using StreamReader ApplicationReader = new(ApplicationLicense);

        using Stream FontLicense = typeof(MainWindow).Assembly.GetManifestResourceStream("Payload.CascadiaCode.LICENSE.txt")!;

        using StreamReader FontReader = new(FontLicense);

        return "Copyright (C) 2026 Ophura\nGPL-3.0-only\n\n" + Reader.ReadToEnd()
            + "\n\n" + LicenseReader.ReadToEnd() + "\n\nCascadia Code license\n\n" + FontReader.ReadToEnd()
            + "\n\nApplication license\n\n" + ApplicationReader.ReadToEnd();
    }

    private static TextBlock Label(string Text, double Size = 13, bool Muted = false)
    {
        return new TextBlock
        {
            Text = Text,

            FontSize = Size,

            Foreground = Brush.Parse(Muted ? "#A8A8A8" : "#EBEBEB")
        };
    }

    private static NumberField Number(decimal Value, decimal Minimum, decimal Maximum, string Name, decimal Increment = 1)
    {
        NumberField Result = new()
        {
            Value = Value,

            Minimum = Minimum,

            Maximum = Maximum,

            Increment = Increment,

            FormatString = Increment < 1 ? "0.##" : "0",

            ParsingNumberStyle = Increment < 1 ? System.Globalization.NumberStyles.AllowDecimalPoint : System.Globalization.NumberStyles.None,

            NumberFormat = System.Globalization.CultureInfo.InvariantCulture.NumberFormat,

            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        ToolTip.SetTip(Result, Name);

        AutomationProperties.SetName(Result, Name);

        return Result;
    }

    private static MotionChoice Choice(params string[] Choices)
    {
        return new MotionChoice
        {
            ItemsSource = Choices,

            SelectedIndex = 0
        };
    }

    private static StackPanel Field(string Name, Control Control)
    {
        AutomationProperties.SetName(Control, Name);

        return new StackPanel
        {
            Spacing = 7,

            Children = { new Grid { Children = { Label(Name, 11, true) } }, Control }
        };
    }

    private static Grid Pair(Control Left, Control Right)
    {
        Grid Result = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,*"),

            ColumnSpacing = 10,

            Children = { Left, Right }
        };

        Grid.SetColumn(Right, 1);

        return Result;
    }

    private static StackPanel Section(string Icon, string Name)
    {
        return new StackPanel
        {
            Orientation = Orientation.Horizontal,

            Spacing = 10,

            Children = { Icons.Make(Icon, 17, Brush.Parse("#B2B2B2")), Label(Name, 14) }
        };
    }

    private static StackPanel SliderField(string Name, Slider Slider, string Unit)
    {
        TextBlock Value = Label($"{Slider.Value:0} {Unit}", 12, true);

        Slider.ValueChanged += (_, _) => Value.Text = $"{Slider.Value:0} {Unit}";

        AutomationProperties.SetName(Slider, Name);

        Grid Header = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),

            Children = { Label(Name, 11, true), Value }
        };

        Grid.SetColumn(Value, 1);

        return new StackPanel
        {
            Spacing = 5,

            Children = { Header, Slider }
        };
    }

    private static Border Card(Control Child, double Padding)
    {
        return new Border
        {
            Background = Brush.Parse("#B0212121"),

            BorderBrush = Brush.Parse("#3B3B3B"),

            BorderThickness = new Thickness(1),

            CornerRadius = new CornerRadius(18),

            Padding = new Thickness(Padding),

            Child = Child
        };
    }

    private static Window DialogWindow(string Title, double Width, double Height)
    {
        Window Dialog = new()
        {
            Title = Title,

            CanResize = false,

            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        WindowChrome.Configure(Dialog, Width, Height);

        return Dialog;
    }

    /// <summary>Composes an unbranded dialog with the same dark window chrome.</summary>
    private static void SetDialogBody(Window Dialog, Control Body)
    {
        Grid Layout = new()
        {
            RowDefinitions = new RowDefinitions("64,*")
        };

        Layout.Children.Add(WindowChrome.Header(Dialog));

        Body.Margin = new Thickness(24, 12, 24, 24);

        Grid.SetRow(Body, 1);

        Layout.Children.Add(Body);

        Dialog.Content = Layout;
    }
}
