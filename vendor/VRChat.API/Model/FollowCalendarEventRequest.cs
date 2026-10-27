
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// FollowCalendarEventRequest
/// </summary>
[DataContract(Name = "FollowCalendarEventRequest")]
public partial class FollowCalendarEventRequest : IEquatable<FollowCalendarEventRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FollowCalendarEventRequest" /> class.
    /// </summary>

    protected FollowCalendarEventRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="FollowCalendarEventRequest" /> class.
    /// </summary>
    /// <param name="isFollowing">isFollowing (required).</param>
    public FollowCalendarEventRequest(bool isFollowing = default)
    {
        this.IsFollowing = isFollowing;
    }

    /// <summary>
    /// Gets or Sets IsFollowing
    /// </summary>
    [DataMember(Name = "isFollowing", IsRequired = true, EmitDefaultValue = true)]
    public bool IsFollowing { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class FollowCalendarEventRequest {\n");
        sb.Append("  IsFollowing: ").Append(IsFollowing).Append('\n');
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
        return this.Equals(input as FollowCalendarEventRequest);
    }

    /// <summary>
    /// Returns true if FollowCalendarEventRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of FollowCalendarEventRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(FollowCalendarEventRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.IsFollowing == input.IsFollowing ||
                this.IsFollowing.Equals(input.IsFollowing)
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
            hashCode = (hashCode * 59) + this.IsFollowing.GetHashCode();
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
