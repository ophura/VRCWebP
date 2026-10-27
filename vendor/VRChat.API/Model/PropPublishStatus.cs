
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// PropPublishStatus
/// </summary>
[DataContract(Name = "PropPublishStatus")]
public partial class PropPublishStatus : IEquatable<PropPublishStatus>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropPublishStatus" /> class.
    /// </summary>
    /// <param name="canPublish">canPublish (default to false).</param>
    public PropPublishStatus(bool canPublish = false)
    {
        this.CanPublish = canPublish;
    }

    /// <summary>
    /// Gets or Sets CanPublish
    /// </summary>
    [DataMember(Name = "canPublish", EmitDefaultValue = true)]
    public bool CanPublish { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class PropPublishStatus {\n");
        sb.Append("  CanPublish: ").Append(CanPublish).Append('\n');
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
        return this.Equals(input as PropPublishStatus);
    }

    /// <summary>
    /// Returns true if PropPublishStatus instances are equal
    /// </summary>
    /// <param name="input">Instance of PropPublishStatus to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(PropPublishStatus input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.CanPublish == input.CanPublish ||
                this.CanPublish.Equals(input.CanPublish)
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
            hashCode = (hashCode * 59) + this.CanPublish.GetHashCode();
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
