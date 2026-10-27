
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines GroupSearchSort
/// </summary>

public enum GroupSearchSort
{
    /// <summary>
    /// Enum JoinedAtasc for value: joinedAt:asc
    /// </summary>
    [EnumMember(Value = "joinedAt:asc")]
    JoinedAtasc = 1,

    /// <summary>
    /// Enum JoinedAtdesc for value: joinedAt:desc
    /// </summary>
    [EnumMember(Value = "joinedAt:desc")]
    JoinedAtdesc = 2
}
