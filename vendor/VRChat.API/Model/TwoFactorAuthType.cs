
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines TwoFactorAuthType
/// </summary>

public enum TwoFactorAuthType
{
    /// <summary>
    /// Enum EmailOtp for value: emailOtp
    /// </summary>
    [EnumMember(Value = "emailOtp")]
    EmailOtp = 1,

    /// <summary>
    /// Enum Otp for value: otp
    /// </summary>
    [EnumMember(Value = "otp")]
    Otp = 2,

    /// <summary>
    /// Enum Totp for value: totp
    /// </summary>
    [EnumMember(Value = "totp")]
    Totp = 3
}
