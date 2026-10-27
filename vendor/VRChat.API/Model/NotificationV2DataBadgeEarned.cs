
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// NotificationV2DataBadgeEarned
/// </summary>
[DataContract(Name = "NotificationV2DataBadgeEarned")]
public partial class NotificationV2DataBadgeEarned : IEquatable<NotificationV2DataBadgeEarned>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2DataBadgeEarned" /> class.
    /// </summary>

    protected NotificationV2DataBadgeEarned() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2DataBadgeEarned" /> class.
    /// </summary>
    /// <param name="badgeDescription">badgeDescription (required).</param>
    /// <param name="badgeId">badgeId (required).</param>
    /// <param name="badgeName">badgeName (required).</param>
    public NotificationV2DataBadgeEarned(string badgeDescription = default, string badgeId = default, string badgeName = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.BadgeDescription = badgeDescription;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.BadgeId = badgeId;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.BadgeName = badgeName;
    }

    /// <summary>
    /// Gets or Sets BadgeDescription
    /// </summary>
    [DataMember(Name = "badgeDescription", IsRequired = true, EmitDefaultValue = true)]
    public string BadgeDescription { get; set; }

    /// <summary>
    /// Gets or Sets BadgeId
    /// </summary>
    [DataMember(Name = "badgeId", IsRequired = true, EmitDefaultValue = true)]
    public string BadgeId { get; set; }

    /// <summary>
    /// Gets or Sets BadgeName
    /// </summary>
    [DataMember(Name = "badgeName", IsRequired = true, EmitDefaultValue = true)]
    public string BadgeName { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class NotificationV2DataBadgeEarned {\n");
        sb.Append("  BadgeDescription: ").Append(BadgeDescription).Append('\n');
        sb.Append("  BadgeId: ").Append(BadgeId).Append('\n');
        sb.Append("  BadgeName: ").Append(BadgeName).Append('\n');
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
        return this.Equals(input as NotificationV2DataBadgeEarned);
    }

    /// <summary>
    /// Returns true if NotificationV2DataBadgeEarned instances are equal
    /// </summary>
    /// <param name="input">Instance of NotificationV2DataBadgeEarned to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(NotificationV2DataBadgeEarned input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.BadgeDescription == input.BadgeDescription ||
                (this.BadgeDescription != null &&
                this.BadgeDescription.Equals(input.BadgeDescription))
            ) &&
            (
                this.BadgeId == input.BadgeId ||
                (this.BadgeId != null &&
                this.BadgeId.Equals(input.BadgeId))
            ) &&
            (
                this.BadgeName == input.BadgeName ||
                (this.BadgeName != null &&
                this.BadgeName.Equals(input.BadgeName))
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
            if (this.BadgeDescription != null)
            {
                hashCode = (hashCode * 59) + this.BadgeDescription.GetHashCode();
            }
            if (this.BadgeId != null)
            {
                hashCode = (hashCode * 59) + this.BadgeId.GetHashCode();
            }
            if (this.BadgeName != null)
            {
                hashCode = (hashCode * 59) + this.BadgeName.GetHashCode();
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
