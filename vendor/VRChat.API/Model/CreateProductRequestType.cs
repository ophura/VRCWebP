
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines CreateProductRequestType
/// </summary>

public enum CreateProductRequestType
{
    /// <summary>
    /// Enum Avatar for value: avatar
    /// </summary>
    [EnumMember(Value = "avatar")]
    Avatar = 1,

    /// <summary>
    /// Enum Credit for value: credit
    /// </summary>
    [EnumMember(Value = "credit")]
    Credit = 2,

    /// <summary>
    /// Enum Inventory for value: inventory
    /// </summary>
    [EnumMember(Value = "inventory")]
    Inventory = 3,

    /// <summary>
    /// Enum Listing for value: listing
    /// </summary>
    [EnumMember(Value = "listing")]
    Listing = 4,

    /// <summary>
    /// Enum TestBirdy for value: test_birdy
    /// </summary>
    [EnumMember(Value = "test_birdy")]
    TestBirdy = 5,

    /// <summary>
    /// Enum Udon for value: udon
    /// </summary>
    [EnumMember(Value = "udon")]
    Udon = 6
}
