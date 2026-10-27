using System.Buffers.Binary;

namespace VRCWebP;

internal sealed partial class MediaService
{
    /// <summary>Detects actual bytes, including PNG-labelled VRChat originals; filenames are not codec evidence.</summary>
    private static bool IsWebPInput(string Path)
    {
        using FileStream Input = File.OpenRead(Path);
        Span<byte> Header = stackalloc byte[12];

        return Input.ReadAtLeast(Header, Header.Length, false) is 12
            && Header[..4].SequenceEqual("RIFF"u8) && Header[8..].SequenceEqual("WEBP"u8);
    }

    /// <summary>Reads actual animation timing without decoding frames or trusting ffprobe's missing WebP duration.</summary>
    private static MediaInfo? ReadWebPTiming(string Path, CancellationToken Token)
    {
        using FileStream Input = File.OpenRead(Path);

        Span<byte> Header = stackalloc byte[16];

        Input.ReadExactly(Header[..12]);

        if (!Header[..4].SequenceEqual("RIFF"u8) || !Header[8..12].SequenceEqual("WEBP"u8)
            || BinaryPrimitives.ReadUInt32LittleEndian(Header[4..]) + 8L != Input.Length)
        {
            throw new InvalidDataException("The WebP container is incomplete.");
        }

        int Width = 0;

        int Height = 0;

        int Frames = 0;

        long Milliseconds = 0;

        while (Input.Position < Input.Length)
        {
            Token.ThrowIfCancellationRequested();

            Input.ReadExactly(Header[..8]);

            uint Length = BinaryPrimitives.ReadUInt32LittleEndian(Header[4..]);

            long Next = Input.Position + Length + (Length & 1);

            if (Next > Input.Length)
            {
                throw new InvalidDataException("The WebP chunk is truncated.");
            }

            if (Header[..4].SequenceEqual("VP8X"u8) && Length is 10)
            {
                Input.ReadExactly(Header[..10]);

                Width = ReadUInt24(Header[4..]) + 1;

                Height = ReadUInt24(Header[7..]) + 1;
            }
            else if (Header[..4].SequenceEqual("ANMF"u8) && Length >= 16)
            {
                Input.ReadExactly(Header);

                // The WebP specification leaves zero-duration playback to the implementation.
                // This application uses a 100 ms fallback for zero; other viewers may choose differently.
                int Duration = ReadUInt24(Header[12..]);

                Milliseconds = checked(Milliseconds + (Duration is 0 ? 100 : Duration));

                ++Frames;
            }

            Input.Position = Next;
        }

        return Frames > 0 && Width > 0 && Height > 0
            ? new MediaInfo(Width, Height, Milliseconds / 1000d, Frames * 1000d / Milliseconds) : null;
    }
}
