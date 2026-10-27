
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines FavoriteType
/// </summary>

public enum FavoriteType
{
    /// <summary>
    /// Enum Avatar for value: avatar
    /// </summary>
    [EnumMember(Value = "avatar")]
    Avatar = 1,

    /// <summary>
    /// Enum Friend for value: friend
    /// </summary>
    [EnumMember(Value = "friend")]
    Friend = 2,

    /// <summary>
    /// Enum VrcPlusWorld for value: vrcPlusWorld
    /// </summary>
    [EnumMember(Value = "vrcPlusWorld")]
    VrcPlusWorld = 3,

    /// <summary>
    /// Enum World for value: world
    /// </summary>
    [EnumMember(Value = "world")]
    World = 4
}
