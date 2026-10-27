
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines OrderOption
/// </summary>

public enum OrderOption
{
    /// <summary>
    /// Enum Ascending for value: ascending
    /// </summary>
    [EnumMember(Value = "ascending")]
    Ascending = 1,

    /// <summary>
    /// Enum Descending for value: descending
    /// </summary>
    [EnumMember(Value = "descending")]
    Descending = 2
}
