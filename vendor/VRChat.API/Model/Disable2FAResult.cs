
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// Disable2FAResult
/// </summary>
[DataContract(Name = "Disable2FAResult")]
public partial class Disable2FAResult : IEquatable<Disable2FAResult>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Disable2FAResult" /> class.
    /// </summary>

    protected Disable2FAResult() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="Disable2FAResult" /> class.
    /// </summary>
    /// <param name="removed">removed (required).</param>
    public Disable2FAResult(bool removed = default)
    {
        this.Removed = removed;
    }

    /// <summary>
    /// Gets or Sets Removed
    /// </summary>
    [DataMember(Name = "removed", IsRequired = true, EmitDefaultValue = true)]
    public bool Removed { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class Disable2FAResult {\n");
        sb.Append("  Removed: ").Append(Removed).Append('\n');
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
        return this.Equals(input as Disable2FAResult);
    }

    /// <summary>
    /// Returns true if Disable2FAResult instances are equal
    /// </summary>
    /// <param name="input">Instance of Disable2FAResult to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Disable2FAResult input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Removed == input.Removed ||
                this.Removed.Equals(input.Removed)
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
            hashCode = (hashCode * 59) + this.Removed.GetHashCode();
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
