
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines InviteMessageType
/// </summary>

public enum InviteMessageType
{
    /// <summary>
    /// Enum Message for value: message
    /// </summary>
    [EnumMember(Value = "message")]
    Message = 1,

    /// <summary>
    /// Enum Request for value: request
    /// </summary>
    [EnumMember(Value = "request")]
    Request = 2,

    /// <summary>
    /// Enum RequestResponse for value: requestResponse
    /// </summary>
    [EnumMember(Value = "requestResponse")]
    RequestResponse = 3,

    /// <summary>
    /// Enum Response for value: response
    /// </summary>
    [EnumMember(Value = "response")]
    Response = 4
}
