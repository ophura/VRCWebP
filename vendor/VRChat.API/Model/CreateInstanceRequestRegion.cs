
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines CreateInstanceRequestRegion
/// </summary>

public enum CreateInstanceRequestRegion
{
    /// <summary>
    /// Enum Eu for value: eu
    /// </summary>
    [EnumMember(Value = "eu")]
    Eu = 1,

    /// <summary>
    /// Enum Jp for value: jp
    /// </summary>
    [EnumMember(Value = "jp")]
    Jp = 2,

    /// <summary>
    /// Enum Us for value: us
    /// </summary>
    [EnumMember(Value = "us")]
    Us = 3,

    /// <summary>
    /// Enum Use for value: use
    /// </summary>
    [EnumMember(Value = "use")]
    Use = 4
}
