
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines MIMEType
/// </summary>

public enum MIMEType
{
    /// <summary>
    /// Enum ApplicationGzip for value: application/gzip
    /// </summary>
    [EnumMember(Value = "application/gzip")]
    ApplicationGzip = 1,

    /// <summary>
    /// Enum ApplicationOctetStream for value: application/octet-stream
    /// </summary>
    [EnumMember(Value = "application/octet-stream")]
    ApplicationOctetStream = 2,

    /// <summary>
    /// Enum ApplicationXAvatar for value: application/x-avatar
    /// </summary>
    [EnumMember(Value = "application/x-avatar")]
    ApplicationXAvatar = 3,

    /// <summary>
    /// Enum ApplicationXProp for value: application/x-prop
    /// </summary>
    [EnumMember(Value = "application/x-prop")]
    ApplicationXProp = 4,

    /// <summary>
    /// Enum ApplicationXRsyncDelta for value: application/x-rsync-delta
    /// </summary>
    [EnumMember(Value = "application/x-rsync-delta")]
    ApplicationXRsyncDelta = 5,

    /// <summary>
    /// Enum ApplicationXRsyncSignature for value: application/x-rsync-signature
    /// </summary>
    [EnumMember(Value = "application/x-rsync-signature")]
    ApplicationXRsyncSignature = 6,

    /// <summary>
    /// Enum ApplicationXWorld for value: application/x-world
    /// </summary>
    [EnumMember(Value = "application/x-world")]
    ApplicationXWorld = 7,

    /// <summary>
    /// Enum ImageBmp for value: image/bmp
    /// </summary>
    [EnumMember(Value = "image/bmp")]
    ImageBmp = 8,

    /// <summary>
    /// Enum ImageGif for value: image/gif
    /// </summary>
    [EnumMember(Value = "image/gif")]
    ImageGif = 9,

    /// <summary>
    /// Enum ImageJpeg for value: image/jpeg
    /// </summary>
    [EnumMember(Value = "image/jpeg")]
    ImageJpeg = 10,

    /// <summary>
    /// Enum ImageJpg for value: image/jpg
    /// </summary>
    [EnumMember(Value = "image/jpg")]
    ImageJpg = 11,

    /// <summary>
    /// Enum ImagePng for value: image/png
    /// </summary>
    [EnumMember(Value = "image/png")]
    ImagePng = 12,

    /// <summary>
    /// Enum ImageSvgxml for value: image/svg+xml
    /// </summary>
    [EnumMember(Value = "image/svg+xml")]
    ImageSvgxml = 13,

    /// <summary>
    /// Enum ImageTiff for value: image/tiff
    /// </summary>
    [EnumMember(Value = "image/tiff")]
    ImageTiff = 14,

    /// <summary>
    /// Enum ImageWebp for value: image/webp
    /// </summary>
    [EnumMember(Value = "image/webp")]
    ImageWebp = 15
}
