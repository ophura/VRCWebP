
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// Response
/// </summary>
[DataContract(Name = "Response")]
public partial class Response : IEquatable<Response>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Response" /> class.
    /// </summary>

    protected Response() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="Response" /> class.
    /// </summary>
    /// <param name="message">message.</param>
    /// <param name="statusCode">statusCode (required).</param>
    /// <param name="wafCode">wafCode.</param>
    public Response(string message = default, int statusCode = default, int wafCode = default)
    {
        this.StatusCode = statusCode;
        this.Message = message;
        this.WafCode = wafCode;
    }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name = "message", EmitDefaultValue = false)]
    public string Message { get; set; }

    /// <summary>
    /// Gets or Sets StatusCode
    /// </summary>
    [DataMember(Name = "status_code", IsRequired = true, EmitDefaultValue = true)]
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or Sets WafCode
    /// </summary>
    [DataMember(Name = "waf_code", EmitDefaultValue = false)]
    public int WafCode { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class Response {\n");
        sb.Append("  Message: ").Append(Message).Append('\n');
        sb.Append("  StatusCode: ").Append(StatusCode).Append('\n');
        sb.Append("  WafCode: ").Append(WafCode).Append('\n');
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
        return this.Equals(input as Response);
    }

    /// <summary>
    /// Returns true if Response instances are equal
    /// </summary>
    /// <param name="input">Instance of Response to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Response input)
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
            ) &&
            (
                this.StatusCode == input.StatusCode ||
                this.StatusCode.Equals(input.StatusCode)
            ) &&
            (
                this.WafCode == input.WafCode ||
                this.WafCode.Equals(input.WafCode)
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
            hashCode = (hashCode * 59) + this.StatusCode.GetHashCode();
            hashCode = (hashCode * 59) + this.WafCode.GetHashCode();
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
        // Message (string) minLength
        if (this.Message != null && this.Message.Length < 1)
        {
            yield return new ValidationResult("Invalid value for Message, length must be greater than 1.", new [] { "Message" });
        }

        // StatusCode (int) minimum
        if (this.StatusCode < (int)100)
        {
            yield return new ValidationResult("Invalid value for StatusCode, must be a value greater than or equal to 100.", new [] { "StatusCode" });
        }

        yield break;
    }
}
