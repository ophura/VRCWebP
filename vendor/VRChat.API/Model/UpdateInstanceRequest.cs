
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// UpdateInstanceRequest
/// </summary>
[DataContract(Name = "UpdateInstanceRequest")]
public partial class UpdateInstanceRequest : IEquatable<UpdateInstanceRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInstanceRequest" /> class.
    /// </summary>

    protected UpdateInstanceRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInstanceRequest" /> class.
    /// </summary>
    /// <param name="calendarEntryId">Calendar event to link to the instance. Send null to remove the current link. (required).</param>
    public UpdateInstanceRequest(string calendarEntryId = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.CalendarEntryId = calendarEntryId;
    }

    /// <summary>
    /// Calendar event to link to the instance. Send null to remove the current link.
    /// </summary>
    /// <value>Calendar event to link to the instance. Send null to remove the current link.</value>
    [DataMember(Name = "calendarEntryId", IsRequired = true, EmitDefaultValue = true)]
    public string CalendarEntryId { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class UpdateInstanceRequest {\n");
        sb.Append("  CalendarEntryId: ").Append(CalendarEntryId).Append('\n');
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
        return this.Equals(input as UpdateInstanceRequest);
    }

    /// <summary>
    /// Returns true if UpdateInstanceRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of UpdateInstanceRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UpdateInstanceRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.CalendarEntryId == input.CalendarEntryId ||
                (this.CalendarEntryId != null &&
                this.CalendarEntryId.Equals(input.CalendarEntryId))
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
            if (this.CalendarEntryId != null)
            {
                hashCode = (hashCode * 59) + this.CalendarEntryId.GetHashCode();
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
