
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// EquipInventoryItemRequest
/// </summary>
[DataContract(Name = "EquipInventoryItemRequest")]
public partial class EquipInventoryItemRequest : IEquatable<EquipInventoryItemRequest>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets EquipSlot
    /// </summary>
    [DataMember(Name = "equipSlot", IsRequired = true, EmitDefaultValue = true)]
    public InventoryEquipSlot EquipSlot { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="EquipInventoryItemRequest" /> class.
    /// </summary>

    protected EquipInventoryItemRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="EquipInventoryItemRequest" /> class.
    /// </summary>
    /// <param name="equipSlot">equipSlot (required).</param>
    public EquipInventoryItemRequest(InventoryEquipSlot equipSlot = default)
    {
        this.EquipSlot = equipSlot;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class EquipInventoryItemRequest {\n");
        sb.Append("  EquipSlot: ").Append(EquipSlot).Append('\n');
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
        return this.Equals(input as EquipInventoryItemRequest);
    }

    /// <summary>
    /// Returns true if EquipInventoryItemRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of EquipInventoryItemRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(EquipInventoryItemRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.EquipSlot == input.EquipSlot ||
                this.EquipSlot.Equals(input.EquipSlot)
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
            hashCode = (hashCode * 59) + this.EquipSlot.GetHashCode();
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
