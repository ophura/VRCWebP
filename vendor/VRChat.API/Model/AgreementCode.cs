
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// The type of agreement.
/// </summary>
/// <value>The type of agreement.</value>

public enum AgreementCode
{
    /// <summary>
    /// Enum ContentCopyrightOwned for value: content.copyright.owned
    /// </summary>
    [EnumMember(Value = "content.copyright.owned")]
    ContentCopyrightOwned = 1
}
