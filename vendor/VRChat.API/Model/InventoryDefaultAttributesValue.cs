
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InventoryDefaultAttributesValue
/// </summary>
[DataContract(Name = "InventoryDefaultAttributesValue")]
public partial class InventoryDefaultAttributesValue : IEquatable<InventoryDefaultAttributesValue>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryDefaultAttributesValue" /> class.
    /// </summary>
    /// <param name="defaultValue">defaultValue.</param>
    /// <param name="validator">validator.</param>
    public InventoryDefaultAttributesValue(string defaultValue = default, InventoryDefaultAttributesValueValidator validator = default)
    {
        this.DefaultValue = defaultValue;
        this.Validator = validator;
    }

    /// <summary>
    /// Gets or Sets DefaultValue
    /// </summary>
    [DataMember(Name = "defaultValue", EmitDefaultValue = false)]
    public string DefaultValue { get; set; }

    /// <summary>
    /// Gets or Sets Validator
    /// </summary>
    [DataMember(Name = "validator", EmitDefaultValue = false)]
    public InventoryDefaultAttributesValueValidator Validator { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InventoryDefaultAttributesValue {\n");
        sb.Append("  DefaultValue: ").Append(DefaultValue).Append('\n');
        sb.Append("  Validator: ").Append(Validator).Append('\n');
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
        return this.Equals(input as InventoryDefaultAttributesValue);
    }

    /// <summary>
    /// Returns true if InventoryDefaultAttributesValue instances are equal
    /// </summary>
    /// <param name="input">Instance of InventoryDefaultAttributesValue to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InventoryDefaultAttributesValue input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.DefaultValue == input.DefaultValue ||
                (this.DefaultValue != null &&
                this.DefaultValue.Equals(input.DefaultValue))
            ) &&
            (
                this.Validator == input.Validator ||
                (this.Validator != null &&
                this.Validator.Equals(input.Validator))
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
            if (this.DefaultValue != null)
            {
                hashCode = (hashCode * 59) + this.DefaultValue.GetHashCode();
            }
            if (this.Validator != null)
            {
                hashCode = (hashCode * 59) + this.Validator.GetHashCode();
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
