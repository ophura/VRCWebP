
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// NotificationV2DataGroupTransfer
/// </summary>
[DataContract(Name = "NotificationV2DataGroupTransfer")]
public partial class NotificationV2DataGroupTransfer : IEquatable<NotificationV2DataGroupTransfer>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2DataGroupTransfer" /> class.
    /// </summary>

    protected NotificationV2DataGroupTransfer() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2DataGroupTransfer" /> class.
    /// </summary>
    /// <param name="groupName">groupName (required).</param>
    /// <param name="ownerUserDisplayName">ownerUserDisplayName (required).</param>
    public NotificationV2DataGroupTransfer(string groupName = default, string ownerUserDisplayName = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.GroupName = groupName;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.OwnerUserDisplayName = ownerUserDisplayName;
    }

    /// <summary>
    /// Gets or Sets GroupName
    /// </summary>
    [DataMember(Name = "groupName", IsRequired = true, EmitDefaultValue = true)]
    public string GroupName { get; set; }

    /// <summary>
    /// Gets or Sets OwnerUserDisplayName
    /// </summary>
    [DataMember(Name = "ownerUserDisplayName", IsRequired = true, EmitDefaultValue = true)]
    public string OwnerUserDisplayName { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class NotificationV2DataGroupTransfer {\n");
        sb.Append("  GroupName: ").Append(GroupName).Append('\n');
        sb.Append("  OwnerUserDisplayName: ").Append(OwnerUserDisplayName).Append('\n');
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
        return this.Equals(input as NotificationV2DataGroupTransfer);
    }

    /// <summary>
    /// Returns true if NotificationV2DataGroupTransfer instances are equal
    /// </summary>
    /// <param name="input">Instance of NotificationV2DataGroupTransfer to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(NotificationV2DataGroupTransfer input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.GroupName == input.GroupName ||
                (this.GroupName != null &&
                this.GroupName.Equals(input.GroupName))
            ) &&
            (
                this.OwnerUserDisplayName == input.OwnerUserDisplayName ||
                (this.OwnerUserDisplayName != null &&
                this.OwnerUserDisplayName.Equals(input.OwnerUserDisplayName))
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
            if (this.GroupName != null)
            {
                hashCode = (hashCode * 59) + this.GroupName.GetHashCode();
            }
            if (this.OwnerUserDisplayName != null)
            {
                hashCode = (hashCode * 59) + this.OwnerUserDisplayName.GetHashCode();
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
