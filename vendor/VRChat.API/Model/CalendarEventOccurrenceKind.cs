
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// How an event occurs or recurs. \&quot;single\&quot; is a standalone event, \&quot;series\&quot; is a parent specification of a recurring series, and \&quot;occurrence\&quot; is an individual event in a series.
/// </summary>
/// <value>How an event occurs or recurs. \&quot;single\&quot; is a standalone event, \&quot;series\&quot; is a parent specification of a recurring series, and \&quot;occurrence\&quot; is an individual event in a series.</value>

public enum CalendarEventOccurrenceKind
{
    /// <summary>
    /// Enum Occurrence for value: occurrence
    /// </summary>
    [EnumMember(Value = "occurrence")]
    Occurrence = 1,

    /// <summary>
    /// Enum Series for value: series
    /// </summary>
    [EnumMember(Value = "series")]
    Series = 2,

    /// <summary>
    /// Enum Single for value: single
    /// </summary>
    [EnumMember(Value = "single")]
    Single = 3
}
