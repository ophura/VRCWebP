
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines InventoryEquipSlot
/// </summary>

public enum InventoryEquipSlot
{
    /// <summary>
    /// Enum Empty for value:
    /// </summary>
    [EnumMember(Value = "")]
    Empty = 1,

    /// <summary>
    /// Enum Drone for value: drone
    /// </summary>
    [EnumMember(Value = "drone")]
    Drone = 2,

    /// <summary>
    /// Enum IconFrame for value: iconFrame
    /// </summary>
    [EnumMember(Value = "iconFrame")]
    IconFrame = 3,

    /// <summary>
    /// Enum NameplateEffect for value: nameplateEffect
    /// </summary>
    [EnumMember(Value = "nameplateEffect")]
    NameplateEffect = 4,

    /// <summary>
    /// Enum Portal for value: portal
    /// </summary>
    [EnumMember(Value = "portal")]
    Portal = 5,

    /// <summary>
    /// Enum ProfileEffect for value: profileEffect
    /// </summary>
    [EnumMember(Value = "profileEffect")]
    ProfileEffect = 6,

    /// <summary>
    /// Enum Warp for value: warp
    /// </summary>
    [EnumMember(Value = "warp")]
    Warp = 7
}
