
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines TransactionStatus
/// </summary>

public enum TransactionStatus
{
    /// <summary>
    /// Enum Active for value: active
    /// </summary>
    [EnumMember(Value = "active")]
    Active = 1,

    /// <summary>
    /// Enum Chargeback for value: chargeback
    /// </summary>
    [EnumMember(Value = "chargeback")]
    Chargeback = 2,

    /// <summary>
    /// Enum Expired for value: expired
    /// </summary>
    [EnumMember(Value = "expired")]
    Expired = 3,

    /// <summary>
    /// Enum Failed for value: failed
    /// </summary>
    [EnumMember(Value = "failed")]
    Failed = 4
}
