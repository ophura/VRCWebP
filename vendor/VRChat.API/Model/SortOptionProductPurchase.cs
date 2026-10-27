
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines SortOptionProductPurchase
/// </summary>

public enum SortOptionProductPurchase
{
    /// <summary>
    /// Enum ExpireDate for value: expireDate
    /// </summary>
    [EnumMember(Value = "expireDate")]
    ExpireDate = 1,

    /// <summary>
    /// Enum Name for value: name
    /// </summary>
    [EnumMember(Value = "name")]
    Name = 2,

    /// <summary>
    /// Enum PurchaseDate for value: purchaseDate
    /// </summary>
    [EnumMember(Value = "purchaseDate")]
    PurchaseDate = 3
}
