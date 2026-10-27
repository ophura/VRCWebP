
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// UpdateGroupRepresentationRequest
/// </summary>
[DataContract(Name = "UpdateGroupRepresentationRequest")]
public partial class UpdateGroupRepresentationRequest : IEquatable<UpdateGroupRepresentationRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateGroupRepresentationRequest" /> class.
    /// </summary>

    protected UpdateGroupRepresentationRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateGroupRepresentationRequest" /> class.
    /// </summary>
    /// <param name="isRepresenting">Whether the user is representing the group. (required).</param>
    public UpdateGroupRepresentationRequest(bool isRepresenting = default)
    {
        this.IsRepresenting = isRepresenting;
    }

    /// <summary>
    /// Whether the user is representing the group.
    /// </summary>
    /// <value>Whether the user is representing the group.</value>
    [DataMember(Name = "isRepresenting", IsRequired = true, EmitDefaultValue = true)]
    public bool IsRepresenting { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class UpdateGroupRepresentationRequest {\n");
        sb.Append("  IsRepresenting: ").Append(IsRepresenting).Append('\n');
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
        return this.Equals(input as UpdateGroupRepresentationRequest);
    }

    /// <summary>
    /// Returns true if UpdateGroupRepresentationRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of UpdateGroupRepresentationRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UpdateGroupRepresentationRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.IsRepresenting == input.IsRepresenting ||
                this.IsRepresenting.Equals(input.IsRepresenting)
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
            hashCode = (hashCode * 59) + this.IsRepresenting.GetHashCode();
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
