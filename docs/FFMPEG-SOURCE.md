# Native dependencies

The application embeds gzip-compressed Windows x64 payloads. Each decompressed
payload must match its `assets/*.sha256` file. Gzip headers must omit filenames
and comments. Native files are extracted to a hash-verified per-user cache.

FFmpeg 9.0.2 and ffprobe use unmodified official sources with libwebp 1.6.0,
dav1d 1.5.4 and zlib 1.3.2. Source URLs, checksums, toolchain and configuration
are recorded in `assets/native-build.json`. Network access, autodetection, GPL
components and nonfree components are disabled; runtime CPU dispatch is enabled.

The matching `VRCWebP-media-sources-9.0.2.zip` supplied with binary releases contains
the pristine media sources and complete standalone build recipes. Build those
sources and place `ffmpeg.exe.gz` and `ffprobe.exe.gz` under `assets/`.

After a locked NuGet restore, obtain `libSkiaSharp.dll` and `libHarfBuzzSharp.dll`
from the Windows x64 native assets of SkiaSharp.NativeAssets.Win32 3.119.4 and
HarfBuzzSharp.NativeAssets.Win32 8.3.1.3. Check their pinned hashes, gzip them
without filename/comment headers, and place both `.dll.gz` files under `assets/`.

FFmpeg is LGPL-2.1-or-later. libwebp, dav1d, zlib and MinGW-w64 retain their notices
in `assets/dependency-licenses.txt`. The application is GPL-3.0-only. Distribute
the matching media source archive beside each binary and link to it clearly.

Official sources: https://ffmpeg.org/releases/ffmpeg-9.0.2.tar.xz
Release signatures: https://ffmpeg.org/download.html#release-verification
Redistribution guidance: https://ffmpeg.org/legal.html
