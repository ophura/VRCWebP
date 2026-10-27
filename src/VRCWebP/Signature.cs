using Org.BouncyCastle.Crypto.Digests;
using System.Buffers.Binary;

namespace VRCWebP;

/// <summary>Produces the librsync MD4/rollsum signature used by VRChat's versioned file protocol.</summary>
internal static class Signature
{
    /// <summary>Writes a network-order header followed by 2048-byte block checksums.</summary>
    internal static byte[] Create(string Path, CancellationToken CancellationToken)
    {
        using FileStream Input = File.OpenRead(Path);

        byte[] Output = new byte[checked(12 + ((int)((Input.Length + 2047) / 2048) * 20))];

        Span<byte> Header = Output.AsSpan(0, 12);

        BinaryPrimitives.WriteUInt32BigEndian(Header, 0x72730136);

        BinaryPrimitives.WriteUInt32BigEndian(Header[4..], 2048);

        BinaryPrimitives.WriteUInt32BigEndian(Header[8..], 16);

        Span<byte> Block = stackalloc byte[2048];

        int Offset = 12;

        MD4Digest Digest = new();

        while (true)
        {
            CancellationToken.ThrowIfCancellationRequested();

            int Count = Input.ReadAtLeast(Block, Block.Length, false);

            if (Count is 0)
            {
                break;
            }

            uint Sum = 0;

            uint Weighted = 0;

            for (int Index = 0; Index < Count; ++Index)
            {
                Sum += (uint)Block[Index] + 31;

                Weighted += Sum;
            }

            Span<byte> Checksum = Output.AsSpan(Offset, 20);

            BinaryPrimitives.WriteUInt32BigEndian(Checksum, (Sum & 0xFFFF) | (Weighted << 16));

            Digest.BlockUpdate(Block[..Count]);

            _ = Digest.DoFinal(Checksum[4..]);

            Offset += 20;
        }

        if (Offset != Output.Length)
        {
            throw new IOException("The upload source changed while its signature was being generated.");
        }

        return Output;
    }
}
