
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines GroupJoinRequestAction
/// </summary>

public enum GroupJoinRequestAction
{
    /// <summary>
    /// Enum Accept for value: accept
    /// </summary>
    [EnumMember(Value = "accept")]
    Accept = 1,

    /// <summary>
    /// Enum Reject for value: reject
    /// </summary>
    [EnumMember(Value = "reject")]
    Reject = 2
}
