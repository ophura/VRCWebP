
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// &#x60;verified&#x60; is obsolete.  User who have verified and are 18+ can switch to &#x60;plus18&#x60; status.
/// </summary>
/// <value>&#x60;verified&#x60; is obsolete.  User who have verified and are 18+ can switch to &#x60;plus18&#x60; status. </value>

public enum AgeVerificationStatus
{
    /// <summary>
    /// Enum plus18 for value: 18+
    /// </summary>
    [EnumMember(Value = "18+")]
    plus18 = 1,

    /// <summary>
    /// Enum hidden for value: hidden
    /// </summary>
    [EnumMember(Value = "hidden")]
    hidden = 2,

    /// <summary>
    /// Enum verified for value: verified
    /// </summary>
    [EnumMember(Value = "verified")]
    verified = 3
}
