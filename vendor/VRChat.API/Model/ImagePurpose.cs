
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Indication of the purpose for uploading images.
/// </summary>
/// <value>Indication of the purpose for uploading images.</value>

public enum ImagePurpose
{
    /// <summary>
    /// Enum Admin for value: admin
    /// </summary>
    [EnumMember(Value = "admin")]
    Admin = 1,

    /// <summary>
    /// Enum Avatargallery for value: avatargallery
    /// </summary>
    [EnumMember(Value = "avatargallery")]
    Avatargallery = 2,

    /// <summary>
    /// Enum Avatarimage for value: avatarimage
    /// </summary>
    [EnumMember(Value = "avatarimage")]
    Avatarimage = 3,

    /// <summary>
    /// Enum Bundle for value: bundle
    /// </summary>
    [EnumMember(Value = "bundle")]
    Bundle = 4,

    /// <summary>
    /// Enum Emoji for value: emoji
    /// </summary>
    [EnumMember(Value = "emoji")]
    Emoji = 5,

    /// <summary>
    /// Enum Emojianimated for value: emojianimated
    /// </summary>
    [EnumMember(Value = "emojianimated")]
    Emojianimated = 6,

    /// <summary>
    /// Enum Gallery for value: gallery
    /// </summary>
    [EnumMember(Value = "gallery")]
    Gallery = 7,

    /// <summary>
    /// Enum Icon for value: icon
    /// </summary>
    [EnumMember(Value = "icon")]
    Icon = 8,

    /// <summary>
    /// Enum Listinggallery for value: listinggallery
    /// </summary>
    [EnumMember(Value = "listinggallery")]
    Listinggallery = 9,

    /// <summary>
    /// Enum Product for value: product
    /// </summary>
    [EnumMember(Value = "product")]
    Product = 10,

    /// <summary>
    /// Enum Sticker for value: sticker
    /// </summary>
    [EnumMember(Value = "sticker")]
    Sticker = 11
}
