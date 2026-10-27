
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InventoryDefaultAttributesValueValidator
/// </summary>
[DataContract(Name = "InventoryDefaultAttributesValueValidator")]
public partial class InventoryDefaultAttributesValueValidator : IEquatable<InventoryDefaultAttributesValueValidator>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryDefaultAttributesValueValidator" /> class.
    /// </summary>
    /// <param name="type">type.</param>
    public InventoryDefaultAttributesValueValidator(string type = default)
    {
        this.Type = type;
    }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", EmitDefaultValue = false)]
    public string Type { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InventoryDefaultAttributesValueValidator {\n");
        sb.Append("  Type: ").Append(Type).Append('\n');
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
        return this.Equals(input as InventoryDefaultAttributesValueValidator);
    }

    /// <summary>
    /// Returns true if InventoryDefaultAttributesValueValidator instances are equal
    /// </summary>
    /// <param name="input">Instance of InventoryDefaultAttributesValueValidator to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InventoryDefaultAttributesValueValidator input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Type == input.Type ||
                (this.Type != null &&
                this.Type.Equals(input.Type))
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
            if (this.Type != null)
            {
                hashCode = (hashCode * 59) + this.Type.GetHashCode();
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
