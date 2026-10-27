
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// APIConfigIOSVersion
/// </summary>
[DataContract(Name = "APIConfigIOSVersion")]
public partial class APIConfigIOSVersion : IEquatable<APIConfigIOSVersion>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="APIConfigIOSVersion" /> class.
    /// </summary>

    protected APIConfigIOSVersion() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="APIConfigIOSVersion" /> class.
    /// </summary>
    /// <param name="major">major (required).</param>
    /// <param name="minor">minor (required).</param>
    public APIConfigIOSVersion(int major = default, int minor = default)
    {
        this.Major = major;
        this.Minor = minor;
    }

    /// <summary>
    /// Gets or Sets Major
    /// </summary>
    [DataMember(Name = "major", IsRequired = true, EmitDefaultValue = true)]
    public int Major { get; set; }

    /// <summary>
    /// Gets or Sets Minor
    /// </summary>
    [DataMember(Name = "minor", IsRequired = true, EmitDefaultValue = true)]
    public int Minor { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class APIConfigIOSVersion {\n");
        sb.Append("  Major: ").Append(Major).Append('\n');
        sb.Append("  Minor: ").Append(Minor).Append('\n');
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
        return this.Equals(input as APIConfigIOSVersion);
    }

    /// <summary>
    /// Returns true if APIConfigIOSVersion instances are equal
    /// </summary>
    /// <param name="input">Instance of APIConfigIOSVersion to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(APIConfigIOSVersion input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Major == input.Major ||
                this.Major.Equals(input.Major)
            ) &&
            (
                this.Minor == input.Minor ||
                this.Minor.Equals(input.Minor)
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
            hashCode = (hashCode * 59) + this.Major.GetHashCode();
            hashCode = (hashCode * 59) + this.Minor.GetHashCode();
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
