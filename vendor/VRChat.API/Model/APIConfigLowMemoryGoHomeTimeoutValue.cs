
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// APIConfigLowMemoryGoHomeTimeoutValue
/// </summary>
[DataContract(Name = "APIConfigLowMemoryGoHomeTimeoutValue")]
public partial class APIConfigLowMemoryGoHomeTimeoutValue : IEquatable<APIConfigLowMemoryGoHomeTimeoutValue>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="APIConfigLowMemoryGoHomeTimeoutValue" /> class.
    /// </summary>
    /// <param name="timeout">timeout.</param>
    public APIConfigLowMemoryGoHomeTimeoutValue(int timeout = default)
    {
        this.Timeout = timeout;
    }

    /// <summary>
    /// Gets or Sets Timeout
    /// </summary>
    [DataMember(Name = "timeout", EmitDefaultValue = false)]
    public int Timeout { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class APIConfigLowMemoryGoHomeTimeoutValue {\n");
        sb.Append("  Timeout: ").Append(Timeout).Append('\n');
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
        return this.Equals(input as APIConfigLowMemoryGoHomeTimeoutValue);
    }

    /// <summary>
    /// Returns true if APIConfigLowMemoryGoHomeTimeoutValue instances are equal
    /// </summary>
    /// <param name="input">Instance of APIConfigLowMemoryGoHomeTimeoutValue to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(APIConfigLowMemoryGoHomeTimeoutValue input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Timeout == input.Timeout ||
                this.Timeout.Equals(input.Timeout)
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
            hashCode = (hashCode * 59) + this.Timeout.GetHashCode();
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
