
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InfoPushExperiment
/// </summary>
[DataContract(Name = "InfoPushExperiment")]
public partial class InfoPushExperiment : IEquatable<InfoPushExperiment>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InfoPushExperiment" /> class.
    /// </summary>
    /// <param name="key">key.</param>
    /// <param name="variant">variant.</param>
    public InfoPushExperiment(string key = default, string variant = default)
    {
        this.Key = key;
        this.Variant = variant;
    }

    /// <summary>
    /// Gets or Sets Key
    /// </summary>
    [DataMember(Name = "key", EmitDefaultValue = false)]
    public string Key { get; set; }

    /// <summary>
    /// Gets or Sets Variant
    /// </summary>
    [DataMember(Name = "variant", EmitDefaultValue = false)]
    public string Variant { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InfoPushExperiment {\n");
        sb.Append("  Key: ").Append(Key).Append('\n');
        sb.Append("  Variant: ").Append(Variant).Append('\n');
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
        return this.Equals(input as InfoPushExperiment);
    }

    /// <summary>
    /// Returns true if InfoPushExperiment instances are equal
    /// </summary>
    /// <param name="input">Instance of InfoPushExperiment to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InfoPushExperiment input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Key == input.Key ||
                (this.Key != null &&
                this.Key.Equals(input.Key))
            ) &&
            (
                this.Variant == input.Variant ||
                (this.Variant != null &&
                this.Variant.Equals(input.Variant))
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
            if (this.Key != null)
            {
                hashCode = (hashCode * 59) + this.Key.GetHashCode();
            }
            if (this.Variant != null)
            {
                hashCode = (hashCode * 59) + this.Variant.GetHashCode();
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
