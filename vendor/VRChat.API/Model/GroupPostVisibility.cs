
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines GroupPostVisibility
/// </summary>

public enum GroupPostVisibility
{
    /// <summary>
    /// Enum Group for value: group
    /// </summary>
    [EnumMember(Value = "group")]
    Group = 1,

    /// <summary>
    /// Enum Public for value: public
    /// </summary>
    [EnumMember(Value = "public")]
    Public = 2
}
