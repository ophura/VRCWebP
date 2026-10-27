
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// NotificationV2DataGroupInformative
/// </summary>
[DataContract(Name = "NotificationV2DataGroupInformative")]
public partial class NotificationV2DataGroupInformative : IEquatable<NotificationV2DataGroupInformative>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2DataGroupInformative" /> class.
    /// </summary>

    protected NotificationV2DataGroupInformative() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2DataGroupInformative" /> class.
    /// </summary>
    /// <param name="groupId">groupId (required).</param>
    /// <param name="groupName">groupName (required).</param>
    /// <param name="transferTargetDisplayName">transferTargetDisplayName.</param>
    public NotificationV2DataGroupInformative(string groupId = default, string groupName = default, string transferTargetDisplayName = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.GroupId = groupId;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.GroupName = groupName;
        this.TransferTargetDisplayName = transferTargetDisplayName;
    }

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
    /// Gets or Sets TransferTargetDisplayName
    /// </summary>
    [DataMember(Name = "transferTargetDisplayName", EmitDefaultValue = false)]
    public string TransferTargetDisplayName { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class NotificationV2DataGroupInformative {\n");
        sb.Append("  GroupId: ").Append(GroupId).Append('\n');
        sb.Append("  GroupName: ").Append(GroupName).Append('\n');
        sb.Append("  TransferTargetDisplayName: ").Append(TransferTargetDisplayName).Append('\n');
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
        return this.Equals(input as NotificationV2DataGroupInformative);
    }

    /// <summary>
    /// Returns true if NotificationV2DataGroupInformative instances are equal
    /// </summary>
    /// <param name="input">Instance of NotificationV2DataGroupInformative to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(NotificationV2DataGroupInformative input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.GroupId == input.GroupId ||
                (this.GroupId != null &&
                this.GroupId.Equals(input.GroupId))
            ) &&
            (
                this.GroupName == input.GroupName ||
                (this.GroupName != null &&
                this.GroupName.Equals(input.GroupName))
            ) &&
            (
                this.TransferTargetDisplayName == input.TransferTargetDisplayName ||
                (this.TransferTargetDisplayName != null &&
                this.TransferTargetDisplayName.Equals(input.TransferTargetDisplayName))
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
            if (this.GroupId != null)
            {
                hashCode = (hashCode * 59) + this.GroupId.GetHashCode();
            }
            if (this.GroupName != null)
            {
                hashCode = (hashCode * 59) + this.GroupName.GetHashCode();
            }
            if (this.TransferTargetDisplayName != null)
            {
                hashCode = (hashCode * 59) + this.TransferTargetDisplayName.GetHashCode();
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
