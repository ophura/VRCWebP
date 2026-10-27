
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines StoreType
/// </summary>

public enum StoreType
{
    /// <summary>
    /// Enum Group for value: group
    /// </summary>
    [EnumMember(Value = "group")]
    Group = 1,

    /// <summary>
    /// Enum House for value: house
    /// </summary>
    [EnumMember(Value = "house")]
    House = 2,

    /// <summary>
    /// Enum World for value: world
    /// </summary>
    [EnumMember(Value = "world")]
    World = 3
}
