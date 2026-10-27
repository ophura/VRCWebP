
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines BannerType
/// </summary>

public enum BannerType
{
    /// <summary>
    /// Enum AvatarBanner for value: avatarBanner
    /// </summary>
    [EnumMember(Value = "avatarBanner")]
    AvatarBanner = 1,

    /// <summary>
    /// Enum Color for value: color
    /// </summary>
    [EnumMember(Value = "color")]
    Color = 2,

    /// <summary>
    /// Enum CustomImage for value: customImage
    /// </summary>
    [EnumMember(Value = "customImage")]
    CustomImage = 3
}
