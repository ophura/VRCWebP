
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines the User&#39;s current status, for example \&quot;ask me\&quot;, \&quot;join me\&quot; or \&quot;offline. This status is a combined indicator of their online activity and privacy preference.
/// </summary>
/// <value>Defines the User&#39;s current status, for example \&quot;ask me\&quot;, \&quot;join me\&quot; or \&quot;offline. This status is a combined indicator of their online activity and privacy preference.</value>

public enum UserStatus
{
    /// <summary>
    /// Enum Active for value: active
    /// </summary>
    [EnumMember(Value = "active")]
    Active = 1,

    /// <summary>
    /// Enum AskMe for value: ask me
    /// </summary>
    [EnumMember(Value = "ask me")]
    AskMe = 2,

    /// <summary>
    /// Enum Busy for value: busy
    /// </summary>
    [EnumMember(Value = "busy")]
    Busy = 3,

    /// <summary>
    /// Enum JoinMe for value: join me
    /// </summary>
    [EnumMember(Value = "join me")]
    JoinMe = 4,

    /// <summary>
    /// Enum Offline for value: offline
    /// </summary>
    [EnumMember(Value = "offline")]
    Offline = 5
}
