
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines CalendarEventDiscoveryInclusion
/// </summary>

public enum CalendarEventDiscoveryInclusion
{
    /// <summary>
    /// Enum Exclude for value: exclude
    /// </summary>
    [EnumMember(Value = "exclude")]
    Exclude = 1,

    /// <summary>
    /// Enum Include for value: include
    /// </summary>
    [EnumMember(Value = "include")]
    Include = 2,

    /// <summary>
    /// Enum Skip for value: skip
    /// </summary>
    [EnumMember(Value = "skip")]
    Skip = 3
}
