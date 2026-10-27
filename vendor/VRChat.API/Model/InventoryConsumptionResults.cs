
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InventoryConsumptionResults
/// </summary>
[DataContract(Name = "InventoryConsumptionResults")]
public partial class InventoryConsumptionResults : IEquatable<InventoryConsumptionResults>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryConsumptionResults" /> class.
    /// </summary>

    protected InventoryConsumptionResults() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryConsumptionResults" /> class.
    /// </summary>
    /// <param name="errors">errors (required).</param>
    /// <param name="inventoryItems">inventoryItems (required).</param>
    /// <param name="inventoryItemsCreated">inventoryItemsCreated (required).</param>
    public InventoryConsumptionResults(List<object> errors = default, List<InventoryItem> inventoryItems = default, int inventoryItemsCreated = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Errors = errors;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.InventoryItems = inventoryItems;
        this.InventoryItemsCreated = inventoryItemsCreated;
    }

    /// <summary>
    /// Gets or Sets Errors
    /// </summary>
    [DataMember(Name = "errors", IsRequired = true, EmitDefaultValue = true)]
    public List<object> Errors { get; set; }

    /// <summary>
    /// Gets or Sets InventoryItems
    /// </summary>
    [DataMember(Name = "inventoryItems", IsRequired = true, EmitDefaultValue = true)]
    public List<InventoryItem> InventoryItems { get; set; }

    /// <summary>
    /// Gets or Sets InventoryItemsCreated
    /// </summary>
    [DataMember(Name = "inventoryItemsCreated", IsRequired = true, EmitDefaultValue = true)]
    public int InventoryItemsCreated { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InventoryConsumptionResults {\n");
        sb.Append("  Errors: ").Append(Errors).Append('\n');
        sb.Append("  InventoryItems: ").Append(InventoryItems).Append('\n');
        sb.Append("  InventoryItemsCreated: ").Append(InventoryItemsCreated).Append('\n');
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
        return this.Equals(input as InventoryConsumptionResults);
    }

    /// <summary>
    /// Returns true if InventoryConsumptionResults instances are equal
    /// </summary>
    /// <param name="input">Instance of InventoryConsumptionResults to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InventoryConsumptionResults input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Errors == input.Errors ||
                this.Errors != null &&
                input.Errors != null &&
                this.Errors.SequenceEqual(input.Errors)
            ) &&
            (
                this.InventoryItems == input.InventoryItems ||
                this.InventoryItems != null &&
                input.InventoryItems != null &&
                this.InventoryItems.SequenceEqual(input.InventoryItems)
            ) &&
            (
                this.InventoryItemsCreated == input.InventoryItemsCreated ||
                this.InventoryItemsCreated.Equals(input.InventoryItemsCreated)
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
            if (this.Errors != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Errors);
            }
            if (this.InventoryItems != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.InventoryItems);
            }
            hashCode = (hashCode * 59) + this.InventoryItemsCreated.GetHashCode();
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
