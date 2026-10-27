
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// How a recurring event stops being scheduled
/// </summary>
/// <value>How a recurring event stops being scheduled</value>

public enum CalendarEventRecurrenceEndType
{
    /// <summary>
    /// Enum AfterDate for value: afterDate
    /// </summary>
    [EnumMember(Value = "afterDate")]
    AfterDate = 1,

    /// <summary>
    /// Enum AfterOccurrences for value: afterOccurrences
    /// </summary>
    [EnumMember(Value = "afterOccurrences")]
    AfterOccurrences = 2
}
