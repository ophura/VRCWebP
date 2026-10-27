
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Instance region
/// </summary>
/// <value>Instance region</value>

public enum InstanceRegion
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
    Use = 4,

    /// <summary>
    /// Enum Unknown for value: unknown
    /// </summary>
    [EnumMember(Value = "unknown")]
    Unknown = 5
}
