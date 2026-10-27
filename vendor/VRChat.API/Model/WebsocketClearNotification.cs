
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WebsocketClearNotification
/// </summary>
[DataContract(Name = "WebsocketClearNotification")]
public partial class WebsocketClearNotification : IEquatable<WebsocketClearNotification>, IValidatableObject
{
    /// <summary>
    /// Defines Type
    /// </summary>

    public enum TypeEnum
    {
        /// <summary>
        /// Enum ClearNotification for value: clear-notification
        /// </summary>
        [EnumMember(Value = "clear-notification")]
        ClearNotification = 1
    }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public TypeEnum Type { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketClearNotification" /> class.
    /// </summary>

    protected WebsocketClearNotification() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketClearNotification" /> class.
    /// </summary>
    /// <param name="type">type (required).</param>
    public WebsocketClearNotification(TypeEnum type = default)
    {
        this.Type = type;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class WebsocketClearNotification {\n");
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
        return this.Equals(input as WebsocketClearNotification);
    }

    /// <summary>
    /// Returns true if WebsocketClearNotification instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketClearNotification to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketClearNotification input)
    {
        if (input == null)
        {
            return false;
        }
        return
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
