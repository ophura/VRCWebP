
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// GroupGalleryFileOrderRequest
/// </summary>
[DataContract(Name = "GroupGalleryFileOrderRequest")]
public partial class GroupGalleryFileOrderRequest : IEquatable<GroupGalleryFileOrderRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupGalleryFileOrderRequest" /> class.
    /// </summary>

    protected GroupGalleryFileOrderRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupGalleryFileOrderRequest" /> class.
    /// </summary>
    /// <param name="galleryId">galleryId (required).</param>
    /// <param name="ids">ids (required).</param>
    public GroupGalleryFileOrderRequest(string galleryId = default, List<string> ids = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.GalleryId = galleryId;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Ids = ids;
    }

    /// <summary>
    /// Gets or Sets GalleryId
    /// </summary>
    [DataMember(Name = "galleryId", IsRequired = true, EmitDefaultValue = true)]
    public string GalleryId { get; set; }

    /// <summary>
    /// Gets or Sets Ids
    /// </summary>
    [DataMember(Name = "ids", IsRequired = true, EmitDefaultValue = true)]
    public List<string> Ids { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class GroupGalleryFileOrderRequest {\n");
        sb.Append("  GalleryId: ").Append(GalleryId).Append('\n');
        sb.Append("  Ids: ").Append(Ids).Append('\n');
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
        return this.Equals(input as GroupGalleryFileOrderRequest);
    }

    /// <summary>
    /// Returns true if GroupGalleryFileOrderRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupGalleryFileOrderRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupGalleryFileOrderRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.GalleryId == input.GalleryId ||
                (this.GalleryId != null &&
                this.GalleryId.Equals(input.GalleryId))
            ) &&
            (
                this.Ids == input.Ids ||
                this.Ids != null &&
                input.Ids != null &&
                this.Ids.SequenceEqual(input.Ids)
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
            if (this.GalleryId != null)
            {
                hashCode = (hashCode * 59) + this.GalleryId.GetHashCode();
            }
            if (this.Ids != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Ids);
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
