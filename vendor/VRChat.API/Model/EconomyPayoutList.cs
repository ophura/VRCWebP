
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// EconomyPayoutList
/// </summary>
[DataContract(Name = "EconomyPayoutList")]
public partial class EconomyPayoutList : IEquatable<EconomyPayoutList>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EconomyPayoutList" /> class.
    /// </summary>

    protected EconomyPayoutList() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="EconomyPayoutList" /> class.
    /// </summary>
    /// <param name="payouts">payouts (required).</param>
    public EconomyPayoutList(List<EconomyPayout> payouts = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Payouts = payouts;
    }

    /// <summary>
    /// Gets or Sets Payouts
    /// </summary>
    [DataMember(Name = "payouts", IsRequired = true, EmitDefaultValue = true)]
    public List<EconomyPayout> Payouts { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class EconomyPayoutList {\n");
        sb.Append("  Payouts: ").Append(Payouts).Append('\n');
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
        return this.Equals(input as EconomyPayoutList);
    }

    /// <summary>
    /// Returns true if EconomyPayoutList instances are equal
    /// </summary>
    /// <param name="input">Instance of EconomyPayoutList to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(EconomyPayoutList input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Payouts == input.Payouts ||
                this.Payouts != null &&
                input.Payouts != null &&
                this.Payouts.SequenceEqual(input.Payouts)
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
            if (this.Payouts != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Payouts);
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
