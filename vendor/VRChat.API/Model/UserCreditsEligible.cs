
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// UserCreditsEligible
/// </summary>
[DataContract(Name = "UserCreditsEligible")]
public partial class UserCreditsEligible : IEquatable<UserCreditsEligible>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreditsEligible" /> class.
    /// </summary>

    protected UserCreditsEligible() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreditsEligible" /> class.
    /// </summary>
    /// <param name="eligible">eligible (required).</param>
    /// <param name="reason">reason.</param>
    public UserCreditsEligible(bool eligible = default, string reason = default)
    {
        this.Eligible = eligible;
        this.Reason = reason;
    }

    /// <summary>
    /// Gets or Sets Eligible
    /// </summary>
    [DataMember(Name = "eligible", IsRequired = true, EmitDefaultValue = true)]
    public bool Eligible { get; set; }

    /// <summary>
    /// Gets or Sets Reason
    /// </summary>
    [DataMember(Name = "reason", EmitDefaultValue = false)]
    public string Reason { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class UserCreditsEligible {\n");
        sb.Append("  Eligible: ").Append(Eligible).Append('\n');
        sb.Append("  Reason: ").Append(Reason).Append('\n');
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
        return this.Equals(input as UserCreditsEligible);
    }

    /// <summary>
    /// Returns true if UserCreditsEligible instances are equal
    /// </summary>
    /// <param name="input">Instance of UserCreditsEligible to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UserCreditsEligible input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Eligible == input.Eligible ||
                this.Eligible.Equals(input.Eligible)
            ) &&
            (
                this.Reason == input.Reason ||
                (this.Reason != null &&
                this.Reason.Equals(input.Reason))
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
            hashCode = (hashCode * 59) + this.Eligible.GetHashCode();
            if (this.Reason != null)
            {
                hashCode = (hashCode * 59) + this.Reason.GetHashCode();
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
