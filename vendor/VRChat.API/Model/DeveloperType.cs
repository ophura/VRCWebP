
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// \&quot;none\&quot; User is a normal user \&quot;trusted\&quot; Unknown \&quot;internal\&quot; Is a VRChat Developer \&quot;moderator\&quot; Is a VRChat Moderator  Staff can hide their developerType at will.
/// </summary>
/// <value>\&quot;none\&quot; User is a normal user \&quot;trusted\&quot; Unknown \&quot;internal\&quot; Is a VRChat Developer \&quot;moderator\&quot; Is a VRChat Moderator  Staff can hide their developerType at will.</value>

public enum DeveloperType
{
    /// <summary>
    /// Enum Internal for value: internal
    /// </summary>
    [EnumMember(Value = "internal")]
    Internal = 1,

    /// <summary>
    /// Enum Moderator for value: moderator
    /// </summary>
    [EnumMember(Value = "moderator")]
    Moderator = 2,

    /// <summary>
    /// Enum None for value: none
    /// </summary>
    [EnumMember(Value = "none")]
    None = 3,

    /// <summary>
    /// Enum Trusted for value: trusted
    /// </summary>
    [EnumMember(Value = "trusted")]
    Trusted = 4
}
