
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// DeclineGroupInviteRequest
/// </summary>
[DataContract(Name = "DeclineGroupInviteRequest")]
public partial class DeclineGroupInviteRequest : IEquatable<DeclineGroupInviteRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclineGroupInviteRequest" /> class.
    /// </summary>
    /// <param name="block">block (default to false).</param>
    public DeclineGroupInviteRequest(bool block = false)
    {
        this.Block = block;
    }

    /// <summary>
    /// Gets or Sets Block
    /// </summary>
    [DataMember(Name = "block", EmitDefaultValue = true)]
    public bool Block { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class DeclineGroupInviteRequest {\n");
        sb.Append("  Block: ").Append(Block).Append('\n');
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
        return this.Equals(input as DeclineGroupInviteRequest);
    }

    /// <summary>
    /// Returns true if DeclineGroupInviteRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of DeclineGroupInviteRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(DeclineGroupInviteRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Block == input.Block ||
                this.Block.Equals(input.Block)
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
            hashCode = (hashCode * 59) + this.Block.GetHashCode();
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
