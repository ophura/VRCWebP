
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines InstanceDiscoveryPlatform
/// </summary>

public enum InstanceDiscoveryPlatform
{
    /// <summary>
    /// Enum Android for value: android
    /// </summary>
    [EnumMember(Value = "android")]
    Android = 1,

    /// <summary>
    /// Enum Ios for value: ios
    /// </summary>
    [EnumMember(Value = "ios")]
    Ios = 2,

    /// <summary>
    /// Enum Standalonewindows for value: standalonewindows
    /// </summary>
    [EnumMember(Value = "standalonewindows")]
    Standalonewindows = 3,

    /// <summary>
    /// Enum Web for value: web
    /// </summary>
    [EnumMember(Value = "web")]
    Web = 4
}
