
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines WebsocketContentRefreshActionType
/// </summary>

public enum WebsocketContentRefreshActionType
{
    /// <summary>
    /// Enum Add for value: add
    /// </summary>
    [EnumMember(Value = "add")]
    Add = 1,

    /// <summary>
    /// Enum Created for value: created
    /// </summary>
    [EnumMember(Value = "created")]
    Created = 2,

    /// <summary>
    /// Enum Delete for value: delete
    /// </summary>
    [EnumMember(Value = "delete")]
    Delete = 3,

    /// <summary>
    /// Enum Deleted for value: deleted
    /// </summary>
    [EnumMember(Value = "deleted")]
    Deleted = 4
}
