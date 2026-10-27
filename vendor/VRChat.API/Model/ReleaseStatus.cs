
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines ReleaseStatus
/// </summary>

public enum ReleaseStatus
{
    /// <summary>
    /// Enum All for value: all
    /// </summary>
    [EnumMember(Value = "all")]
    All = 1,

    /// <summary>
    /// Enum Hidden for value: hidden
    /// </summary>
    [EnumMember(Value = "hidden")]
    Hidden = 2,

    /// <summary>
    /// Enum Private for value: private
    /// </summary>
    [EnumMember(Value = "private")]
    Private = 3,

    /// <summary>
    /// Enum Public for value: public
    /// </summary>
    [EnumMember(Value = "public")]
    Public = 4
}
