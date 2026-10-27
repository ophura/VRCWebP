
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines GroupUserVisibility
/// </summary>

public enum GroupUserVisibility
{
    /// <summary>
    /// Enum Friends for value: friends
    /// </summary>
    [EnumMember(Value = "friends")]
    Friends = 1,

    /// <summary>
    /// Enum Hidden for value: hidden
    /// </summary>
    [EnumMember(Value = "hidden")]
    Hidden = 2,

    /// <summary>
    /// Enum Visible for value: visible
    /// </summary>
    [EnumMember(Value = "visible")]
    Visible = 3
}
