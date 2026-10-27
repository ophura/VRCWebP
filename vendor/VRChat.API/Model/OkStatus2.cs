
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// Another status response consisting of solely a string description of whether the result of an operation was ok.
/// </summary>
[DataContract(Name = "OkStatus2")]
public partial class OkStatus2 : IEquatable<OkStatus2>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OkStatus2" /> class.
    /// </summary>

    protected OkStatus2() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="OkStatus2" /> class.
    /// </summary>
    /// <param name="oK">The actual status itself (required).</param>
    public OkStatus2(string oK = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.OK = oK;
    }

    /// <summary>
    /// The actual status itself
    /// </summary>
    /// <value>The actual status itself</value>
    [DataMember(Name = "OK", IsRequired = true, EmitDefaultValue = true)]
    public string OK { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class OkStatus2 {\n");
        sb.Append("  OK: ").Append(OK).Append('\n');
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
        return this.Equals(input as OkStatus2);
    }

    /// <summary>
    /// Returns true if OkStatus2 instances are equal
    /// </summary>
    /// <param name="input">Instance of OkStatus2 to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(OkStatus2 input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.OK == input.OK ||
                (this.OK != null &&
                this.OK.Equals(input.OK))
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
            if (this.OK != null)
            {
                hashCode = (hashCode * 59) + this.OK.GetHashCode();
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
