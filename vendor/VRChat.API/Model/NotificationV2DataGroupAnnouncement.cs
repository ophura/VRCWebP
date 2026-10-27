
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// NotificationV2DataGroupAnnouncement
/// </summary>
[DataContract(Name = "NotificationV2DataGroupAnnouncement")]
public partial class NotificationV2DataGroupAnnouncement : IEquatable<NotificationV2DataGroupAnnouncement>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2DataGroupAnnouncement" /> class.
    /// </summary>

    protected NotificationV2DataGroupAnnouncement() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2DataGroupAnnouncement" /> class.
    /// </summary>
    /// <param name="announcementTitle">announcementTitle (required).</param>
    /// <param name="groupId">groupId (required).</param>
    /// <param name="groupName">groupName (required).</param>
    public NotificationV2DataGroupAnnouncement(string announcementTitle = default, string groupId = default, string groupName = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.AnnouncementTitle = announcementTitle;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.GroupId = groupId;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.GroupName = groupName;
    }

    /// <summary>
    /// Gets or Sets AnnouncementTitle
    /// </summary>
    [DataMember(Name = "announcementTitle", IsRequired = true, EmitDefaultValue = true)]
    public string AnnouncementTitle { get; set; }

    /// <summary>
    /// Gets or Sets GroupId
    /// </summary>
    [DataMember(Name = "groupId", IsRequired = true, EmitDefaultValue = true)]
    public string GroupId { get; set; }

    /// <summary>
    /// Gets or Sets GroupName
    /// </summary>
    [DataMember(Name = "groupName", IsRequired = true, EmitDefaultValue = true)]
    public string GroupName { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class NotificationV2DataGroupAnnouncement {\n");
        sb.Append("  AnnouncementTitle: ").Append(AnnouncementTitle).Append('\n');
        sb.Append("  GroupId: ").Append(GroupId).Append('\n');
        sb.Append("  GroupName: ").Append(GroupName).Append('\n');
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
        return this.Equals(input as NotificationV2DataGroupAnnouncement);
    }

    /// <summary>
    /// Returns true if NotificationV2DataGroupAnnouncement instances are equal
    /// </summary>
    /// <param name="input">Instance of NotificationV2DataGroupAnnouncement to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(NotificationV2DataGroupAnnouncement input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.AnnouncementTitle == input.AnnouncementTitle ||
                (this.AnnouncementTitle != null &&
                this.AnnouncementTitle.Equals(input.AnnouncementTitle))
            ) &&
            (
                this.GroupId == input.GroupId ||
                (this.GroupId != null &&
                this.GroupId.Equals(input.GroupId))
            ) &&
            (
                this.GroupName == input.GroupName ||
                (this.GroupName != null &&
                this.GroupName.Equals(input.GroupName))
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
            if (this.AnnouncementTitle != null)
            {
                hashCode = (hashCode * 59) + this.AnnouncementTitle.GetHashCode();
            }
            if (this.GroupId != null)
            {
                hashCode = (hashCode * 59) + this.GroupId.GetHashCode();
            }
            if (this.GroupName != null)
            {
                hashCode = (hashCode * 59) + this.GroupName.GetHashCode();
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
