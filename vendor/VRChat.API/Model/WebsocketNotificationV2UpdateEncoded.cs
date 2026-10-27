
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WebsocketNotificationV2UpdateEncoded
/// </summary>
[DataContract(Name = "WebsocketNotificationV2UpdateEncoded")]
public partial class WebsocketNotificationV2UpdateEncoded : IEquatable<WebsocketNotificationV2UpdateEncoded>, IValidatableObject
{
    /// <summary>
    /// Defines Type
    /// </summary>

    public enum TypeEnum
    {
        /// <summary>
        /// Enum NotificationV2Update for value: notification-v2-update
        /// </summary>
        [EnumMember(Value = "notification-v2-update")]
        NotificationV2Update = 1
    }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotificationV2UpdateEncoded" /> class.
    /// </summary>

    protected WebsocketNotificationV2UpdateEncoded() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotificationV2UpdateEncoded" /> class.
    /// </summary>
    /// <param name="content">content (required).</param>
    /// <param name="type">type (required).</param>
    public WebsocketNotificationV2UpdateEncoded(string content = default, TypeEnum type = default)
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
        sb.Append("class WebsocketNotificationV2UpdateEncoded {\n");
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
        return this.Equals(input as WebsocketNotificationV2UpdateEncoded);
    }

    /// <summary>
    /// Returns true if WebsocketNotificationV2UpdateEncoded instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketNotificationV2UpdateEncoded to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketNotificationV2UpdateEncoded input)
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
