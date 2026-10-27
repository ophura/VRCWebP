
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WebsocketGroupMemberUpdatedEncoded
/// </summary>
[DataContract(Name = "WebsocketGroupMemberUpdatedEncoded")]
public partial class WebsocketGroupMemberUpdatedEncoded : IEquatable<WebsocketGroupMemberUpdatedEncoded>, IValidatableObject
{
    /// <summary>
    /// Defines Type
    /// </summary>

    public enum TypeEnum
    {
        /// <summary>
        /// Enum GroupMemberUpdated for value: group-member-updated
        /// </summary>
        [EnumMember(Value = "group-member-updated")]
        GroupMemberUpdated = 1
    }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketGroupMemberUpdatedEncoded" /> class.
    /// </summary>

    protected WebsocketGroupMemberUpdatedEncoded() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketGroupMemberUpdatedEncoded" /> class.
    /// </summary>
    /// <param name="content">content (required).</param>
    /// <param name="type">type (required).</param>
    public WebsocketGroupMemberUpdatedEncoded(string content = default, TypeEnum type = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Content = content;
        this.Type = type;
    }

    /// <summary>
    /// Gets or Sets Content
    /// </summary>
    [DataMember(Name = "content", IsRequired = true, EmitDefaultValue = true)]
    public string Content { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class WebsocketGroupMemberUpdatedEncoded {\n");
        sb.Append("  Content: ").Append(Content).Append('\n');
        sb.Append("  Type: ").Append(Type).Append('\n');
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
        return this.Equals(input as WebsocketGroupMemberUpdatedEncoded);
    }

    /// <summary>
    /// Returns true if WebsocketGroupMemberUpdatedEncoded instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketGroupMemberUpdatedEncoded to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketGroupMemberUpdatedEncoded input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Content == input.Content ||
                (this.Content != null &&
                this.Content.Equals(input.Content))
            ) &&
            (
                this.Type == input.Type ||
                this.Type.Equals(input.Type)
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
            if (this.Content != null)
            {
                hashCode = (hashCode * 59) + this.Content.GetHashCode();
            }
            hashCode = (hashCode * 59) + this.Type.GetHashCode();
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
