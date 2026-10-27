
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A join state field&#39;s value before and after an update.
/// </summary>
[DataContract(Name = "GroupAuditLogJoinStateChange")]
public partial class GroupAuditLogJoinStateChange : IEquatable<GroupAuditLogJoinStateChange>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets New
    /// </summary>
    [DataMember(Name = "new", IsRequired = true, EmitDefaultValue = true)]
    public GroupJoinState New { get; set; }

    /// <summary>
    /// Gets or Sets Old
    /// </summary>
    [DataMember(Name = "old", IsRequired = true, EmitDefaultValue = true)]
    public GroupJoinState Old { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLogJoinStateChange" /> class.
    /// </summary>

    protected GroupAuditLogJoinStateChange() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLogJoinStateChange" /> class.
    /// </summary>
    /// <param name="varNew">varNew (required).</param>
    /// <param name="old">old (required).</param>
    public GroupAuditLogJoinStateChange(GroupJoinState varNew = default, GroupJoinState old = default)
    {
        this.New = varNew;
        this.Old = old;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class GroupAuditLogJoinStateChange {\n");
        sb.Append("  New: ").Append(New).Append('\n');
        sb.Append("  Old: ").Append(Old).Append('\n');
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
        return this.Equals(input as GroupAuditLogJoinStateChange);
    }

    /// <summary>
    /// Returns true if GroupAuditLogJoinStateChange instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupAuditLogJoinStateChange to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupAuditLogJoinStateChange input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.New == input.New ||
                this.New.Equals(input.New)
            ) &&
            (
                this.Old == input.Old ||
                this.Old.Equals(input.Old)
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
            hashCode = (hashCode * 59) + this.New.GetHashCode();
            hashCode = (hashCode * 59) + this.Old.GetHashCode();
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
