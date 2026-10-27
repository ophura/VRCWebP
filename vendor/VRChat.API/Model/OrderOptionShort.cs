
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines OrderOptionShort
/// </summary>

public enum OrderOptionShort
{
    /// <summary>
    /// Enum Asc for value: asc
    /// </summary>
    [EnumMember(Value = "asc")]
    Asc = 1,

    /// <summary>
    /// Enum Desc for value: desc
    /// </summary>
    [EnumMember(Value = "desc")]
    Desc = 2
}
