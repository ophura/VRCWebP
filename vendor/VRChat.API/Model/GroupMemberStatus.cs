
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines GroupMemberStatus
/// </summary>

public enum GroupMemberStatus
{
    /// <summary>
    /// Enum Banned for value: banned
    /// </summary>
    [EnumMember(Value = "banned")]
    Banned = 1,

    /// <summary>
    /// Enum Inactive for value: inactive
    /// </summary>
    [EnumMember(Value = "inactive")]
    Inactive = 2,

    /// <summary>
    /// Enum Invited for value: invited
    /// </summary>
    [EnumMember(Value = "invited")]
    Invited = 3,

    /// <summary>
    /// Enum Member for value: member
    /// </summary>
    [EnumMember(Value = "member")]
    Member = 4,

    /// <summary>
    /// Enum Requested for value: requested
    /// </summary>
    [EnumMember(Value = "requested")]
    Requested = 5,

    /// <summary>
    /// Enum Userblocked for value: userblocked
    /// </summary>
    [EnumMember(Value = "userblocked")]
    Userblocked = 6
}
