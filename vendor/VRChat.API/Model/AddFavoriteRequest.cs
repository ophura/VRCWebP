
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// AddFavoriteRequest
/// </summary>
[DataContract(Name = "AddFavoriteRequest")]
public partial class AddFavoriteRequest : IEquatable<AddFavoriteRequest>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public FavoriteType Type { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="AddFavoriteRequest" /> class.
    /// </summary>

    protected AddFavoriteRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="AddFavoriteRequest" /> class.
    /// </summary>
    /// <param name="favoriteId">Must be either AvatarID, WorldID or UserID. (required).</param>
    /// <param name="tags">Tags indicate which group this favorite belongs to. Adding multiple groups makes it show up in all. Removing it from one in that case removes it from all. (required).</param>
    /// <param name="type">type (required).</param>
    public AddFavoriteRequest(string favoriteId = default, List<string> tags = default, FavoriteType type = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.FavoriteId = favoriteId;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Tags = tags;
        this.Type = type;
    }

    /// <summary>
    /// Must be either AvatarID, WorldID or UserID.
    /// </summary>
    /// <value>Must be either AvatarID, WorldID or UserID.</value>
    [DataMember(Name = "favoriteId", IsRequired = true, EmitDefaultValue = true)]
    public string FavoriteId { get; set; }

    /// <summary>
    /// Tags indicate which group this favorite belongs to. Adding multiple groups makes it show up in all. Removing it from one in that case removes it from all.
    /// </summary>
    /// <value>Tags indicate which group this favorite belongs to. Adding multiple groups makes it show up in all. Removing it from one in that case removes it from all.</value>
    [DataMember(Name = "tags", IsRequired = true, EmitDefaultValue = true)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AddFavoriteRequest {\n");
        sb.Append("  FavoriteId: ").Append(FavoriteId).Append('\n');
        sb.Append("  Tags: ").Append(Tags).Append('\n');
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
        return this.Equals(input as AddFavoriteRequest);
    }

    /// <summary>
    /// Returns true if AddFavoriteRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of AddFavoriteRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(AddFavoriteRequest input)
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
                this.Tags == input.Tags ||
                this.Tags != null &&
                input.Tags != null &&
                this.Tags.SequenceEqual(input.Tags)
            ) &&
            (
                this.Type == input.Type ||
                this.Type.Equals(input.Type)
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
            if (this.Tags != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Tags);
            }
            hashCode = (hashCode * 59) + this.Type.GetHashCode();
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
