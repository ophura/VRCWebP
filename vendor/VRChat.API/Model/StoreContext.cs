
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// StoreContext
/// </summary>
[DataContract(Name = "StoreContext")]
public partial class StoreContext : IEquatable<StoreContext>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StoreContext" /> class.
    /// </summary>

    protected StoreContext() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="StoreContext" /> class.
    /// </summary>
    /// <param name="id">id (required).</param>
    /// <param name="imageUrl">imageUrl (required).</param>
    /// <param name="name">name (required).</param>
    public StoreContext(string id = default, string imageUrl = default, string name = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Id = id;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.ImageUrl = imageUrl;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Name = name;
    }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets ImageUrl
    /// </summary>
    [DataMember(Name = "imageUrl", IsRequired = true, EmitDefaultValue = true)]
    public string ImageUrl { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
    public string Name { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class StoreContext {\n");
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  ImageUrl: ").Append(ImageUrl).Append('\n');
        sb.Append("  Name: ").Append(Name).Append('\n');
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
        return this.Equals(input as StoreContext);
    }

    /// <summary>
    /// Returns true if StoreContext instances are equal
    /// </summary>
    /// <param name="input">Instance of StoreContext to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(StoreContext input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Id == input.Id ||
                (this.Id != null &&
                this.Id.Equals(input.Id))
            ) &&
            (
                this.ImageUrl == input.ImageUrl ||
                (this.ImageUrl != null &&
                this.ImageUrl.Equals(input.ImageUrl))
            ) &&
            (
                this.Name == input.Name ||
                (this.Name != null &&
                this.Name.Equals(input.Name))
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
            if (this.Id != null)
            {
                hashCode = (hashCode * 59) + this.Id.GetHashCode();
            }
            if (this.ImageUrl != null)
            {
                hashCode = (hashCode * 59) + this.ImageUrl.GetHashCode();
            }
            if (this.Name != null)
            {
                hashCode = (hashCode * 59) + this.Name.GetHashCode();
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
