
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// APIConfigConstantsInstancePopulationBracket
/// </summary>
[DataContract(Name = "APIConfigConstantsInstancePopulationBracket")]
public partial class APIConfigConstantsInstancePopulationBracket : IEquatable<APIConfigConstantsInstancePopulationBracket>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="APIConfigConstantsInstancePopulationBracket" /> class.
    /// </summary>
    /// <param name="max">Maximum population of an instance in the bracket.</param>
    /// <param name="min">Minimum population of an instance in the bracket.</param>
    public APIConfigConstantsInstancePopulationBracket(int max = default, int min = default)
    {
        this.Max = max;
        this.Min = min;
    }

    /// <summary>
    /// Maximum population of an instance in the bracket
    /// </summary>
    /// <value>Maximum population of an instance in the bracket</value>
    [DataMember(Name = "max", EmitDefaultValue = false)]
    public int Max { get; set; }

    /// <summary>
    /// Minimum population of an instance in the bracket
    /// </summary>
    /// <value>Minimum population of an instance in the bracket</value>
    [DataMember(Name = "min", EmitDefaultValue = false)]
    public int Min { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class APIConfigConstantsInstancePopulationBracket {\n");
        sb.Append("  Max: ").Append(Max).Append('\n');
        sb.Append("  Min: ").Append(Min).Append('\n');
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
        return this.Equals(input as APIConfigConstantsInstancePopulationBracket);
    }

    /// <summary>
    /// Returns true if APIConfigConstantsInstancePopulationBracket instances are equal
    /// </summary>
    /// <param name="input">Instance of APIConfigConstantsInstancePopulationBracket to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(APIConfigConstantsInstancePopulationBracket input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Max == input.Max ||
                this.Max.Equals(input.Max)
            ) &&
            (
                this.Min == input.Min ||
                this.Min.Equals(input.Min)
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
            hashCode = (hashCode * 59) + this.Max.GetHashCode();
            hashCode = (hashCode * 59) + this.Min.GetHashCode();
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
