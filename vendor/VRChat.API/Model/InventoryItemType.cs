
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines InventoryItemType
/// </summary>

public enum InventoryItemType
{
    /// <summary>
    /// Enum Bundle for value: bundle
    /// </summary>
    [EnumMember(Value = "bundle")]
    Bundle = 1,

    /// <summary>
    /// Enum Droneskin for value: droneskin
    /// </summary>
    [EnumMember(Value = "droneskin")]
    Droneskin = 2,

    /// <summary>
    /// Enum Emoji for value: emoji
    /// </summary>
    [EnumMember(Value = "emoji")]
    Emoji = 3,

    /// <summary>
    /// Enum IconFrame for value: iconFrame
    /// </summary>
    [EnumMember(Value = "iconFrame")]
    IconFrame = 4,

    /// <summary>
    /// Enum NameplateEffect for value: nameplateEffect
    /// </summary>
    [EnumMember(Value = "nameplateEffect")]
    NameplateEffect = 5,

    /// <summary>
    /// Enum Portalskin for value: portalskin
    /// </summary>
    [EnumMember(Value = "portalskin")]
    Portalskin = 6,

    /// <summary>
    /// Enum ProfileEffect for value: profileEffect
    /// </summary>
    [EnumMember(Value = "profileEffect")]
    ProfileEffect = 7,

    /// <summary>
    /// Enum Prop for value: prop
    /// </summary>
    [EnumMember(Value = "prop")]
    Prop = 8,

    /// <summary>
    /// Enum Sticker for value: sticker
    /// </summary>
    [EnumMember(Value = "sticker")]
    Sticker = 9,

    /// <summary>
    /// Enum Warpeffect for value: warpeffect
    /// </summary>
    [EnumMember(Value = "warpeffect")]
    Warpeffect = 10
}
