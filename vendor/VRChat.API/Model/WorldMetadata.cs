
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WorldMetadata
/// </summary>
[DataContract(Name = "WorldMetadata")]
public partial class WorldMetadata : IEquatable<WorldMetadata>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldMetadata" /> class.
    /// </summary>

    protected WorldMetadata() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldMetadata" /> class.
    /// </summary>
    /// <param name="id">WorldID be \&quot;offline\&quot; on User profiles if you are not friends with that user. (required).</param>
    /// <param name="metadata">metadata (required).</param>
    public WorldMetadata(string id = default, WorldMetadataValue metadata = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Id = id;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Metadata = metadata;
    }

    /// <summary>
    /// WorldID be \&quot;offline\&quot; on User profiles if you are not friends with that user.
    /// </summary>
    /// <value>WorldID be \&quot;offline\&quot; on User profiles if you are not friends with that user.</value>
    [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets Metadata
    /// </summary>
    [DataMember(Name = "metadata", IsRequired = true, EmitDefaultValue = true)]
    public WorldMetadataValue Metadata { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class WorldMetadata {\n");
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  Metadata: ").Append((object)Metadata).Append('\n');
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
        return this.Equals(input as WorldMetadata);
    }

    /// <summary>
    /// Returns true if WorldMetadata instances are equal
    /// </summary>
    /// <param name="input">Instance of WorldMetadata to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WorldMetadata input)
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
                this.Metadata == input.Metadata ||
                (this.Metadata != null &&
                this.Metadata.Equals(input.Metadata))
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
            if (this.Metadata != null)
            {
                hashCode = (hashCode * 59) + this.Metadata.GetHashCode();
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
