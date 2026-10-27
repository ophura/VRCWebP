
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Flags for content filtering
/// </summary>
/// <value>Flags for content filtering</value>

public enum ContentFilter
{
    /// <summary>
    /// Enum ContentAdult for value: content_adult
    /// </summary>
    [EnumMember(Value = "content_adult")]
    ContentAdult = 1,

    /// <summary>
    /// Enum ContentGore for value: content_gore
    /// </summary>
    [EnumMember(Value = "content_gore")]
    ContentGore = 2,

    /// <summary>
    /// Enum ContentHorror for value: content_horror
    /// </summary>
    [EnumMember(Value = "content_horror")]
    ContentHorror = 3,

    /// <summary>
    /// Enum ContentSex for value: content_sex
    /// </summary>
    [EnumMember(Value = "content_sex")]
    ContentSex = 4,

    /// <summary>
    /// Enum ContentViolence for value: content_violence
    /// </summary>
    [EnumMember(Value = "content_violence")]
    ContentViolence = 5
}
