using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using System.Buffers;
using System.Diagnostics;
using System.Globalization;

namespace VRCWebP;

/// <summary>Streams the committed WebP at its native canvas and cadence using two reusable native image buffers.</summary>
internal sealed class EncodedPreviewSession : IAsyncDisposable
{
    private static readonly ArrayPool<byte> FramePool = ArrayPool<byte>.Create(67108864, 1);

    private readonly ConversionResult Result;

    private readonly Action<WriteableBitmap, double> Present;

    private readonly Action<Exception> Failed;

    private readonly Action Ended;

    private readonly CancellationTokenSource Lifetime;

    private readonly SemaphoreSlim Wake = new(0, 1);

    private readonly Lock Gate = new();

    private readonly TaskCompletionSource Ready = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private TaskCompletionSource RequestReady = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private CancellationTokenSource? Segment;

    private Task? Worker;

    private Task? Shutdown;

    private long Revision = 1;

    private double Requested;

    private double CurrentPosition;

    private bool Playing = true;

    private bool LoadRequested = true;

    private bool Loaded;

    private bool Finished;

    private bool Disposed;

    private int FrontIndex = 1;

    internal double Duration { get; }

    internal double FrameRate => Result.Frames / Duration;

    internal string Path => Result.Path;

    internal WriteableBitmap? CurrentFrame { get; private set; }

    internal int Width => Result.Width;

    internal int Height => Result.Height;

    internal bool IsPlaying
    {
        get
        {
            lock (Gate)
            {
                return Playing;
            }
        }
    }

    internal double Position
    {
        get
        {
            lock (Gate)
            {
                return CurrentPosition;
            }
        }
    }


    internal EncodedPreviewSession(ConversionResult Result, Action<WriteableBitmap, double> Present, Action<Exception> Failed,
        Action Ended, CancellationToken Token)
    {
        this.Result = Result;

        this.Present = Present;

        this.Failed = Failed;

        this.Ended = Ended;

        Lifetime = CancellationTokenSource.CreateLinkedTokenSource(Token);

        RequestReady = Ready;

        Duration = Math.Round(Result.Frames * 1000d / Result.Settings.FrameRate) / 1000d;
    }

    /// <summary>Starts once; caller cancellation stops the startup wait, while the session owns continued playback.</summary>
    internal Task StartAsync(CancellationToken Token)
    {
        Worker ??= Task.Run(RunAsync, CancellationToken.None);

        return Ready.Task.WaitAsync(TimeSpan.FromSeconds(30), Token);
    }

    internal void SetPlaying(bool Value)
    {
        lock (Gate)
        {
            if (Disposed)
            {
                return;
            }

            Playing = Value;

            if (Value && !Loaded)
            {
                RequestLocked(Finished ? 0 : CurrentPosition);
            }

            SignalLocked();
        }
    }

    /// <summary>Replaces a pending seek and shows its requested frame without automatically resuming playback.</summary>
    internal Task SeekAsync(double Position, CancellationToken Token)
    {
        Token.ThrowIfCancellationRequested();

        lock (Gate)
        {
            ObjectDisposedException.ThrowIf(Disposed, this);

            Playing = false;

            RequestLocked(Math.Clamp(Position, 0, Duration));

            return RequestReady.Task.WaitAsync(TimeSpan.FromSeconds(30), Token);
        }
    }

    /// <summary>Leaves the last displayed frame intact and stops an uncompleted seek until Play or another seek.</summary>
    internal void CancelSeek()
    {
        lock (Gate)
        {
            Playing = false;

            LoadRequested = false;

            Loaded = false;

            ++Revision;

            Segment?.Cancel();

            RequestReady.TrySetCanceled();

            SignalLocked();
        }
    }

    private void RequestLocked(double Position)
    {
        Requested = Position;

        LoadRequested = true;

        Loaded = false;

        Finished = false;

        ++Revision;

        RequestReady.TrySetCanceled();

        RequestReady = new(TaskCreationOptions.RunContinuationsAsynchronously);

        Segment?.Cancel();

        SignalLocked();
    }

    private void SignalLocked()
    {
        if (!Disposed && Wake.CurrentCount is 0)
        {
            Wake.Release();
        }
    }

    /// <summary>Serializes decoder replacement and observes every worker before releasing its buffers.</summary>
    private async Task RunAsync()
    {
        WriteableBitmap[] Buffers = [];

        try
        {
            Lifetime.Token.ThrowIfCancellationRequested();

            Buffers = new WriteableBitmap[2];

            for (int Index = 0; Index < Buffers.Length; ++Index)
            {
                Buffers[Index] = new(new PixelSize(Result.Width, Result.Height), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Unpremul);
            }

            while (true)
            {
                Lifetime.Token.ThrowIfCancellationRequested();

                long Visit;

                double Start;

                TaskCompletionSource Completion;

                using CancellationTokenSource Current = CancellationTokenSource.CreateLinkedTokenSource(Lifetime.Token);

                lock (Gate)
                {
                    Visit = Revision;

                    Start = Requested;

                    Completion = RequestReady;

                    Segment = Current;
                }

                try
                {
                    bool Load;

                    lock (Gate)
                    {
                        Load = LoadRequested;
                    }

                    if (!Load)
                    {
                        await Wake.WaitAsync(Current.Token);

                        continue;
                    }

                    await DecodeAsync(Buffers, Start, Visit, Completion, Current.Token);

                    lock (Gate)
                    {
                        if (Revision == Visit)
                        {
                            Playing = false;

                            Loaded = false;

                            LoadRequested = false;

                            Finished = true;

                            CurrentPosition = Duration;
                        }
                    }

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        bool CurrentVisit;

                        lock (Gate)
                        {
                            CurrentVisit = Revision == Visit && Finished;
                        }

                        if (CurrentVisit)
                        {
                            Ended();
                        }
                    });

                    await WaitForRequestAsync(Visit);
                }
                catch (OperationCanceledException) when (!Lifetime.IsCancellationRequested)
                {
                    // A newer seek owns the next decoder. Cleanup is joined in DecodeAsync.
                }
                catch (Exception) when (Current.IsCancellationRequested)
                {
                    Lifetime.Token.ThrowIfCancellationRequested();
                }
                catch (Exception Error) when (Error is not OperationCanceledException)
                {
                    Ready.TrySetException(Error);

                    Completion.TrySetException(Error);

                    _ = Completion.Task.Exception;

                    _ = Ready.Task.Exception;

                    lock (Gate)
                    {
                        if (Revision == Visit)
                        {
                            Playing = false;

                            Loaded = false;

                            LoadRequested = false;
                        }
                    }

                    await Dispatcher.UIThread.InvokeAsync(() => Failed(Error));

                    await WaitForRequestAsync(Visit);
                }
                finally
                {
                    lock (Gate)
                    {
                        if (ReferenceEquals(Segment, Current))
                        {
                            Segment = null;
                        }
                    }
                }
            }
        }
        catch (OperationCanceledException) when (Lifetime.IsCancellationRequested)
        {
            Ready.TrySetCanceled(Lifetime.Token);
        }
        catch (Exception Error)
        {
            Ready.TrySetException(Error);

            _ = Ready.Task.Exception;

            await Dispatcher.UIThread.InvokeAsync(() => Failed(Error));
        }
        finally
        {
            CurrentFrame = null;

            foreach (WriteableBitmap? Buffer in Buffers)
            {
                Buffer?.Dispose();
            }

            lock (Gate)
            {
                _ = RequestReady.TrySetCanceled();
            }
        }
    }

    private async Task WaitForRequestAsync(long Visit)
    {
        while (true)
        {
            lock (Gate)
            {
                if (Revision != Visit)
                {
                    return;
                }
            }

            await Wake.WaitAsync(Lifetime.Token);
        }
    }

    /// <summary>Reads raw native-sized frames directly into a back buffer; there is no per-frame image list or byte-array allocation.</summary>
    private async Task DecodeAsync(WriteableBitmap[] Buffers, double RequestedStart, long Visit, TaskCompletionSource Completion, CancellationToken Token)
    {
        string Binary = Payload.Get("ffmpeg.exe");

        double Start = Math.Min(RequestedStart, Math.Max(0, Duration - (1 / FrameRate)));

        string Arguments = string.Create(CultureInfo.InvariantCulture,
            $"-hide_banner -nostdin -v error -filter_threads 1 -threads 2 -ignore_loop 0 -i {MediaService.Quote(Result.Path)} -ss {Start:R} -an -sn -dn -map_metadata -1 -map_chapters -1 -fps_mode passthrough -threads:v 1 -pix_fmt bgra -f rawvideo pipe:1");

        using Process Process = new()
        {
            StartInfo = new ProcessStartInfo(Binary, Arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        using MediaWorkerLimit Limit = new(536870912);

        _ = Process.Start();

        try
        {
            Limit.Attach(Process);
        }
        catch
        {
            MediaService.StopProcess(Process);

            await Process.WaitForExitAsync(CancellationToken.None);

            throw;
        }

        using CancellationTokenRegistration Registration = Token.Register(() => MediaService.StopProcess(Process));

        Task<string> Errors = MediaService.ReadBoundedAsync(Process.StandardError, 8192);

        long Clock = Stopwatch.GetTimestamp();

        long Ordinal = 0;

        using PooledBuffer Pixels = new(checked(Result.Width * Result.Height * 4), FramePool);

        try
        {
            while (true)
            {
                int BufferIndex = 1 - Volatile.Read(ref FrontIndex);

                WriteableBitmap Buffer = Buffers[BufferIndex];

                int Count = await Process.StandardOutput.BaseStream.ReadAtLeastAsync(Pixels.Memory, Pixels.Length, false, Token);

                bool Read = Count > 0;

                if (Read && Count != Pixels.Length)
                {
                    throw new EndOfStreamException("The encoded preview frame is incomplete.");
                }

                if (!Read)
                {
                    break;
                }


                if (Ordinal > 0)
                {
                    bool Paused = false;

                    while (!IsPlaying)
                    {
                        Paused = true;

                        await Wake.WaitAsync(Token);
                    }

                    if (Paused)
                    {
                        Clock = Stopwatch.GetTimestamp() - (long)(Ordinal / FrameRate * Stopwatch.Frequency);
                    }

                    double Remaining = (Ordinal / FrameRate) - Stopwatch.GetElapsedTime(Clock).TotalSeconds;

                    if (Remaining > 0)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Remaining), Token);
                    }
                }

                Token.ThrowIfCancellationRequested();

                double Position = (Start + (Ordinal / FrameRate)) % Duration;

                bool First = Ordinal is 0;

                // Decode sequentially but skip stale presentations when rendering cannot keep up.
                if (First || Stopwatch.GetElapsedTime(Clock).TotalSeconds - (Ordinal / FrameRate) < 2 / FrameRate)
                {
                    CopyFrame(Buffer, Pixels.Memory.Span);

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        lock (Gate)
                        {
                            if (Disposed || Revision != Visit || (!First && !Playing))
                            {
                                return;
                            }

                            CurrentPosition = First && !Playing ? RequestedStart : Position;

                            Loaded = true;
                        }

                        Volatile.Write(ref FrontIndex, BufferIndex);

                        CurrentFrame = Buffer;

                        Present(Buffer, CurrentPosition);

                    });
                }

                if (First)
                {
                    Clock = Stopwatch.GetTimestamp();

                    Ready.TrySetResult();

                    Completion.TrySetResult();
                }

                ++Ordinal;
            }

            double FinalDelay = (Ordinal / FrameRate) - Stopwatch.GetElapsedTime(Clock).TotalSeconds;

            if (FinalDelay > 0 && IsPlaying)
            {
                await Task.Delay(TimeSpan.FromSeconds(FinalDelay), Token);
            }

            await Process.WaitForExitAsync(Token);

            string Error = await Errors;

            Token.ThrowIfCancellationRequested();

            if (Process.ExitCode is not 0 || Ordinal is 0)
            {
                throw new InvalidDataException(string.IsNullOrWhiteSpace(Error) ? "The encoded preview decoder returned no usable frames." : Error.Trim()[..Math.Min(1000, Error.Trim().Length)]);
            }
        }
        finally
        {
            MediaService.StopProcess(Process);

            await Process.WaitForExitAsync(CancellationToken.None);

            _ = await Errors;

        }
    }

    /// <summary>Holds the native framebuffer lease only during a synchronous worker-thread copy; it never crosses an await.</summary>
    private unsafe void CopyFrame(WriteableBitmap Destination, ReadOnlySpan<byte> Pixels)
    {
        using ILockedFramebuffer Frame = Destination.Lock();

        int Stride = checked(Result.Width * 4);

        if (Frame.RowBytes == Stride)
        {
            Pixels.CopyTo(new Span<byte>((void*)Frame.Address, Pixels.Length));
        }
        else
        {
            for (int Row = 0; Row < Result.Height; ++Row)
            {
                Pixels.Slice(Row * Stride, Stride).CopyTo(new Span<byte>((byte*)Frame.Address + (Row * Frame.RowBytes), Stride));
            }
        }
    }

    public ValueTask DisposeAsync()
    {
        lock (Gate)
        {
            return new ValueTask(Shutdown ??= StopAsync());
        }
    }

    private async Task StopAsync()
    {
        Disposed = true;

        await Lifetime.CancelAsync();

        await (Worker ?? Task.CompletedTask);

        Wake.Dispose();

        Lifetime.Dispose();
    }

}
