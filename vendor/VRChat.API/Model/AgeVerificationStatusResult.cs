
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// The caller&#39;s age verification status.
/// </summary>
[DataContract(Name = "AgeVerificationStatusResult")]
public partial class AgeVerificationStatusResult : IEquatable<AgeVerificationStatusResult>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AgeVerificationStatusResult" /> class.
    /// </summary>

    protected AgeVerificationStatusResult() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="AgeVerificationStatusResult" /> class.
    /// </summary>
    /// <param name="status">status (required).</param>
    public AgeVerificationStatusResult(string status = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Status = status;
    }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
    public string Status { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AgeVerificationStatusResult {\n");
        sb.Append("  Status: ").Append(Status).Append('\n');
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
        return this.Equals(input as AgeVerificationStatusResult);
    }

    /// <summary>
    /// Returns true if AgeVerificationStatusResult instances are equal
    /// </summary>
    /// <param name="input">Instance of AgeVerificationStatusResult to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(AgeVerificationStatusResult input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Status == input.Status ||
                (this.Status != null &&
                this.Status.Equals(input.Status))
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
            if (this.Status != null)
            {
                hashCode = (hashCode * 59) + this.Status.GetHashCode();
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
