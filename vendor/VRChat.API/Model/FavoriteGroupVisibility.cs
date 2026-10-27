
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines FavoriteGroupVisibility
/// </summary>

public enum FavoriteGroupVisibility
{
    /// <summary>
    /// Enum Friends for value: friends
    /// </summary>
    [EnumMember(Value = "friends")]
    Friends = 1,

    /// <summary>
    /// Enum Private for value: private
    /// </summary>
    [EnumMember(Value = "private")]
    Private = 2,

    /// <summary>
    /// Enum Public for value: public
    /// </summary>
    [EnumMember(Value = "public")]
    Public = 3
}
