
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// APIConfigConstantsLanguage
/// </summary>
[DataContract(Name = "APIConfigConstantsLanguage")]
public partial class APIConfigConstantsLanguage : IEquatable<APIConfigConstantsLanguage>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="APIConfigConstantsLanguage" /> class.
    /// </summary>
    /// <param name="sPOKENLANGUAGEOPTIONS">Supported spoken language options.</param>
    public APIConfigConstantsLanguage(Dictionary<string, string> sPOKENLANGUAGEOPTIONS = default)
    {
        this.SPOKEN_LANGUAGE_OPTIONS = sPOKENLANGUAGEOPTIONS;
    }

    /// <summary>
    /// Supported spoken language options
    /// </summary>
    /// <value>Supported spoken language options</value>
    [DataMember(Name = "SPOKEN_LANGUAGE_OPTIONS", EmitDefaultValue = false)]
    public Dictionary<string, string> SPOKEN_LANGUAGE_OPTIONS { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class APIConfigConstantsLanguage {\n");
        sb.Append("  SPOKEN_LANGUAGE_OPTIONS: ").Append(SPOKEN_LANGUAGE_OPTIONS).Append('\n');
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
        return this.Equals(input as APIConfigConstantsLanguage);
    }

    /// <summary>
    /// Returns true if APIConfigConstantsLanguage instances are equal
    /// </summary>
    /// <param name="input">Instance of APIConfigConstantsLanguage to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(APIConfigConstantsLanguage input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.SPOKEN_LANGUAGE_OPTIONS == input.SPOKEN_LANGUAGE_OPTIONS ||
                this.SPOKEN_LANGUAGE_OPTIONS != null &&
                input.SPOKEN_LANGUAGE_OPTIONS != null &&
                this.SPOKEN_LANGUAGE_OPTIONS.SequenceEqual(input.SPOKEN_LANGUAGE_OPTIONS)
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
            if (this.SPOKEN_LANGUAGE_OPTIONS != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.SPOKEN_LANGUAGE_OPTIONS);
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
