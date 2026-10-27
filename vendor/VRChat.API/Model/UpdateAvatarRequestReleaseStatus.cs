
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines UpdateAvatarRequestReleaseStatus
/// </summary>

public enum UpdateAvatarRequestReleaseStatus
{
    /// <summary>
    /// Enum Hidden for value: hidden
    /// </summary>
    [EnumMember(Value = "hidden")]
    Hidden = 1,

    /// <summary>
    /// Enum Private for value: private
    /// </summary>
    [EnumMember(Value = "private")]
    Private = 2,

    /// <summary>
    /// Enum Public for value: public
    /// </summary>
    [EnumMember(Value = "public")]
    Public = 3
}
