
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// API/Photon region.
/// </summary>
/// <value>API/Photon region.</value>

public enum Region
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
    /// Enum Unknown for value: unknown
    /// </summary>
    [EnumMember(Value = "unknown")]
    Unknown = 3,

    /// <summary>
    /// Enum Us for value: us
    /// </summary>
    [EnumMember(Value = "us")]
    Us = 4,

    /// <summary>
    /// Enum Use for value: use
    /// </summary>
    [EnumMember(Value = "use")]
    Use = 5,

    /// <summary>
    /// Enum Usw for value: usw
    /// </summary>
    [EnumMember(Value = "usw")]
    Usw = 6,

    /// <summary>
    /// Enum Usx for value: usx
    /// </summary>
    [EnumMember(Value = "usx")]
    Usx = 7
}
