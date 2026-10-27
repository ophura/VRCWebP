
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines LicenseAction
/// </summary>

public enum LicenseAction
{
    /// <summary>
    /// Enum Have for value: have
    /// </summary>
    [EnumMember(Value = "have")]
    Have = 1,

    /// <summary>
    /// Enum Wear for value: wear
    /// </summary>
    [EnumMember(Value = "wear")]
    Wear = 2
}
