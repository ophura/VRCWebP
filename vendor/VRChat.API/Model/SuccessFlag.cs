
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// SuccessFlag
/// </summary>
[DataContract(Name = "SuccessFlag")]
public partial class SuccessFlag : IEquatable<SuccessFlag>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SuccessFlag" /> class.
    /// </summary>

    protected SuccessFlag() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="SuccessFlag" /> class.
    /// </summary>
    /// <param name="success">success (required).</param>
    public SuccessFlag(bool success = default)
    {
        this.Success = success;
    }

    /// <summary>
    /// Gets or Sets Success
    /// </summary>
    [DataMember(Name = "success", IsRequired = true, EmitDefaultValue = true)]
    public bool Success { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class SuccessFlag {\n");
        sb.Append("  Success: ").Append(Success).Append('\n');
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
        return this.Equals(input as SuccessFlag);
    }

    /// <summary>
    /// Returns true if SuccessFlag instances are equal
    /// </summary>
    /// <param name="input">Instance of SuccessFlag to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(SuccessFlag input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Success == input.Success ||
                this.Success.Equals(input.Success)
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
            hashCode = (hashCode * 59) + this.Success.GetHashCode();
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
