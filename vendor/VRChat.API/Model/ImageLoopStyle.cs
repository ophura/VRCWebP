
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Animation looping style for images.
/// </summary>
/// <value>Animation looping style for images.</value>

public enum ImageLoopStyle
{
    /// <summary>
    /// Enum Linear for value: linear
    /// </summary>
    [EnumMember(Value = "linear")]
    Linear = 1,

    /// <summary>
    /// Enum Pingpong for value: pingpong
    /// </summary>
    [EnumMember(Value = "pingpong")]
    Pingpong = 2
}
