
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines GroupPrivacy
/// </summary>

public enum GroupPrivacy
{
    /// <summary>
    /// Enum Default for value: default
    /// </summary>
    [EnumMember(Value = "default")]
    Default = 1,

    /// <summary>
    /// Enum Private for value: private
    /// </summary>
    [EnumMember(Value = "private")]
    Private = 2
}
