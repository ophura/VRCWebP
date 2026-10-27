
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// FavoriteGroupContentsItemBase
/// </summary>
[DataContract(Name = "FavoriteGroupContentsItemBase")]
public partial class FavoriteGroupContentsItemBase : IEquatable<FavoriteGroupContentsItemBase>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupContentsItemBase" /> class.
    /// </summary>

    protected FavoriteGroupContentsItemBase() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupContentsItemBase" /> class.
    /// </summary>
    /// <param name="favoriteId">favoriteId (required).</param>
    /// <param name="id">id (required).</param>
    /// <param name="tags">tags (required).</param>
    public FavoriteGroupContentsItemBase(string favoriteId = default, string id = default, List<string> tags = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.FavoriteId = favoriteId;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Id = id;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Tags = tags;
    }

    /// <summary>
    /// Gets or Sets FavoriteId
    /// </summary>
    [DataMember(Name = "favoriteId", IsRequired = true, EmitDefaultValue = true)]
    public string FavoriteId { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets Tags
    /// </summary>
    [DataMember(Name = "tags", IsRequired = true, EmitDefaultValue = true)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class FavoriteGroupContentsItemBase {\n");
        sb.Append("  FavoriteId: ").Append(FavoriteId).Append('\n');
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  Tags: ").Append(Tags).Append('\n');
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
        return this.Equals(input as FavoriteGroupContentsItemBase);
    }

    /// <summary>
    /// Returns true if FavoriteGroupContentsItemBase instances are equal
    /// </summary>
    /// <param name="input">Instance of FavoriteGroupContentsItemBase to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(FavoriteGroupContentsItemBase input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.FavoriteId == input.FavoriteId ||
                (this.FavoriteId != null &&
                this.FavoriteId.Equals(input.FavoriteId))
            ) &&
            (
                this.Id == input.Id ||
                (this.Id != null &&
                this.Id.Equals(input.Id))
            ) &&
            (
                this.Tags == input.Tags ||
                this.Tags != null &&
                input.Tags != null &&
                this.Tags.SequenceEqual(input.Tags)
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
            if (this.FavoriteId != null)
            {
                hashCode = (hashCode * 59) + this.FavoriteId.GetHashCode();
            }
            if (this.Id != null)
            {
                hashCode = (hashCode * 59) + this.Id.GetHashCode();
            }
            if (this.Tags != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Tags);
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
