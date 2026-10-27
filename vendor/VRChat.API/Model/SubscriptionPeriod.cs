
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines SubscriptionPeriod
/// </summary>

public enum SubscriptionPeriod
{
    /// <summary>
    /// Enum Day for value: day
    /// </summary>
    [EnumMember(Value = "day")]
    Day = 1,

    /// <summary>
    /// Enum Hour for value: hour
    /// </summary>
    [EnumMember(Value = "hour")]
    Hour = 2,

    /// <summary>
    /// Enum Month for value: month
    /// </summary>
    [EnumMember(Value = "month")]
    Month = 3,

    /// <summary>
    /// Enum Week for value: week
    /// </summary>
    [EnumMember(Value = "week")]
    Week = 4,

    /// <summary>
    /// Enum Year for value: year
    /// </summary>
    [EnumMember(Value = "year")]
    Year = 5
}
