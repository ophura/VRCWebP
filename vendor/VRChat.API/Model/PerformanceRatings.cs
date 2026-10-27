
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Avatar Performance ratings.
/// </summary>
/// <value>Avatar Performance ratings.</value>

public enum PerformanceRatings
{
    /// <summary>
    /// Enum Excellent for value: Excellent
    /// </summary>
    [EnumMember(Value = "Excellent")]
    Excellent = 1,

    /// <summary>
    /// Enum Good for value: Good
    /// </summary>
    [EnumMember(Value = "Good")]
    Good = 2,

    /// <summary>
    /// Enum Medium for value: Medium
    /// </summary>
    [EnumMember(Value = "Medium")]
    Medium = 3,

    /// <summary>
    /// Enum None for value: None
    /// </summary>
    [EnumMember(Value = "None")]
    None = 4,

    /// <summary>
    /// Enum Poor for value: Poor
    /// </summary>
    [EnumMember(Value = "Poor")]
    Poor = 5,

    /// <summary>
    /// Enum VeryPoor for value: VeryPoor
    /// </summary>
    [EnumMember(Value = "VeryPoor")]
    VeryPoor = 6
}
