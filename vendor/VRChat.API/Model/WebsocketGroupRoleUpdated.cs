
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WebsocketGroupRoleUpdated
/// </summary>
[DataContract(Name = "WebsocketGroupRoleUpdated")]
public partial class WebsocketGroupRoleUpdated : IEquatable<WebsocketGroupRoleUpdated>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketGroupRoleUpdated" /> class.
    /// </summary>

    protected WebsocketGroupRoleUpdated() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketGroupRoleUpdated" /> class.
    /// </summary>
    /// <param name="role">role (required).</param>
    public WebsocketGroupRoleUpdated(GroupRole role = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Role = role;
    }

    /// <summary>
    /// Gets or Sets Role
    /// </summary>
    [DataMember(Name = "role", IsRequired = true, EmitDefaultValue = true)]
    public GroupRole Role { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class WebsocketGroupRoleUpdated {\n");
        sb.Append("  Role: ").Append(Role).Append('\n');
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
        return this.Equals(input as WebsocketGroupRoleUpdated);
    }

    /// <summary>
    /// Returns true if WebsocketGroupRoleUpdated instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketGroupRoleUpdated to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketGroupRoleUpdated input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Role == input.Role ||
                (this.Role != null &&
                this.Role.Equals(input.Role))
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
            if (this.Role != null)
            {
                hashCode = (hashCode * 59) + this.Role.GetHashCode();
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
