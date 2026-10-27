
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines GroupRoleTemplate
/// </summary>

public enum GroupRoleTemplate
{
    /// <summary>
    /// Enum Default for value: default
    /// </summary>
    [EnumMember(Value = "default")]
    Default = 1,

    /// <summary>
    /// Enum ManagedFree for value: managedFree
    /// </summary>
    [EnumMember(Value = "managedFree")]
    ManagedFree = 2,

    /// <summary>
    /// Enum ManagedInvite for value: managedInvite
    /// </summary>
    [EnumMember(Value = "managedInvite")]
    ManagedInvite = 3,

    /// <summary>
    /// Enum ManagedRequest for value: managedRequest
    /// </summary>
    [EnumMember(Value = "managedRequest")]
    ManagedRequest = 4
}
