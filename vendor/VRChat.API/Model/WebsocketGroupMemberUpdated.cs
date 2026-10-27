
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WebsocketGroupMemberUpdated
/// </summary>
[DataContract(Name = "WebsocketGroupMemberUpdated")]
public partial class WebsocketGroupMemberUpdated : IEquatable<WebsocketGroupMemberUpdated>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketGroupMemberUpdated" /> class.
    /// </summary>

    protected WebsocketGroupMemberUpdated() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketGroupMemberUpdated" /> class.
    /// </summary>
    /// <param name="member">member (required).</param>
    public WebsocketGroupMemberUpdated(GroupMemberLimitedUser member = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Member = member;
    }

    /// <summary>
    /// Gets or Sets Member
    /// </summary>
    [DataMember(Name = "member", IsRequired = true, EmitDefaultValue = true)]
    public GroupMemberLimitedUser Member { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class WebsocketGroupMemberUpdated {\n");
        sb.Append("  Member: ").Append(Member).Append('\n');
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
        return this.Equals(input as WebsocketGroupMemberUpdated);
    }

    /// <summary>
    /// Returns true if WebsocketGroupMemberUpdated instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketGroupMemberUpdated to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketGroupMemberUpdated input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Member == input.Member ||
                (this.Member != null &&
                this.Member.Equals(input.Member))
            );
    }

    /// <summary>
    /// Gets the hash code
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hashCode = 41;
            if (this.Member != null)
            {
                hashCode = (hashCode * 59) + this.Member.GetHashCode();
            }
            return hashCode;
        }
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        yield break;
    }
}
