
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// A third-party service VRChat mints an SSO token for. Anything else is refused with \&quot;That is not a supported SSO provider.\&quot;
/// </summary>
/// <value>A third-party service VRChat mints an SSO token for. Anything else is refused with \&quot;That is not a supported SSO provider.\&quot;</value>

public enum SsoProvider
{
    /// <summary>
    /// Enum Canny for value: canny
    /// </summary>
    [EnumMember(Value = "canny")]
    Canny = 1,

    /// <summary>
    /// Enum Furality for value: furality
    /// </summary>
    [EnumMember(Value = "furality")]
    Furality = 2
}
