
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines StoreView
/// </summary>

public enum StoreView
{
    /// <summary>
    /// Enum All for value: all
    /// </summary>
    [EnumMember(Value = "all")]
    All = 1,

    /// <summary>
    /// Enum Draft for value: draft
    /// </summary>
    [EnumMember(Value = "draft")]
    Draft = 2,

    /// <summary>
    /// Enum Preview for value: preview
    /// </summary>
    [EnumMember(Value = "preview")]
    Preview = 3,

    /// <summary>
    /// Enum Public for value: public
    /// </summary>
    [EnumMember(Value = "public")]
    Public = 4,

    /// <summary>
    /// Enum PublicPreview for value: publicPreview
    /// </summary>
    [EnumMember(Value = "publicPreview")]
    PublicPreview = 5
}
