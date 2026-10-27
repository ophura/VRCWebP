
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// ChangeWorldTagsRequest
/// </summary>
[DataContract(Name = "ChangeWorldTagsRequest")]
public partial class ChangeWorldTagsRequest : IEquatable<ChangeWorldTagsRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeWorldTagsRequest" /> class.
    /// </summary>

    protected ChangeWorldTagsRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeWorldTagsRequest" /> class.
    /// </summary>
    /// <param name="tags">The tags being added or removed. (required).</param>
    public ChangeWorldTagsRequest(List<string> tags = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Tags = tags;
    }

    /// <summary>
    /// The tags being added or removed.
    /// </summary>
    /// <value>The tags being added or removed.</value>
    [DataMember(Name = "tags", IsRequired = true, EmitDefaultValue = true)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class ChangeWorldTagsRequest {\n");
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
        return this.Equals(input as ChangeWorldTagsRequest);
    }

    /// <summary>
    /// Returns true if ChangeWorldTagsRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of ChangeWorldTagsRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(ChangeWorldTagsRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
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
