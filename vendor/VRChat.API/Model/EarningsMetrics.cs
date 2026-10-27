
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// EarningsMetrics
/// </summary>
[DataContract(Name = "EarningsMetrics")]
public partial class EarningsMetrics : IEquatable<EarningsMetrics>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EarningsMetrics" /> class.
    /// </summary>

    protected EarningsMetrics() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="EarningsMetrics" /> class.
    /// </summary>
    /// <param name="breakdown">breakdown (required).</param>
    /// <param name="sellerId">A users unique ID, usually in the form of &#x60;usr_c1644b5b-3ca4-45b4-97c6-a2a0de70d469&#x60;. Legacy players can have old IDs in the form of &#x60;8JoV9XEdpo&#x60;. The ID can never be changed. (required).</param>
    /// <param name="totals">totals (required).</param>
    public EarningsMetrics(List<object> breakdown = default, string sellerId = default, EarningsMetricsTotals totals = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Breakdown = breakdown;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.SellerId = sellerId;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Totals = totals;
    }

    /// <summary>
    /// Gets or Sets Breakdown
    /// </summary>
    [DataMember(Name = "breakdown", IsRequired = true, EmitDefaultValue = true)]
    public List<object> Breakdown { get; set; }

    /// <summary>
    /// A users unique ID, usually in the form of &#x60;usr_c1644b5b-3ca4-45b4-97c6-a2a0de70d469&#x60;. Legacy players can have old IDs in the form of &#x60;8JoV9XEdpo&#x60;. The ID can never be changed.
    /// </summary>
    /// <value>A users unique ID, usually in the form of &#x60;usr_c1644b5b-3ca4-45b4-97c6-a2a0de70d469&#x60;. Legacy players can have old IDs in the form of &#x60;8JoV9XEdpo&#x60;. The ID can never be changed.</value>
    [DataMember(Name = "sellerId", IsRequired = true, EmitDefaultValue = true)]
    public string SellerId { get; set; }

    /// <summary>
    /// Gets or Sets Totals
    /// </summary>
    [DataMember(Name = "totals", IsRequired = true, EmitDefaultValue = true)]
    public EarningsMetricsTotals Totals { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class EarningsMetrics {\n");
        sb.Append("  Breakdown: ").Append(Breakdown).Append('\n');
        sb.Append("  SellerId: ").Append(SellerId).Append('\n');
        sb.Append("  Totals: ").Append(Totals).Append('\n');
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
        return this.Equals(input as EarningsMetrics);
    }

    /// <summary>
    /// Returns true if EarningsMetrics instances are equal
    /// </summary>
    /// <param name="input">Instance of EarningsMetrics to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(EarningsMetrics input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Breakdown == input.Breakdown ||
                this.Breakdown != null &&
                input.Breakdown != null &&
                this.Breakdown.SequenceEqual(input.Breakdown)
            ) &&
            (
                this.SellerId == input.SellerId ||
                (this.SellerId != null &&
                this.SellerId.Equals(input.SellerId))
            ) &&
            (
                this.Totals == input.Totals ||
                (this.Totals != null &&
                this.Totals.Equals(input.Totals))
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
            if (this.Breakdown != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Breakdown);
            }
            if (this.SellerId != null)
            {
                hashCode = (hashCode * 59) + this.SellerId.GetHashCode();
            }
            if (this.Totals != null)
            {
                hashCode = (hashCode * 59) + this.Totals.GetHashCode();
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
