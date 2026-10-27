
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines PlayerModerationType
/// </summary>

public enum PlayerModerationType
{
    /// <summary>
    /// Enum Block for value: block
    /// </summary>
    [EnumMember(Value = "block")]
    Block = 1,

    /// <summary>
    /// Enum HideAvatar for value: hideAvatar
    /// </summary>
    [EnumMember(Value = "hideAvatar")]
    HideAvatar = 2,

    /// <summary>
    /// Enum InteractOff for value: interactOff
    /// </summary>
    [EnumMember(Value = "interactOff")]
    InteractOff = 3,

    /// <summary>
    /// Enum InteractOn for value: interactOn
    /// </summary>
    [EnumMember(Value = "interactOn")]
    InteractOn = 4,

    /// <summary>
    /// Enum Mute for value: mute
    /// </summary>
    [EnumMember(Value = "mute")]
    Mute = 5,

    /// <summary>
    /// Enum MuteChat for value: muteChat
    /// </summary>
    [EnumMember(Value = "muteChat")]
    MuteChat = 6,

    /// <summary>
    /// Enum ShowAvatar for value: showAvatar
    /// </summary>
    [EnumMember(Value = "showAvatar")]
    ShowAvatar = 7,

    /// <summary>
    /// Enum Unmute for value: unmute
    /// </summary>
    [EnumMember(Value = "unmute")]
    Unmute = 8,

    /// <summary>
    /// Enum UnmuteChat for value: unmuteChat
    /// </summary>
    [EnumMember(Value = "unmuteChat")]
    UnmuteChat = 9
}
