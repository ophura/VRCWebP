
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// CreateAvatarModerationRequest
/// </summary>
[DataContract(Name = "CreateAvatarModerationRequest")]
public partial class CreateAvatarModerationRequest : IEquatable<CreateAvatarModerationRequest>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets AvatarModerationType
    /// </summary>
    [DataMember(Name = "avatarModerationType", IsRequired = true, EmitDefaultValue = true)]
    public AvatarModerationType AvatarModerationType { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAvatarModerationRequest" /> class.
    /// </summary>

    protected CreateAvatarModerationRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAvatarModerationRequest" /> class.
    /// </summary>
    /// <param name="avatarModerationType">avatarModerationType (required).</param>
    /// <param name="targetAvatarId">targetAvatarId (required).</param>
    public CreateAvatarModerationRequest(AvatarModerationType avatarModerationType = default, string targetAvatarId = default)
    {
        this.AvatarModerationType = avatarModerationType;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.TargetAvatarId = targetAvatarId;
    }

    /// <summary>
    /// Gets or Sets TargetAvatarId
    /// </summary>
    [DataMember(Name = "targetAvatarId", IsRequired = true, EmitDefaultValue = true)]
    public string TargetAvatarId { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class CreateAvatarModerationRequest {\n");
        sb.Append("  AvatarModerationType: ").Append(AvatarModerationType).Append('\n');
        sb.Append("  TargetAvatarId: ").Append(TargetAvatarId).Append('\n');
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
        return this.Equals(input as CreateAvatarModerationRequest);
    }

    /// <summary>
    /// Returns true if CreateAvatarModerationRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of CreateAvatarModerationRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(CreateAvatarModerationRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.AvatarModerationType == input.AvatarModerationType ||
                this.AvatarModerationType.Equals(input.AvatarModerationType)
            ) &&
            (
                this.TargetAvatarId == input.TargetAvatarId ||
                (this.TargetAvatarId != null &&
                this.TargetAvatarId.Equals(input.TargetAvatarId))
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
            hashCode = (hashCode * 59) + this.AvatarModerationType.GetHashCode();
            if (this.TargetAvatarId != null)
            {
                hashCode = (hashCode * 59) + this.TargetAvatarId.GetHashCode();
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
