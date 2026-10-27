
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// UpdateInviteMessageRequest
/// </summary>
[DataContract(Name = "UpdateInviteMessageRequest")]
public partial class UpdateInviteMessageRequest : IEquatable<UpdateInviteMessageRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInviteMessageRequest" /> class.
    /// </summary>

    protected UpdateInviteMessageRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInviteMessageRequest" /> class.
    /// </summary>
    /// <param name="message">message (required).</param>
    public UpdateInviteMessageRequest(string message = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Message = message;
    }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name = "message", IsRequired = true, EmitDefaultValue = true)]
    public string Message { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class UpdateInviteMessageRequest {\n");
        sb.Append("  Message: ").Append(Message).Append('\n');
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
        return this.Equals(input as UpdateInviteMessageRequest);
    }

    /// <summary>
    /// Returns true if UpdateInviteMessageRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of UpdateInviteMessageRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UpdateInviteMessageRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Message == input.Message ||
                (this.Message != null &&
                this.Message.Equals(input.Message))
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
            if (this.Message != null)
            {
                hashCode = (hashCode * 59) + this.Message.GetHashCode();
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
