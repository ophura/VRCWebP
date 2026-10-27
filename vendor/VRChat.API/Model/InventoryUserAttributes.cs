
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InventoryUserAttributes
/// </summary>
[DataContract(Name = "InventoryUserAttributes")]
public partial class InventoryUserAttributes : IEquatable<InventoryUserAttributes>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUserAttributes" /> class.
    /// </summary>
    /// <param name="primaryColor">primaryColor.</param>
    /// <param name="secondaryColor">secondaryColor.</param>
    /// <param name="trailColor">trailColor.</param>
    public InventoryUserAttributes(string primaryColor = default, string secondaryColor = default, string trailColor = default)
    {
        this.PrimaryColor = primaryColor;
        this.SecondaryColor = secondaryColor;
        this.TrailColor = trailColor;
    }

    /// <summary>
    /// Gets or Sets PrimaryColor
    /// </summary>
    [DataMember(Name = "primaryColor", EmitDefaultValue = false)]
    public string PrimaryColor { get; set; }

    /// <summary>
    /// Gets or Sets SecondaryColor
    /// </summary>
    [DataMember(Name = "secondaryColor", EmitDefaultValue = false)]
    public string SecondaryColor { get; set; }

    /// <summary>
    /// Gets or Sets TrailColor
    /// </summary>
    [DataMember(Name = "trailColor", EmitDefaultValue = false)]
    public string TrailColor { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InventoryUserAttributes {\n");
        sb.Append("  PrimaryColor: ").Append(PrimaryColor).Append('\n');
        sb.Append("  SecondaryColor: ").Append(SecondaryColor).Append('\n');
        sb.Append("  TrailColor: ").Append(TrailColor).Append('\n');
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
        return this.Equals(input as InventoryUserAttributes);
    }

    /// <summary>
    /// Returns true if InventoryUserAttributes instances are equal
    /// </summary>
    /// <param name="input">Instance of InventoryUserAttributes to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InventoryUserAttributes input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.PrimaryColor == input.PrimaryColor ||
                (this.PrimaryColor != null &&
                this.PrimaryColor.Equals(input.PrimaryColor))
            ) &&
            (
                this.SecondaryColor == input.SecondaryColor ||
                (this.SecondaryColor != null &&
                this.SecondaryColor.Equals(input.SecondaryColor))
            ) &&
            (
                this.TrailColor == input.TrailColor ||
                (this.TrailColor != null &&
                this.TrailColor.Equals(input.TrailColor))
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
            if (this.PrimaryColor != null)
            {
                hashCode = (hashCode * 59) + this.PrimaryColor.GetHashCode();
            }
            if (this.SecondaryColor != null)
            {
                hashCode = (hashCode * 59) + this.SecondaryColor.GetHashCode();
            }
            if (this.TrailColor != null)
            {
                hashCode = (hashCode * 59) + this.TrailColor.GetHashCode();
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
