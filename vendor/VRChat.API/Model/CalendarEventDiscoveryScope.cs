
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines CalendarEventDiscoveryScope
/// </summary>

public enum CalendarEventDiscoveryScope
{
    /// <summary>
    /// Enum All for value: all
    /// </summary>
    [EnumMember(Value = "all")]
    All = 1,

    /// <summary>
    /// Enum Live for value: live
    /// </summary>
    [EnumMember(Value = "live")]
    Live = 2,

    /// <summary>
    /// Enum Upcoming for value: upcoming
    /// </summary>
    [EnumMember(Value = "upcoming")]
    Upcoming = 3
}
