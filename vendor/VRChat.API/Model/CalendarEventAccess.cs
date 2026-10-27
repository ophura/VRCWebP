
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Who the event is for
/// </summary>
/// <value>Who the event is for</value>

public enum CalendarEventAccess
{
    /// <summary>
    /// Enum Group for value: group
    /// </summary>
    [EnumMember(Value = "group")]
    Group = 1,

    /// <summary>
    /// Enum Public for value: public
    /// </summary>
    [EnumMember(Value = "public")]
    Public = 2
}
