
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WebsocketInstanceQueueJoinedEncoded
/// </summary>
[DataContract(Name = "WebsocketInstanceQueueJoinedEncoded")]
public partial class WebsocketInstanceQueueJoinedEncoded : IEquatable<WebsocketInstanceQueueJoinedEncoded>, IValidatableObject
{
    /// <summary>
    /// Defines Type
    /// </summary>

    public enum TypeEnum
    {
        /// <summary>
        /// Enum InstanceQueueJoined for value: instance-queue-joined
        /// </summary>
        [EnumMember(Value = "instance-queue-joined")]
        InstanceQueueJoined = 1
    }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketInstanceQueueJoinedEncoded" /> class.
    /// </summary>

    protected WebsocketInstanceQueueJoinedEncoded() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketInstanceQueueJoinedEncoded" /> class.
    /// </summary>
    /// <param name="content">content (required).</param>
    /// <param name="type">type (required).</param>
    public WebsocketInstanceQueueJoinedEncoded(string content = default, TypeEnum type = default)
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
        sb.Append("class WebsocketInstanceQueueJoinedEncoded {\n");
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
        return this.Equals(input as WebsocketInstanceQueueJoinedEncoded);
    }

    /// <summary>
    /// Returns true if WebsocketInstanceQueueJoinedEncoded instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketInstanceQueueJoinedEncoded to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketInstanceQueueJoinedEncoded input)
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
