
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// The day of the week, used for recurring events.
/// </summary>
/// <value>The day of the week, used for recurring events.</value>

public enum CalendarDayOfWeek
{
    /// <summary>
    /// Enum FR for value: FR
    /// </summary>
    [EnumMember(Value = "FR")]
    FR = 1,

    /// <summary>
    /// Enum MO for value: MO
    /// </summary>
    [EnumMember(Value = "MO")]
    MO = 2,

    /// <summary>
    /// Enum SA for value: SA
    /// </summary>
    [EnumMember(Value = "SA")]
    SA = 3,

    /// <summary>
    /// Enum SU for value: SU
    /// </summary>
    [EnumMember(Value = "SU")]
    SU = 4,

    /// <summary>
    /// Enum TH for value: TH
    /// </summary>
    [EnumMember(Value = "TH")]
    TH = 5,

    /// <summary>
    /// Enum TU for value: TU
    /// </summary>
    [EnumMember(Value = "TU")]
    TU = 6,

    /// <summary>
    /// Enum WE for value: WE
    /// </summary>
    [EnumMember(Value = "WE")]
    WE = 7
}
