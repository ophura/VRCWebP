
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// PurchaseContextData
/// </summary>
[DataContract(Name = "PurchaseContextData")]
public partial class PurchaseContextData : IEquatable<PurchaseContextData>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets LocationType
    /// </summary>
    [DataMember(Name = "locationType", IsRequired = true, EmitDefaultValue = true)]
    public ProductPurchaseLocationType LocationType { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="PurchaseContextData" /> class.
    /// </summary>

    protected PurchaseContextData() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="PurchaseContextData" /> class.
    /// </summary>
    /// <param name="locationType">locationType (required).</param>
    /// <param name="storeId">storeId.</param>
    /// <param name="worldId">WorldID be \&quot;offline\&quot; on User profiles if you are not friends with that user..</param>
    public PurchaseContextData(ProductPurchaseLocationType locationType = default, string storeId = default, string worldId = default)
    {
        this.LocationType = locationType;
        this.StoreId = storeId;
        this.WorldId = worldId;
    }

    /// <summary>
    /// Gets or Sets StoreId
    /// </summary>
    [DataMember(Name = "storeId", EmitDefaultValue = false)]
    public string StoreId { get; set; }

    /// <summary>
    /// WorldID be \&quot;offline\&quot; on User profiles if you are not friends with that user.
    /// </summary>
    /// <value>WorldID be \&quot;offline\&quot; on User profiles if you are not friends with that user.</value>
    [DataMember(Name = "worldId", EmitDefaultValue = false)]
    public string WorldId { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class PurchaseContextData {\n");
        sb.Append("  LocationType: ").Append(LocationType).Append('\n');
        sb.Append("  StoreId: ").Append(StoreId).Append('\n');
        sb.Append("  WorldId: ").Append(WorldId).Append('\n');
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
        return this.Equals(input as PurchaseContextData);
    }

    /// <summary>
    /// Returns true if PurchaseContextData instances are equal
    /// </summary>
    /// <param name="input">Instance of PurchaseContextData to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(PurchaseContextData input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.LocationType == input.LocationType ||
                this.LocationType.Equals(input.LocationType)
            ) &&
            (
                this.StoreId == input.StoreId ||
                (this.StoreId != null &&
                this.StoreId.Equals(input.StoreId))
            ) &&
            (
                this.WorldId == input.WorldId ||
                (this.WorldId != null &&
                this.WorldId.Equals(input.WorldId))
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
            hashCode = (hashCode * 59) + this.LocationType.GetHashCode();
            if (this.StoreId != null)
            {
                hashCode = (hashCode * 59) + this.StoreId.GetHashCode();
            }
            if (this.WorldId != null)
            {
                hashCode = (hashCode * 59) + this.WorldId.GetHashCode();
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
