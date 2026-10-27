
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// RewardRedemptionResult
/// </summary>
[DataContract(Name = "RewardRedemptionResult")]
public partial class RewardRedemptionResult : IEquatable<RewardRedemptionResult>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RewardRedemptionResult" /> class.
    /// </summary>

    protected RewardRedemptionResult() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="RewardRedemptionResult" /> class.
    /// </summary>
    /// <param name="redeemedRewards">redeemedRewards (required).</param>
    /// <param name="redemptionCode">redemptionCode (required).</param>
    public RewardRedemptionResult(List<RewardRedemption> redeemedRewards = default, string redemptionCode = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.RedeemedRewards = redeemedRewards;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.RedemptionCode = redemptionCode;
    }

    /// <summary>
    /// Gets or Sets RedeemedRewards
    /// </summary>
    [DataMember(Name = "redeemedRewards", IsRequired = true, EmitDefaultValue = true)]
    public List<RewardRedemption> RedeemedRewards { get; set; }

    /// <summary>
    /// Gets or Sets RedemptionCode
    /// </summary>
    [DataMember(Name = "redemptionCode", IsRequired = true, EmitDefaultValue = true)]
    public string RedemptionCode { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class RewardRedemptionResult {\n");
        sb.Append("  RedeemedRewards: ").Append(RedeemedRewards).Append('\n');
        sb.Append("  RedemptionCode: ").Append(RedemptionCode).Append('\n');
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
        return this.Equals(input as RewardRedemptionResult);
    }

    /// <summary>
    /// Returns true if RewardRedemptionResult instances are equal
    /// </summary>
    /// <param name="input">Instance of RewardRedemptionResult to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(RewardRedemptionResult input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.RedeemedRewards == input.RedeemedRewards ||
                this.RedeemedRewards != null &&
                input.RedeemedRewards != null &&
                this.RedeemedRewards.SequenceEqual(input.RedeemedRewards)
            ) &&
            (
                this.RedemptionCode == input.RedemptionCode ||
                (this.RedemptionCode != null &&
                this.RedemptionCode.Equals(input.RedemptionCode))
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
            if (this.RedeemedRewards != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.RedeemedRewards);
            }
            if (this.RedemptionCode != null)
            {
                hashCode = (hashCode * 59) + this.RedemptionCode.GetHashCode();
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
