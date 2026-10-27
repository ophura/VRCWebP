
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// GroupGalleryFileOrder
/// </summary>
[DataContract(Name = "GroupGalleryFileOrder")]
public partial class GroupGalleryFileOrder : IEquatable<GroupGalleryFileOrder>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupGalleryFileOrder" /> class.
    /// </summary>

    protected GroupGalleryFileOrder() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupGalleryFileOrder" /> class.
    /// </summary>
    /// <param name="ids">ids (required).</param>
    public GroupGalleryFileOrder(List<string> ids = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Ids = ids;
    }

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
        sb.Append("class GroupGalleryFileOrder {\n");
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
        return this.Equals(input as GroupGalleryFileOrder);
    }

    /// <summary>
    /// Returns true if GroupGalleryFileOrder instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupGalleryFileOrder to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupGalleryFileOrder input)
    {
        if (input == null)
        {
            return false;
        }
        return
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
