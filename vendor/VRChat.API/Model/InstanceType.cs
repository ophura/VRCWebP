
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines InstanceType
/// </summary>

public enum InstanceType
{
    /// <summary>
    /// Enum Friends for value: friends
    /// </summary>
    [EnumMember(Value = "friends")]
    Friends = 1,

    /// <summary>
    /// Enum Group for value: group
    /// </summary>
    [EnumMember(Value = "group")]
    Group = 2,

    /// <summary>
    /// Enum Hidden for value: hidden
    /// </summary>
    [EnumMember(Value = "hidden")]
    Hidden = 3,

    /// <summary>
    /// Enum Private for value: private
    /// </summary>
    [EnumMember(Value = "private")]
    Private = 4,

    /// <summary>
    /// Enum Public for value: public
    /// </summary>
    [EnumMember(Value = "public")]
    Public = 5
}
