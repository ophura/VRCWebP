
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// UpdateListingRequest
/// </summary>
[DataContract(Name = "UpdateListingRequest")]
public partial class UpdateListingRequest : IEquatable<UpdateListingRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateListingRequest" /> class.
    /// </summary>

    protected UpdateListingRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateListingRequest" /> class.
    /// </summary>
    /// <param name="active">active (required).</param>
    public UpdateListingRequest(bool active = default)
    {
        this.Active = active;
    }

    /// <summary>
    /// Gets or Sets Active
    /// </summary>
    [DataMember(Name = "active", IsRequired = true, EmitDefaultValue = true)]
    public bool Active { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class UpdateListingRequest {\n");
        sb.Append("  Active: ").Append(Active).Append('\n');
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
        return this.Equals(input as UpdateListingRequest);
    }

    /// <summary>
    /// Returns true if UpdateListingRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of UpdateListingRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UpdateListingRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Active == input.Active ||
                this.Active.Equals(input.Active)
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
            hashCode = (hashCode * 59) + this.Active.GetHashCode();
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
