
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines ProductListingType
/// </summary>

public enum ProductListingType
{
    /// <summary>
    /// Enum Duration for value: duration
    /// </summary>
    [EnumMember(Value = "duration")]
    Duration = 1,

    /// <summary>
    /// Enum Instant for value: instant
    /// </summary>
    [EnumMember(Value = "instant")]
    Instant = 2,

    /// <summary>
    /// Enum Permanent for value: permanent
    /// </summary>
    [EnumMember(Value = "permanent")]
    Permanent = 3,

    /// <summary>
    /// Enum Subscription for value: subscription
    /// </summary>
    [EnumMember(Value = "subscription")]
    Subscription = 4
}
