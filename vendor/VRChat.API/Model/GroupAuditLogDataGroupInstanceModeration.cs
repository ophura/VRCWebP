
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// GroupAuditLogDataGroupInstanceModeration
/// </summary>
[DataContract(Name = "GroupAuditLogDataGroupInstanceModeration")]
public partial class GroupAuditLogDataGroupInstanceModeration : IEquatable<GroupAuditLogDataGroupInstanceModeration>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLogDataGroupInstanceModeration" /> class.
    /// </summary>

    protected GroupAuditLogDataGroupInstanceModeration() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLogDataGroupInstanceModeration" /> class.
    /// </summary>
    /// <param name="location">The instance the target was kicked from or warned in. (required).</param>
    public GroupAuditLogDataGroupInstanceModeration(string location = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Location = location;
    }

    /// <summary>
    /// The instance the target was kicked from or warned in.
    /// </summary>
    /// <value>The instance the target was kicked from or warned in.</value>
    [DataMember(Name = "location", IsRequired = true, EmitDefaultValue = true)]
    public string Location { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class GroupAuditLogDataGroupInstanceModeration {\n");
        sb.Append("  Location: ").Append(Location).Append('\n');
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
        return this.Equals(input as GroupAuditLogDataGroupInstanceModeration);
    }

    /// <summary>
    /// Returns true if GroupAuditLogDataGroupInstanceModeration instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupAuditLogDataGroupInstanceModeration to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupAuditLogDataGroupInstanceModeration input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Location == input.Location ||
                (this.Location != null &&
                this.Location.Equals(input.Location))
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
            if (this.Location != null)
            {
                hashCode = (hashCode * 59) + this.Location.GetHashCode();
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
