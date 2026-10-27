
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A status response consisting of solely a string description of whether the result of an operation was ok.
/// </summary>
[DataContract(Name = "OkStatus")]
public partial class OkStatus : IEquatable<OkStatus>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OkStatus" /> class.
    /// </summary>

    protected OkStatus() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="OkStatus" /> class.
    /// </summary>
    /// <param name="ok">The actual status itself (required) (default to &quot;maybe?&quot;).</param>
    public OkStatus(string ok = @"maybe?")
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Ok = ok;
    }

    /// <summary>
    /// The actual status itself
    /// </summary>
    /// <value>The actual status itself</value>
    [DataMember(Name = "ok", IsRequired = true, EmitDefaultValue = true)]
    public string Ok { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class OkStatus {\n");
        sb.Append("  Ok: ").Append(Ok).Append('\n');
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
        return this.Equals(input as OkStatus);
    }

    /// <summary>
    /// Returns true if OkStatus instances are equal
    /// </summary>
    /// <param name="input">Instance of OkStatus to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(OkStatus input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Ok == input.Ok ||
                (this.Ok != null &&
                this.Ok.Equals(input.Ok))
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
            if (this.Ok != null)
            {
                hashCode = (hashCode * 59) + this.Ok.GetHashCode();
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
