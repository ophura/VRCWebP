
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines LicenseType
/// </summary>

public enum LicenseType
{
    /// <summary>
    /// Enum Avatar for value: avatar
    /// </summary>
    [EnumMember(Value = "avatar")]
    Avatar = 1,

    /// <summary>
    /// Enum LicenseGroup for value: licenseGroup
    /// </summary>
    [EnumMember(Value = "licenseGroup")]
    LicenseGroup = 2,

    /// <summary>
    /// Enum Permission for value: permission
    /// </summary>
    [EnumMember(Value = "permission")]
    Permission = 3,

    /// <summary>
    /// Enum Product for value: product
    /// </summary>
    [EnumMember(Value = "product")]
    Product = 4
}
