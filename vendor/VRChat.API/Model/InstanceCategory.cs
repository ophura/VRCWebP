
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A category an instance can be listed under.
/// </summary>
[DataContract(Name = "InstanceCategory")]
public partial class InstanceCategory : IEquatable<InstanceCategory>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceCategory" /> class.
    /// </summary>

    protected InstanceCategory() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceCategory" /> class.
    /// </summary>
    /// <param name="deleted">deleted (required).</param>
    /// <param name="iconUrl">iconUrl (required).</param>
    /// <param name="id">id (required).</param>
    /// <param name="name">name (required).</param>
    /// <param name="order">order (required).</param>
    public InstanceCategory(bool deleted = default, string iconUrl = default, string id = default, string name = default, int order = default)
    {
        this.Deleted = deleted;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.IconUrl = iconUrl;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Id = id;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Name = name;
        this.Order = order;
    }

    /// <summary>
    /// Gets or Sets Deleted
    /// </summary>
    [DataMember(Name = "deleted", IsRequired = true, EmitDefaultValue = true)]
    public bool Deleted { get; set; }

    /// <summary>
    /// Gets or Sets IconUrl
    /// </summary>
    [DataMember(Name = "iconUrl", IsRequired = true, EmitDefaultValue = true)]
    public string IconUrl { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Order
    /// </summary>
    [DataMember(Name = "order", IsRequired = true, EmitDefaultValue = true)]
    public int Order { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InstanceCategory {\n");
        sb.Append("  Deleted: ").Append(Deleted).Append('\n');
        sb.Append("  IconUrl: ").Append(IconUrl).Append('\n');
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  Name: ").Append(Name).Append('\n');
        sb.Append("  Order: ").Append(Order).Append('\n');
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
        return this.Equals(input as InstanceCategory);
    }

    /// <summary>
    /// Returns true if InstanceCategory instances are equal
    /// </summary>
    /// <param name="input">Instance of InstanceCategory to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InstanceCategory input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Deleted == input.Deleted ||
                this.Deleted.Equals(input.Deleted)
            ) &&
            (
                this.IconUrl == input.IconUrl ||
                (this.IconUrl != null &&
                this.IconUrl.Equals(input.IconUrl))
            ) &&
            (
                this.Id == input.Id ||
                (this.Id != null &&
                this.Id.Equals(input.Id))
            ) &&
            (
                this.Name == input.Name ||
                (this.Name != null &&
                this.Name.Equals(input.Name))
            ) &&
            (
                this.Order == input.Order ||
                this.Order.Equals(input.Order)
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
            hashCode = (hashCode * 59) + this.Deleted.GetHashCode();
            if (this.IconUrl != null)
            {
                hashCode = (hashCode * 59) + this.IconUrl.GetHashCode();
            }
            if (this.Id != null)
            {
                hashCode = (hashCode * 59) + this.Id.GetHashCode();
            }
            if (this.Name != null)
            {
                hashCode = (hashCode * 59) + this.Name.GetHashCode();
            }
            hashCode = (hashCode * 59) + this.Order.GetHashCode();
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
