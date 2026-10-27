
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines FileStatus
/// </summary>

public enum FileStatus
{
    /// <summary>
    /// Enum Complete for value: complete
    /// </summary>
    [EnumMember(Value = "complete")]
    Complete = 1,

    /// <summary>
    /// Enum None for value: none
    /// </summary>
    [EnumMember(Value = "none")]
    None = 2,

    /// <summary>
    /// Enum Queued for value: queued
    /// </summary>
    [EnumMember(Value = "queued")]
    Queued = 3,

    /// <summary>
    /// Enum Waiting for value: waiting
    /// </summary>
    [EnumMember(Value = "waiting")]
    Waiting = 4
}
