
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// AvatarStyle
/// </summary>
[DataContract(Name = "AvatarStyle")]
public partial class AvatarStyle : IEquatable<AvatarStyle>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvatarStyle" /> class.
    /// </summary>

    protected AvatarStyle() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="AvatarStyle" /> class.
    /// </summary>
    /// <param name="id">id (required).</param>
    /// <param name="styleName">styleName (required).</param>
    public AvatarStyle(string id = default, string styleName = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Id = id;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.StyleName = styleName;
    }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets StyleName
    /// </summary>
    [DataMember(Name = "styleName", IsRequired = true, EmitDefaultValue = true)]
    public string StyleName { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AvatarStyle {\n");
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  StyleName: ").Append(StyleName).Append('\n');
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
        return this.Equals(input as AvatarStyle);
    }

    /// <summary>
    /// Returns true if AvatarStyle instances are equal
    /// </summary>
    /// <param name="input">Instance of AvatarStyle to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(AvatarStyle input)
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
                this.StyleName == input.StyleName ||
                (this.StyleName != null &&
                this.StyleName.Equals(input.StyleName))
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
            if (this.StyleName != null)
            {
                hashCode = (hashCode * 59) + this.StyleName.GetHashCode();
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
