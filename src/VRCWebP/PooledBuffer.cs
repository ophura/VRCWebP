using System.Buffers;

namespace VRCWebP;

/// <summary>Owns a bounded byte-array lease and erases its contents before returning it to the shared pool.</summary>
internal sealed class PooledBuffer : IDisposable
{
    private byte[]? Buffer;

    private readonly ArrayPool<byte> Pool;


    internal int Length { get; set; }

    internal Memory<byte> Memory => (Buffer ?? throw new ObjectDisposedException(nameof(PooledBuffer))).AsMemory(0, Length);

    internal PooledBuffer(int Length) : this(Length, ArrayPool<byte>.Shared)
    {
    }

    /// <summary>Uses the supplied pool for the entire lease; consumers relinquish all views at Dispose.</summary>
    internal PooledBuffer(int Length, ArrayPool<byte> Pool)
    {
        this.Pool = Pool;

        Buffer = Pool.Rent(Length);

        this.Length = Length;

    }

    public void Dispose()
    {
        byte[]? Returned = Interlocked.Exchange(ref Buffer, null);

        if (Returned is not null)
        {
            Pool.Return(Returned, clearArray: true);

        }
    }
}
