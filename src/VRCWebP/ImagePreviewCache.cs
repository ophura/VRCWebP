using System.Security.Cryptography;
using System.Text;

namespace VRCWebP;

/// <summary>Persists small versioned previews, coalesces requests and bounds disk, worker memory and concurrency.</summary>
internal sealed class ImagePreviewCache : IDisposable
{
    internal const long DiskBudget = 134217728;

    internal const long MaximumPreviewBytes = 2097152;

    private readonly string Root;

    private readonly SemaphoreSlim Workers = new(2, 2);

    private readonly CancellationTokenSource Lifetime = new();

    private readonly Dictionary<string, (Task<string> Task, CancellationTokenSource Stop)> Pending = new(StringComparer.Ordinal);

    private readonly Lock Gate = new();

    private readonly Lock DiskGate = new();

    private readonly Dictionary<string, DateTimeOffset> Failed = new(StringComparer.Ordinal);

    private bool Disposed;

    private readonly Func<string, string, CancellationToken, Task> Convert;

    internal ImagePreviewCache()
        : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VRCWebP", "previews"), ConvertAsync)
    {
    }

    internal ImagePreviewCache(string Root)
        : this(Root, ConvertAsync)
    {
    }

    internal ImagePreviewCache(string Root, Func<string, string, CancellationToken, Task> Convert)
    {
        this.Root = Path.GetFullPath(Root);

        this.Convert = Convert;
    }

    /// <summary>Generates directly from local encoded bytes, before their temporary workspace is released.</summary>
    internal Task<string> StoreLocalAsync(string Account, string FileId, int Version, string Source, CancellationToken Token) =>
        GetAsync(Account, FileId, Version, (_, _) => Task.FromResult(Source), Token, Local: true);

    /// <summary>Shares one fetch/generation per immutable version; cancelling a selection does not discard a useful in-flight fetch.</summary>
    internal Task<string> GetAsync(string Account, string FileId, int Version, Func<string, CancellationToken, Task<string>> Fetch, CancellationToken Token, bool Local = false)
    {
        string Destination = PreviewPath(Account, FileId, Version);

        Task<string> Work;

        lock (Gate)
        {
            ObjectDisposedException.ThrowIf(Disposed, this);

            if (Failed.TryGetValue(Destination, out DateTimeOffset RetryAt) && RetryAt > DateTimeOffset.UtcNow && !Local)
            {
                // A completed local upload or another window may have populated the cache since the failed request.
                return Task.Run(async () =>
                {
                    if (await IsValidAsync(Destination, Token))
                    {
                        lock (Gate)
                        {
                            _ = Failed.Remove(Destination);
                        }

                        return Destination;
                    }

                    throw new IOException("The preview is temporarily unavailable. It will not be repeatedly requested.");
                }, Token);
            }

            if (Pending.TryGetValue(Destination, out (Task<string> Task, CancellationTokenSource Stop) Existing))
            {
                Work = Existing.Task;
            }
            else
            {
                CancellationTokenSource Stop = CancellationTokenSource.CreateLinkedTokenSource(Lifetime.Token, Local ? Token : CancellationToken.None);

                Stop.CancelAfter(TimeSpan.FromSeconds(45));

                Work = Task.Run(() => BuildAsync(Destination, Fetch, Local, Stop.Token), Stop.Token);

                Pending.Add(Destination, (Work, Stop));

                _ = ReleaseAsync(Destination, Work, Stop);
            }
        }

        return Work.WaitAsync(Token);
    }

    /// <summary>Cancels generators for a deleted record before removing only that record's cached previews.</summary>
    internal async Task RemoveAsync(string Account, string FileId)
    {
        string Folder = Path.GetDirectoryName(PreviewPath(Account, FileId, 1))!;

        (Task<string> Task, CancellationTokenSource Stop)[] Matches;

        lock (Gate)
        {
            Matches = [.. Pending.Where(Item => Path.GetDirectoryName(Item.Key) == Folder).Select(Item => Item.Value)];

            foreach ((Task<string> _, CancellationTokenSource Stop) in Matches)
            {
                Stop.Cancel();
            }
        }

        foreach ((Task<string> Work, _) in Matches)
        {
            try
            {
                _ = await Work;
            }
            catch (Exception)
            {
                // Deletion owns the final invalidation even if a pending preview failed.
            }
        }

        await Task.Run(() =>
        {
            if (Directory.Exists(Folder))
            {
                foreach (string File in Directory.EnumerateFiles(Folder))
                {
                    System.IO.File.Delete(File);
                }

                Directory.Delete(Folder);
            }
        });
    }

    private string PreviewPath(string Account, string FileId, int Version)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(Version, 1);

        Span<byte> Identity = stackalloc byte[256];

        int Count = Encoding.UTF8.GetBytes(Account.AsSpan(), Identity);

        Identity[Count++] = 0;

        Count += Encoding.UTF8.GetBytes(FileId.AsSpan(), Identity[Count..]);

        Span<byte> Digest = stackalloc byte[32];

        _ = SHA256.HashData(Identity[..Count], Digest);

        string Folder = System.Convert.ToHexStringLower(Digest);

        return Path.Combine(Root, Folder, Version.ToString(System.Globalization.CultureInfo.InvariantCulture) + ".webp");
    }

    /// <summary>Runs file I/O and media work on a worker, committing only a complete bounded preview.</summary>
    private async Task<string> BuildAsync(string Destination, Func<string, CancellationToken, Task<string>> Fetch, bool Local, CancellationToken Token)
    {
        await Workers.WaitAsync(Token);

        string? Raw = null;

        string? Temporary = null;

        FileStream? Lease = null;

        try
        {
            using (FileStream CleanupLease = await AcquireIndexLeaseAsync(Token))
            {
                ReclaimTemporaryFiles();

                Prune(Destination);
            }

            if (await IsValidAsync(Destination, Token))
            {
                File.SetLastWriteTimeUtc(Destination, DateTime.UtcNow);

                lock (Gate)
                {
                    _ = Failed.Remove(Destination);
                }

                return Destination;
            }

            string Folder = Path.GetDirectoryName(Destination)!;

            _ = Directory.CreateDirectory(Folder);

            string Work = Path.Combine(Folder, Guid.NewGuid().ToString("N"));

            Lease = new FileStream(Work + ".lease", FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 1, FileOptions.DeleteOnClose);

            Raw = Work + ".download";

            Temporary = Work + ".webp.tmp";

            string Source = await Fetch(Raw, Token);

            await Convert(Source, Temporary, Token);

            long Bytes = new FileInfo(Temporary).Length;

            if (Bytes is <= 0 or > MaximumPreviewBytes)
            {
                throw new InvalidDataException("The generated preview exceeds its bounded cache size.");
            }

            Token.ThrowIfCancellationRequested();

            byte[] Digest;

            await using (FileStream Encoded = new(Temporary, FileMode.Open, FileAccess.Read, FileShare.Read, 65536, FileOptions.Asynchronous))
            {
                Digest = await SHA256.HashDataAsync(Encoded, Token);
            }

            using FileStream IndexLease = await AcquireIndexLeaseAsync(Token);

            File.Move(Temporary, Destination, true);

            Temporary = null;

            await File.WriteAllBytesAsync(Destination + ".digest", Digest, Token);

            if (Local)
            {
                // Prefer retaining previews whose source came from this machine when pruning.
                File.WriteAllBytes(Destination + ".local", []);
            }

            Prune(Destination);

            lock (Gate)
            {
                _ = Failed.Remove(Destination);
            }

            return Destination;
        }
        finally
        {
            try
            {
                if (Raw is not null)
                {
                    File.Delete(Raw);
                }

                if (Temporary is not null)
                {
                    File.Delete(Temporary);
                }
            }
            finally
            {
                Lease?.Dispose();

                _ = Workers.Release();
            }
        }
    }

    private async Task ReleaseAsync(string Destination, Task<string> Work, CancellationTokenSource Stop)
    {
        try
        {
            _ = await Work.ConfigureAwait(false);
        }
        catch (Exception)
        {
            // The requesting caller observes failures; no abandoned task remains unobserved.
            lock (Gate)
            {
                if (!Stop.IsCancellationRequested)
                {
                    if (Failed.Count >= 128)
                    {
                        Failed.Clear();
                    }

                    Failed[Destination] = DateTimeOffset.UtcNow.AddMinutes(1);
                }
            }
        }
        finally
        {
            lock (Gate)
            {
                _ = Pending.Remove(Destination);

                Stop.Dispose();
            }
        }
    }

    /// <summary>Serializes commit and eviction across application processes without blocking the UI dispatcher.</summary>
    private async Task<FileStream> AcquireIndexLeaseAsync(CancellationToken Token)
    {
        _ = Directory.CreateDirectory(Root);

        while (true)
        {
            Token.ThrowIfCancellationRequested();

            try
            {
                return new FileStream(Path.Combine(Root, ".index.lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 1);
            }
            catch (IOException)
            {
                await Task.Delay(25, Token);
            }
        }
    }

    /// <summary>Reclaims only owned abandoned work; a live process holds its lease with FileShare.None.</summary>
    private void ReclaimTemporaryFiles()
    {
        if (!Directory.Exists(Root))
        {
            return;
        }

        foreach (string LeasePath in Directory.EnumerateFiles(Root, "*.lease", SearchOption.AllDirectories))
        {
            try
            {
                using FileStream Lease = new(LeasePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 1, FileOptions.DeleteOnClose);
                string Work = LeasePath[..^6];
                File.Delete(Work + ".download");
                File.Delete(Work + ".webp.tmp");
            }
            catch (IOException)
            {
                // Another cache instance still owns this generation.
            }
        }

        foreach (string Path in Directory.EnumerateFiles(Root, "*", SearchOption.AllDirectories))
        {
            bool Download = Path.EndsWith(".download", StringComparison.Ordinal);

            bool Temporary = Path.EndsWith(".webp.tmp", StringComparison.Ordinal);

            bool Sidecar = Path.EndsWith(".digest", StringComparison.Ordinal) || Path.EndsWith(".local", StringComparison.Ordinal);

            string Base = Download ? Path[..^9] : Temporary ? Path[..^9] : "";

            try
            {
                // Compatibility cleanup for pre-lease work requires an age comfortably beyond
                // the 45-second generation deadline. New work is protected by its live lease.
                if (((Download || Temporary) && !File.Exists(Base + ".lease")
                    && (new FileInfo(Path).Length > UploadPolicy.AvatarGalleryMaximumBytes
                        || File.GetLastWriteTimeUtc(Path) < DateTime.UtcNow.AddMinutes(-10)))
                    || (Sidecar && !File.Exists(Path[..Path.LastIndexOf('.')])))
                {
                    using (FileStream Exclusive = new(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                    }

                    File.Delete(Path);
                }
            }
            catch (IOException)
            {
                // A competing reader/generator or pruner won the race.
            }
        }
    }

    /// <summary>Evicts least-recently-used remote previews first, with a hard total-byte bound.</summary>
    private void Prune(string Current)
    {
        lock (DiskGate)
        {
            HashSet<string> Protected;

            lock (Gate)
            {
                Protected = Pending.Keys.ToHashSet(StringComparer.Ordinal);
            }

            FileInfo[] Files = [.. new DirectoryInfo(Root).EnumerateFiles("*.webp", SearchOption.AllDirectories)];

            long Total = Files.Sum(File => File.Length + SidecarBytes(File.FullName));

            static long SidecarBytes(string Path)
            {
                FileInfo Digest = new(Path + ".digest");

                FileInfo Local = new(Path + ".local");

                return (Digest.Exists ? Digest.Length : 0) + (Local.Exists ? Local.Length : 0);
            }

            foreach (FileInfo File in Files.OrderBy(File => System.IO.File.Exists(File.FullName + ".local")).ThenBy(File => File.LastWriteTimeUtc))
            {
                if (Total <= DiskBudget)
                {
                    break;
                }

                if (File.FullName == Current || Protected.Contains(File.FullName))
                {
                    continue;
                }

                try
                {
                    long Bytes = File.Length + SidecarBytes(File.FullName);

                    File.Delete();

                    System.IO.File.Delete(File.FullName + ".local");

                    System.IO.File.Delete(File.FullName + ".digest");

                    Total -= Bytes;
                }
                catch (IOException)
                {
                    // A currently decoded preview may be locked briefly; another victim can be used.
                }
            }
        }
    }

    private static async Task ConvertAsync(string Source, string Destination, CancellationToken Token)
    {
        string Binary = Payload.Get("ffmpeg.exe");

        using MediaWorkerLimit Limit = new(268435456);

        string Filter = "fps=6:round=near:eof_action=pass,scale=256:256:force_original_aspect_ratio=decrease,pad=256:256:(ow-iw)/2:(oh-ih)/2:color=0x00000000";

        _ = await MediaService.RunAsync(Binary, "-hide_banner -nostdin -v error -threads 2 -filter_threads 1 -i " + MediaService.Quote(Source)
            + " -t 2 -an -sn -dn -map_metadata -1 -map_chapters -1 -vf " + MediaService.Quote(Filter)
            + " -frames:v 12 -c:v libwebp_anim -lossless 0 -quality 60 -compression_level 0 -loop 0 -f webp -y " + MediaService.Quote(Destination), null, Token, Limit);
    }

    private static async Task<bool> IsValidAsync(string Path, CancellationToken Token)
    {
        try
        {
            if (!File.Exists(Path) || new FileInfo(Path).Length is <= 0 or > MaximumPreviewBytes
                || !File.Exists(Path + ".digest") || new FileInfo(Path + ".digest").Length is not 32)
            {
                return false;
            }

            byte[] Expected = await File.ReadAllBytesAsync(Path + ".digest", Token);

            await using FileStream Source = new(Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete, 65536, FileOptions.Asynchronous);

            byte[] Actual = await SHA256.HashDataAsync(Source, Token);

            return CryptographicOperations.FixedTimeEquals(Expected, Actual);
        }
        catch (IOException)
        {
            return false;
        }
    }

    public void Dispose()
    {
        Task<string>[] Work;

        lock (Gate)
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;

            Work = [.. Pending.Values.Select(Item => Item.Task)];
        }

        Lifetime.Cancel();

        _ = DisposeWorkersAsync(Work);
    }

    private async Task DisposeWorkersAsync(Task<string>[] Work)
    {
        try
        {
            _ = await Task.WhenAll(Work);
        }
        catch (Exception)
        {
            // Cancellation and generation errors have already been observed by each caller.
        }
        finally
        {
            Workers.Dispose();

            Lifetime.Dispose();
        }
    }
}
