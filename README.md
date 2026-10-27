# VRCWebP

Windows application for creating animated WebP images and managing VRChat image uploads.

Early-stage software. Please report reproducible problems through Issues; omit credentials and personal data.

Safe support baseline: Windows 11 24H2 x64. Earlier versions may work; verify compatibility yourself.

Build with the pinned .NET SDK and Visual Studio C++ NativeAOT prerequisites.
Prepare the [native dependencies](docs/FFMPEG-SOURCE.md), then run:

```powershell
dotnet publish src/VRCWebP/VRCWebP.csproj -c Release -r win-x64 --self-contained true
```

Original source: [GPL-3.0-only](LICENSE). Dependencies retain their own licenses.
