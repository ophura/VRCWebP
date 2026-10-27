
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// JoinGroupRequest
/// </summary>
[DataContract(Name = "JoinGroupRequest")]
public partial class JoinGroupRequest : IEquatable<JoinGroupRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JoinGroupRequest" /> class.
    /// </summary>
    /// <param name="inviteId">inviteId.</param>
    public JoinGroupRequest(string inviteId = default)
    {
        this.InviteId = inviteId;
    }

    /// <summary>
    /// Gets or Sets InviteId
    /// </summary>
    [DataMember(Name = "inviteId", EmitDefaultValue = false)]
    public string InviteId { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class JoinGroupRequest {\n");
        sb.Append("  InviteId: ").Append(InviteId).Append('\n');
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
        return this.Equals(input as JoinGroupRequest);
    }

    /// <summary>
    /// Returns true if JoinGroupRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of JoinGroupRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(JoinGroupRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.InviteId == input.InviteId ||
                (this.InviteId != null &&
                this.InviteId.Equals(input.InviteId))
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
            if (this.InviteId != null)
            {
                hashCode = (hashCode * 59) + this.InviteId.GetHashCode();
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
