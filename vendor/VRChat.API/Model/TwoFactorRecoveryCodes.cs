
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// TwoFactorRecoveryCodes
/// </summary>
[DataContract(Name = "TwoFactorRecoveryCodes")]
public partial class TwoFactorRecoveryCodes : IEquatable<TwoFactorRecoveryCodes>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TwoFactorRecoveryCodes" /> class.
    /// </summary>
    /// <param name="otp">otp.</param>
    /// <param name="requiresTwoFactorAuth">requiresTwoFactorAuth.</param>
    public TwoFactorRecoveryCodes(List<TwoFactorRecoveryCode> otp = default, List<string> requiresTwoFactorAuth = default)
    {
        this.Otp = otp;
        this.RequiresTwoFactorAuth = requiresTwoFactorAuth;
    }

    /// <summary>
    /// Gets or Sets Otp
    /// </summary>
    [DataMember(Name = "otp", EmitDefaultValue = false)]
    public List<TwoFactorRecoveryCode> Otp { get; set; }

    /// <summary>
    /// Gets or Sets RequiresTwoFactorAuth
    /// </summary>
    [DataMember(Name = "requiresTwoFactorAuth", EmitDefaultValue = false)]
    public List<string> RequiresTwoFactorAuth { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class TwoFactorRecoveryCodes {\n");
        sb.Append("  Otp: ").Append(Otp).Append('\n');
        sb.Append("  RequiresTwoFactorAuth: ").Append(RequiresTwoFactorAuth).Append('\n');
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
        return this.Equals(input as TwoFactorRecoveryCodes);
    }

    /// <summary>
    /// Returns true if TwoFactorRecoveryCodes instances are equal
    /// </summary>
    /// <param name="input">Instance of TwoFactorRecoveryCodes to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(TwoFactorRecoveryCodes input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Otp == input.Otp ||
                this.Otp != null &&
                input.Otp != null &&
                this.Otp.SequenceEqual(input.Otp)
            ) &&
            (
                this.RequiresTwoFactorAuth == input.RequiresTwoFactorAuth ||
                this.RequiresTwoFactorAuth != null &&
                input.RequiresTwoFactorAuth != null &&
                this.RequiresTwoFactorAuth.SequenceEqual(input.RequiresTwoFactorAuth)
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
            if (this.Otp != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Otp);
            }
            if (this.RequiresTwoFactorAuth != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.RequiresTwoFactorAuth);
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
