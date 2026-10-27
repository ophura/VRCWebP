
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Mask shape for images.
/// </summary>
/// <value>Mask shape for images.</value>

public enum ImageMask
{
    /// <summary>
    /// Enum Circle for value: circle
    /// </summary>
    [EnumMember(Value = "circle")]
    Circle = 1,

    /// <summary>
    /// Enum Flower for value: flower
    /// </summary>
    [EnumMember(Value = "flower")]
    Flower = 2,

    /// <summary>
    /// Enum Heart for value: heart
    /// </summary>
    [EnumMember(Value = "heart")]
    Heart = 3,

    /// <summary>
    /// Enum Pow for value: pow
    /// </summary>
    [EnumMember(Value = "pow")]
    Pow = 4,

    /// <summary>
    /// Enum Square for value: square
    /// </summary>
    [EnumMember(Value = "square")]
    Square = 5,

    /// <summary>
    /// Enum Star for value: star
    /// </summary>
    [EnumMember(Value = "star")]
    Star = 6
}
