
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines GroupJoinState
/// </summary>

public enum GroupJoinState
{
    /// <summary>
    /// Enum Closed for value: closed
    /// </summary>
    [EnumMember(Value = "closed")]
    Closed = 1,

    /// <summary>
    /// Enum Invite for value: invite
    /// </summary>
    [EnumMember(Value = "invite")]
    Invite = 2,

    /// <summary>
    /// Enum Open for value: open
    /// </summary>
    [EnumMember(Value = "open")]
    Open = 3,

    /// <summary>
    /// Enum Request for value: request
    /// </summary>
    [EnumMember(Value = "request")]
    Request = 4
}
