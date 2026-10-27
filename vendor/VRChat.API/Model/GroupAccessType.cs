
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Group access type when the instance type is \&quot;group\&quot;
/// </summary>
/// <value>Group access type when the instance type is \&quot;group\&quot;</value>

public enum GroupAccessType
{
    /// <summary>
    /// Enum Members for value: members
    /// </summary>
    [EnumMember(Value = "members")]
    Members = 1,

    /// <summary>
    /// Enum Plus for value: plus
    /// </summary>
    [EnumMember(Value = "plus")]
    Plus = 2,

    /// <summary>
    /// Enum Public for value: public
    /// </summary>
    [EnumMember(Value = "public")]
    Public = 3
}
