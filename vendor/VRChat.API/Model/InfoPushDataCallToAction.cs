
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InfoPushDataCallToAction
/// </summary>
[DataContract(Name = "InfoPushDataCallToAction")]
public partial class InfoPushDataCallToAction : IEquatable<InfoPushDataCallToAction>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InfoPushDataCallToAction" /> class.
    /// </summary>
    /// <param name="text">text.</param>
    public InfoPushDataCallToAction(string text = default)
    {
        this.Text = text;
    }

    /// <summary>
    /// Gets or Sets Text
    /// </summary>
    [DataMember(Name = "text", EmitDefaultValue = false)]
    public string Text { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InfoPushDataCallToAction {\n");
        sb.Append("  Text: ").Append(Text).Append('\n');
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
        return this.Equals(input as InfoPushDataCallToAction);
    }

    /// <summary>
    /// Returns true if InfoPushDataCallToAction instances are equal
    /// </summary>
    /// <param name="input">Instance of InfoPushDataCallToAction to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InfoPushDataCallToAction input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Text == input.Text ||
                (this.Text != null &&
                this.Text.Equals(input.Text))
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
            if (this.Text != null)
            {
                hashCode = (hashCode * 59) + this.Text.GetHashCode();
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
