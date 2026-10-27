
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// GroupAuditLogDataGroupInstanceAnnouncement
/// </summary>
[DataContract(Name = "GroupAuditLogDataGroupInstanceAnnouncement")]
public partial class GroupAuditLogDataGroupInstanceAnnouncement : IEquatable<GroupAuditLogDataGroupInstanceAnnouncement>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLogDataGroupInstanceAnnouncement" /> class.
    /// </summary>

    protected GroupAuditLogDataGroupInstanceAnnouncement() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLogDataGroupInstanceAnnouncement" /> class.
    /// </summary>
    /// <param name="message">The announcement message. (required).</param>
    /// <param name="title">The announcement title. (required).</param>
    public GroupAuditLogDataGroupInstanceAnnouncement(string message = default, string title = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Message = message;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Title = title;
    }

    /// <summary>
    /// The announcement message.
    /// </summary>
    /// <value>The announcement message.</value>
    [DataMember(Name = "message", IsRequired = true, EmitDefaultValue = true)]
    public string Message { get; set; }

    /// <summary>
    /// The announcement title.
    /// </summary>
    /// <value>The announcement title.</value>
    [DataMember(Name = "title", IsRequired = true, EmitDefaultValue = true)]
    public string Title { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class GroupAuditLogDataGroupInstanceAnnouncement {\n");
        sb.Append("  Message: ").Append(Message).Append('\n');
        sb.Append("  Title: ").Append(Title).Append('\n');
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
        return this.Equals(input as GroupAuditLogDataGroupInstanceAnnouncement);
    }

    /// <summary>
    /// Returns true if GroupAuditLogDataGroupInstanceAnnouncement instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupAuditLogDataGroupInstanceAnnouncement to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupAuditLogDataGroupInstanceAnnouncement input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Message == input.Message ||
                (this.Message != null &&
                this.Message.Equals(input.Message))
            ) &&
            (
                this.Title == input.Title ||
                (this.Title != null &&
                this.Title.Equals(input.Title))
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
            if (this.Message != null)
            {
                hashCode = (hashCode * 59) + this.Message.GetHashCode();
            }
            if (this.Title != null)
            {
                hashCode = (hashCode * 59) + this.Title.GetHashCode();
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
