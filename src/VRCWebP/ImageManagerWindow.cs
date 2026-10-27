using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace VRCWebP;

/// <summary>Browses owned images using explicit pagination and confirms every destructive action.</summary>
internal sealed partial class ImageManagerWindow : Window
{
    internal event Action<string>? FileDeleted;

    private int PageSize = 6;

    private readonly Grid Catalog = null!;

    private readonly StackPanel Body = null!;

    private readonly Grid Layout = null!;

    private readonly VRChatService Api;

    private readonly CancellationToken ParentToken;

    private readonly StackPanel Rows = new() { Spacing = 6, MinHeight = 318 };

    private readonly TextBlock Status = new() { TextWrapping = TextWrapping.Wrap, FontSize = 12, MinHeight = 32, MaxLines = 2, TextTrimming = TextTrimming.CharacterEllipsis };

    private readonly TextBlock PageLabel = new() { FontSize = 11, VerticalAlignment = VerticalAlignment.Center };

    private readonly Button Previous = Icons.Button("back", "Previous images");

    private readonly Button Next = Icons.Button("arrow", "Next images");

    private readonly Button Refresh = Icons.Button("refresh", "Refresh your uploaded images");

    private readonly Button Use = Icons.TextAction("Use image");

    private readonly Button Delete = Icons.TextAction("Delete file");

    private readonly Button Copy = Icons.Button("link", "Copy selected image link");

    private readonly CancellationTokenRegistration ParentRegistration;

    private readonly MediaService PreviewMedia = new();

    private readonly Image Preview = new() { Width = 256, Height = 256, Stretch = Stretch.Uniform };

    private readonly TextBlock PreviewHint = new() { Text = "Select an image to preview", TextWrapping = TextWrapping.Wrap, FontSize = 12, MaxLines = 2, TextTrimming = TextTrimming.CharacterEllipsis };

    private readonly List<Bitmap> PreviewFrames = [];

    private readonly DispatcherTimer PreviewPlayback = new() { Interval = TimeSpan.FromSeconds(1d / 12) };

    private CancellationTokenSource? PreviewSelection;

    private Task? PreviewPending;

    private readonly HashSet<Task> PreviewWork = [];

    private readonly SemaphoreSlim PreviewDecodeGate = new(1, 1);

    private int PreviewIndex;

    private long PreviewRevision;

    private readonly Button Cancel = Icons.TextAction("Cancel");

    private RemoteImageBatch Batch = new([], 0, false);

    private RemoteImageFile? Selected;

    private int Page;

    private CancellationTokenSource? Operation;

    private Task? Pending;

    private bool ClosingRequested;

    private string? ClosingSelection;

    /// <summary>Shares the parent's authenticated service only while its foreground-operation gate is held.</summary>
    internal ImageManagerWindow(VRChatService Api, CancellationToken ParentToken)
    {
        this.Api = Api;

        this.ParentToken = ParentToken;

        Title = "Uploaded images";

        CanResize = false;

        WindowStartupLocation = WindowStartupLocation.CenterOwner;

        WindowChrome.Configure(this, 1040, 620);

        _ = Status.Bind(ToolTip.TipProperty, Status.GetObservable(TextBlock.TextProperty));

        ToolTip.SetTip(PreviewHint, "Select an image to display its cached preview.");

        ParentRegistration = ParentToken.Register(() => Dispatcher.UIThread.Post(() => Close()));

        Closed += (_, _) =>
        {
            ParentRegistration.Dispose();

            ClearPreview();

            PreviewMedia.Dispose();

            PreviewDecodeGate.Dispose();
        };

        PreviewPlayback.Tick += (_, _) =>
        {
            if (PreviewFrames.Count > 1)
            {
                PreviewIndex = (PreviewIndex + 1) % PreviewFrames.Count;

                Preview.Source = PreviewFrames[PreviewIndex];
            }
        };

        ToolTip.SetTip(Use, "Use this existing image without uploading it again");

        ToolTip.SetTip(Delete, "Delete the selected image file from your VRChat account");

        ToolTip.SetTip(Cancel, "Cancel the current file operation");

        Grid Navigation = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto,Auto,Auto"),

            ColumnSpacing = 8,

            Children = { PageLabel, Previous, Next, Refresh }
        };

        Grid.SetColumn(Previous, 1);

        Grid.SetColumn(Next, 2);

        Grid.SetColumn(Refresh, 3);

        StackPanel Actions = new()
        {
            Orientation = Orientation.Horizontal,

            Spacing = 8,

            Children = { Use, Copy, Delete, Cancel }
        };

        Border PreviewCard = new()
        {
            CornerRadius = new CornerRadius(12),

            Background = Brush.Parse("#242424"),

            Padding = new Thickness(12),

            Child = new StackPanel { Spacing = 10, Children = { Preview, PreviewHint } }
        };

        ToolTip.SetTip(Preview, "Small cached preview, up to two seconds. The full uploaded image is unchanged.");

        Catalog = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,280"),

            ColumnSpacing = 16,

            Children = { Rows, PreviewCard }
        };

        Grid.SetColumn(PreviewCard, 1);

        Body = new StackPanel
        {
            Spacing = 12,

            Children =
            {
                new TextBlock { Text = "Uploaded by " + (Api.DisplayName ?? "the connected account"), FontSize = 14 },
                Navigation,
                Catalog,
                Status,
                Actions
            }
        };

        Layout = SetBody(this, Body);

        Previous.Click += async (_, _) =>
        {
            if (Page > 0)
            {
                --Page;

                RenderRows();
            }
            else
            {
                await RunAsync(async Token =>
                {
                    await LoadAsync(Math.Max(0, Batch.Offset - VRChatService.ImageBatchSize), Token);

                    Page = Math.Max(0, (Batch.Images.Count - 1) / PageSize);

                    RenderRows();
                });
            }
        };

        Next.Click += async (_, _) =>
        {
            if ((Page + 1) * PageSize < Batch.Images.Count)
            {
                ++Page;

                RenderRows();
            }
            else
            {
                await RunAsync(Token => LoadAsync(Batch.Offset + VRChatService.ImageBatchSize, Token));
            }
        };

        Refresh.Click += async (_, _) => await RunAsync(Token => LoadAsync(0, Token));

        Use.Click += (_, _) =>
        {
            if (Selected is { Complete: true } Image)
            {
                ClosingSelection = Image.Id;

                Close(Image.Id);
            }
        };

        Copy.Click += async (_, _) =>
        {
            if (Selected is not null && Clipboard is not null)
            {
                try
                {
                    await Clipboard.SetTextAsync(Selected.Complete
                        ? $"https://api.vrchat.cloud/api/1/file/{Selected.Id}/{Selected.Version}/file" : Selected.Id);

                    UiMotion.SetText(Status, Selected.Complete ? "Image link copied." : "Recovery file ID copied.");
                }
                catch (Exception Error)
                {
                    UiMotion.SetText(Status, Error.Message);
                }
            }
        };

        Delete.Click += async (_, _) =>
        {
            RemoteImageFile? Image = Selected;

            if (Image is not null && await ConfirmDeleteAsync(Image))
            {
                await RunAsync(async Token =>
                {
                    await Task.Run(() => Api.DeleteImageAsync(Image.Id, Token), Token);

                    FileDeleted?.Invoke(Image.Id);

                    await LoadAsync(Batch.Offset, Token);

                    UiMotion.SetText(Status, "File deleted; deletion confirmed by VRChat.");
                });
            }
        };

        Cancel.Click += async (_, _) =>
        {
            if (Operation is not null)
            {
                await Operation.CancelAsync();
            }
        };

        Opened += async (_, _) =>
        {
            FitImageLayout();

            await RunAsync(Token => LoadAsync(0, Token));
        };

        Closing += async (_, Event) =>
        {
            if (Pending is { IsCompleted: false } || PreviewWork.Any(Work => !Work.IsCompleted))
            {
                Event.Cancel = true;

                if (!ClosingRequested)
                {
                    ClosingRequested = true;

                    if (Operation is not null)
                    {
                        await Operation.CancelAsync();
                    }

                    if (PreviewSelection is not null)
                    {
                        await PreviewSelection.CancelAsync();
                    }

                    try
                    {
                        await Task.WhenAll([.. PreviewWork, Pending ?? Task.CompletedTask]);
                    }
                    catch (Exception)
                    {
                        // A failed preview must not prevent the manager from closing or returning its selection.
                    }

                    Close(ClosingSelection);
                }
            }
        };

        UpdateButtons();
    }

    /// <summary>Uses fewer rows and a smaller preview before the shared monitor fitter needs to shrink the text.</summary>
    private void FitImageLayout()
    {
        bool Compact = Height < 580;

        PageSize = Compact ? 3 : 6;

        Rows.MinHeight = (PageSize * 54) - 6;

        Preview.Width = Compact ? 144 : 256;

        Preview.Height = Preview.Width;

        Catalog.ColumnDefinitions = new ColumnDefinitions(Compact ? "*,168" : "*,280");

        Body.Spacing = Compact ? 8 : 12;

        Body.Margin = Compact ? new Thickness(16, 8, 16, 16) : new Thickness(24, 12, 24, 24);

        Layout.MinWidth = Math.Min(1040, Width);

        Layout.MinHeight = Math.Min(Compact ? 450 : 620, Height);
    }

    /// <summary>Refreshes only metadata; local pages reuse the current bounded batch.</summary>
    private async Task LoadAsync(int Offset, CancellationToken Token)
    {
        UiMotion.SetText(Status, "Loading uploaded images…");

        Batch = await Task.Run(() => Api.ListImagesAsync(Offset, Token), Token);

        Page = 0;

        RenderRows();

        UiMotion.SetText(Status, Batch.Images.Count is 0 ? "No images on this page. Use Next if more files are available." : "Select an image to reuse it, copy its link or recovery ID, or delete it.");
    }

    /// <summary>Replaces a fixed set of rows with the shared crossfade rather than adding a scrollbar.</summary>
    private void RenderRows()
    {
        Selected = null;

        ++PreviewRevision;

        PreviewSelection?.Cancel();

        ClearPreview();

        UiMotion.SetText(PreviewHint, "Select an image to preview");

        UiMotion.Change(Rows, () =>
        {
            Rows.Children.Clear();

            foreach (RemoteImageFile Image in Batch.Images.Skip(Page * PageSize).Take(PageSize))
            {
                TextBlock Name = new() { Text = Image.Name, FontSize = 12, TextTrimming = TextTrimming.CharacterEllipsis };

                TextBlock Details = new()
                {
                    Text = $"{Image.Extension} record · {Image.Bytes / 1048576d:0.##} MiB · {(Image.Complete ? "ready" : "incomplete")}",

                    FontSize = 11,

                    Foreground = Brush.Parse("#B2B2B2"),

                    Transitions =
                    [
                        new BrushTransition
                        {
                            Property = TextBlock.ForegroundProperty,

                            Duration = UiMotion.Duration
                        }
                    ]
                };

                Button Row = new()
                {
                    Height = 48,

                    HorizontalAlignment = HorizontalAlignment.Stretch,

                    HorizontalContentAlignment = HorizontalAlignment.Stretch,

                    Content = new StackPanel { Spacing = 3, Children = { Name, Details } }
                };

                ToolTip.SetTip(Row, Image.Name + "\n" + Image.Id);

                Row.Click += (_, _) =>
                {
                    Selected = Image;

                    PreviewPending = ShowPreviewAsync(Image);

                    _ = PreviewWork.Add(PreviewPending);

                    _ = ObservePreviewAsync(PreviewPending);

                    foreach (Button Other in Rows.Children.OfType<Button>())
                    {
                        Other.Classes.Set("primary", ReferenceEquals(Other, Row));

                        ((TextBlock)((StackPanel)Other.Content!).Children[1]).Foreground =
                            Brush.Parse(ReferenceEquals(Other, Row) ? "#252525" : "#B2B2B2");
                    }

                    UpdateButtons();
                };

                Rows.Children.Add(Row);
            }
        });

        int First = Batch.Images.Count is 0 ? 0 : (Page * PageSize) + 1;

        UiMotion.SetText(PageLabel, $"{First}–{Math.Min((Page + 1) * PageSize, Batch.Images.Count)} of {Batch.Images.Count} loaded images");

        UpdateButtons();
    }

    /// <summary>Debounces selection, shares cached bytes and discards decoded frames from superseded selections.</summary>
    private async Task ShowPreviewAsync(RemoteImageFile Image)
    {
        long Revision = ++PreviewRevision;

        PreviewSelection?.Cancel();

        using CancellationTokenSource Current = CancellationTokenSource.CreateLinkedTokenSource(ParentToken);

        PreviewSelection = Current;

        ClearPreview();

        if (!Image.Complete)
        {
            UiMotion.SetText(PreviewHint, "Preview unavailable until this image is complete.");

            PreviewSelection = null;

            return;
        }

        UiMotion.SetText(PreviewHint, "Loading cached preview…");

        string PreviewStage = "Read cached image preview";

        try
        {
            await Task.Delay(180, Current.Token);

            string Path = await Api.ImagePreviewAsync(Image, Current.Token);

            PreviewStage = "Decode cached image preview";

            await PreviewDecodeGate.WaitAsync(Current.Token);

            List<Bitmap> Frames;

            try
            {
                Frames = await Task.Run(async () =>
                {
                    ConversionSettings Settings = new(256, 256, 12, 0, 2, "Contain", 0, 0);

                    using MediaWorkerLimit Limit = new(268435456);

                    string[] Files = await PreviewMedia.PreviewFramesAsync(Path, Settings, Current.Token, 2, WorkerLimit: Limit);

                    List<Bitmap> Decoded = [];

                    try
                    {
                        foreach (string File in Files)
                        {
                            Current.Token.ThrowIfCancellationRequested();

                            Decoded.Add(new Bitmap(File));
                        }

                        return Decoded;
                    }
                    catch
                    {
                        Decoded.ForEach(Frame => Frame.Dispose());

                        throw;
                    }
                    finally
                    {
                        foreach (string File in Files)
                        {
                            System.IO.File.Delete(File);
                        }

                        Directory.Delete(System.IO.Path.GetDirectoryName(Files[0])!);
                    }
                }, Current.Token);
            }
            finally
            {
                _ = PreviewDecodeGate.Release();
            }

            if (Revision != PreviewRevision || Current.IsCancellationRequested || ClosingRequested)
            {
                Frames.ForEach(Frame => Frame.Dispose());

                return;
            }

            PreviewFrames.AddRange(Frames);

            PreviewIndex = 0;

            UiMotion.Change(Preview, () => Preview.Source = Frames[0]);

            UiMotion.SetText(PreviewHint, "Cached preview · up to 2 s");

            ToolTip.SetTip(PreviewHint, "Cached preview · up to 2 s");

            if (Frames.Count > 1)
            {
                PreviewPlayback.Start();
            }
        }
        catch (OperationCanceledException)
        {
            // A newer selection or a closed window owns the display now.
        }
        catch (Exception Error)
        {
            await VRChatService.LogPreviewFailureAsync(PreviewStage, Error);

            if (Revision == PreviewRevision && !ClosingRequested)
            {
                string Reason = Error is ServerRequestException Server ? Server.SafeExplanation : ServerExplanation.Sanitize(Error.Message);

                UiMotion.SetText(PreviewHint, "Preview unavailable. File actions remain available.");

                ToolTip.SetTip(PreviewHint, "Preview unavailable: " + Reason);
            }
        }
        finally
        {
            if (ReferenceEquals(PreviewSelection, Current))
            {
                PreviewSelection = null;
            }
        }
    }

    private void ClearPreview()
    {
        PreviewPlayback.Stop();

        UiMotion.Change(Preview, () => Preview.Source = null);

        PreviewFrames.ForEach(Frame => Frame.Dispose());

        PreviewFrames.Clear();
    }

    private async Task ObservePreviewAsync(Task Work)
    {
        try
        {
            await Work;
        }
        catch (Exception)
        {
            // Closing still owns cleanup if an already detached preview surface failed.
        }
        finally
        {
            _ = PreviewWork.Remove(Work);
        }
    }

    private Task RunAsync(Func<CancellationToken, Task> Action)
    {
        if (Operation is not null || ClosingRequested)
        {
            return Task.CompletedTask;
        }

        Pending = RunCoreAsync(Action);

        return Pending;
    }

    /// <summary>Serializes cancellable work and keeps network parsing and transfers off the GUI thread.</summary>
    private async Task RunCoreAsync(Func<CancellationToken, Task> Action)
    {
        using CancellationTokenSource Current = CancellationTokenSource.CreateLinkedTokenSource(ParentToken);

        Current.CancelAfter(TimeSpan.FromMinutes(3));

        Operation = Current;

        UpdateButtons();

        try
        {
            await Action(Current.Token);
        }
        catch (OperationCanceledException)
        {
            UiMotion.SetText(Status, "Cancelled. Refresh to check the latest server state.");
        }
        catch (Exception Error)
        {
            UiMotion.SetText(Status, Error.Message);
        }
        finally
        {
            Operation = null;

            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        bool Idle = Operation is null && !ClosingRequested;

        Previous.IsEnabled = Idle && (Page > 0 || Batch.Offset > 0);

        Next.IsEnabled = Idle && ((Page + 1) * PageSize < Batch.Images.Count || Batch.HasMore);

        Refresh.IsEnabled = Idle;

        Rows.IsEnabled = Idle;

        Use.IsEnabled = Idle && Selected is { Complete: true };

        Copy.IsEnabled = Idle && Selected is not null;

        ToolTip.SetTip(Copy, Selected is { Complete: false } ? "Copy recovery file ID" : "Copy selected image link");

        Delete.IsEnabled = Idle && Selected is not null;

        ToolTip.SetTip(Use, Selected is { Complete: false } ? "Wait until the file is complete before reusing it" : "Use this existing image without uploading it again");

        UiMotion.SetVisible(Cancel, !Idle);
    }

    /// <summary>Requires explicit confirmation because deleting a referenced file can break published images.</summary>
    private async Task<bool> ConfirmDeleteAsync(RemoteImageFile Image)
    {
        Window Dialog = new() { CanResize = false, WindowStartupLocation = WindowStartupLocation.CenterOwner };

        WindowChrome.Configure(Dialog, 540, 290);

        Button Confirm = Icons.TextAction("Delete file");

        Button Keep = Icons.TextAction("Keep file");

        ToolTip.SetTip(Confirm, "Permanently delete this file and all its versions");

        ToolTip.SetTip(Keep, "Keep the uploaded image");

        Confirm.Click += (_, _) => Dialog.Close(true);

        Keep.Click += (_, _) => Dialog.Close(false);

        TextBlock Prompt = new()
        {
            Text = "Delete this uploaded image and all its versions? Avatars or groups using it may lose their image. This cannot be undone.",

            TextWrapping = TextWrapping.Wrap,

            FontSize = 13
        };

        _ = SetBody(Dialog, new StackPanel
        {
            Spacing = 16,

            Children =
            {
                Prompt,
                new TextBlock { Text = Image.Name, TextTrimming = TextTrimming.CharacterEllipsis, FontSize = 12 },
                new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10, Children = { Keep, Confirm } }
            }
        });

        return await Dialog.ShowDialog<bool>(this);
    }

    private static Grid SetBody(Window Window, Control Body)
    {
        Grid Layout = new() { RowDefinitions = new RowDefinitions("64,*") };

        Layout.Children.Add(WindowChrome.Header(Window));

        Body.Margin = new Thickness(24, 12, 24, 24);

        Grid.SetRow(Body, 1);

        Layout.Children.Add(Body);

        Window.Content = Layout;

        return Layout;
    }
}
