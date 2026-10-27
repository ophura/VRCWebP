
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// An error body carrying only a message string. Unlike &#x60;Error&#x60;, there is no nested object and no &#x60;status_code&#x60;, so a consumer that assumes the usual shape will read &#x60;undefined&#x60; from it.
/// </summary>
[DataContract(Name = "BareError")]
public partial class BareError : IEquatable<BareError>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BareError" /> class.
    /// </summary>

    protected BareError() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="BareError" /> class.
    /// </summary>
    /// <param name="error">error (required).</param>
    public BareError(string error = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Error = error;
    }

    /// <summary>
    /// Gets or Sets Error
    /// </summary>
    [DataMember(Name = "error", IsRequired = true, EmitDefaultValue = true)]
    public string Error { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class BareError {\n");
        sb.Append("  Error: ").Append(Error).Append('\n');
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
        return this.Equals(input as BareError);
    }

    /// <summary>
    /// Returns true if BareError instances are equal
    /// </summary>
    /// <param name="input">Instance of BareError to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(BareError input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Error == input.Error ||
                (this.Error != null &&
                this.Error.Equals(input.Error))
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
            if (this.Error != null)
            {
                hashCode = (hashCode * 59) + this.Error.GetHashCode();
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
