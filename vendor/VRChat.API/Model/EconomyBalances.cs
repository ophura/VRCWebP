
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// EconomyBalances
/// </summary>
[DataContract(Name = "EconomyBalances")]
public partial class EconomyBalances : IEquatable<EconomyBalances>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EconomyBalances" /> class.
    /// </summary>

    protected EconomyBalances() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="EconomyBalances" /> class.
    /// </summary>
    /// <param name="balance">balance (required).</param>
    /// <param name="earnings">earnings.</param>
    /// <param name="standard">standard (required).</param>
    public EconomyBalances(int balance = default, int earnings = default, int standard = default)
    {
        this.Balance = balance;
        this.Standard = standard;
        this.Earnings = earnings;
    }

    /// <summary>
    /// Gets or Sets Balance
    /// </summary>
    [DataMember(Name = "balance", IsRequired = true, EmitDefaultValue = true)]
    public int Balance { get; set; }

    /// <summary>
    /// Gets or Sets Earnings
    /// </summary>
    [DataMember(Name = "earnings", EmitDefaultValue = false)]
    public int Earnings { get; set; }

    /// <summary>
    /// Gets or Sets Standard
    /// </summary>
    [DataMember(Name = "standard", IsRequired = true, EmitDefaultValue = true)]
    public int Standard { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class EconomyBalances {\n");
        sb.Append("  Balance: ").Append(Balance).Append('\n');
        sb.Append("  Earnings: ").Append(Earnings).Append('\n');
        sb.Append("  Standard: ").Append(Standard).Append('\n');
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
        return this.Equals(input as EconomyBalances);
    }

    /// <summary>
    /// Returns true if EconomyBalances instances are equal
    /// </summary>
    /// <param name="input">Instance of EconomyBalances to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(EconomyBalances input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Balance == input.Balance ||
                this.Balance.Equals(input.Balance)
            ) &&
            (
                this.Earnings == input.Earnings ||
                this.Earnings.Equals(input.Earnings)
            ) &&
            (
                this.Standard == input.Standard ||
                this.Standard.Equals(input.Standard)
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
            hashCode = (hashCode * 59) + this.Balance.GetHashCode();
            hashCode = (hashCode * 59) + this.Earnings.GetHashCode();
            hashCode = (hashCode * 59) + this.Standard.GetHashCode();
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
