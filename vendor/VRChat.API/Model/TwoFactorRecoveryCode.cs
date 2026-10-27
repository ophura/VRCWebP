
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// TwoFactorRecoveryCode
/// </summary>
[DataContract(Name = "TwoFactorRecoveryCode")]
public partial class TwoFactorRecoveryCode : IEquatable<TwoFactorRecoveryCode>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TwoFactorRecoveryCode" /> class.
    /// </summary>

    protected TwoFactorRecoveryCode() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="TwoFactorRecoveryCode" /> class.
    /// </summary>
    /// <param name="code">code (required).</param>
    /// <param name="used">used (required).</param>
    public TwoFactorRecoveryCode(string code = default, bool used = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Code = code;
        this.Used = used;
    }

    /// <summary>
    /// Gets or Sets Code
    /// </summary>
    [DataMember(Name = "code", IsRequired = true, EmitDefaultValue = true)]
    public string Code { get; set; }

    /// <summary>
    /// Gets or Sets Used
    /// </summary>
    [DataMember(Name = "used", IsRequired = true, EmitDefaultValue = true)]
    public bool Used { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class TwoFactorRecoveryCode {\n");
        sb.Append("  Code: ").Append(Code).Append('\n');
        sb.Append("  Used: ").Append(Used).Append('\n');
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
        return this.Equals(input as TwoFactorRecoveryCode);
    }

    /// <summary>
    /// Returns true if TwoFactorRecoveryCode instances are equal
    /// </summary>
    /// <param name="input">Instance of TwoFactorRecoveryCode to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(TwoFactorRecoveryCode input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Code == input.Code ||
                (this.Code != null &&
                this.Code.Equals(input.Code))
            ) &&
            (
                this.Used == input.Used ||
                this.Used.Equals(input.Used)
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
            if (this.Code != null)
            {
                hashCode = (hashCode * 59) + this.Code.GetHashCode();
            }
            hashCode = (hashCode * 59) + this.Used.GetHashCode();
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
