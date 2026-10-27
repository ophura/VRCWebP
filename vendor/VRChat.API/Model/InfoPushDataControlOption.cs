
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InfoPushDataControlOption
/// </summary>
[DataContract(Name = "InfoPushDataControlOption")]
public partial class InfoPushDataControlOption : IEquatable<InfoPushDataControlOption>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InfoPushDataControlOption" /> class.
    /// </summary>

    protected InfoPushDataControlOption() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InfoPushDataControlOption" /> class.
    /// </summary>
    /// <param name="id">id (required).</param>
    /// <param name="label">label (required).</param>
    /// <param name="varParams">varParams (required).</param>
    public InfoPushDataControlOption(string id = default, LocalizedString label = default, Dictionary<string, object> varParams = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Id = id;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Label = label;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Params = varParams;
    }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets Label
    /// </summary>
    [DataMember(Name = "label", IsRequired = true, EmitDefaultValue = true)]
    public LocalizedString Label { get; set; }

    /// <summary>
    /// Gets or Sets Params
    /// </summary>
    [DataMember(Name = "params", IsRequired = true, EmitDefaultValue = true)]
    public Dictionary<string, object> Params { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InfoPushDataControlOption {\n");
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  Label: ").Append(Label).Append('\n');
        sb.Append("  Params: ").Append(Params).Append('\n');
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
        return this.Equals(input as InfoPushDataControlOption);
    }

    /// <summary>
    /// Returns true if InfoPushDataControlOption instances are equal
    /// </summary>
    /// <param name="input">Instance of InfoPushDataControlOption to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InfoPushDataControlOption input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Id == input.Id ||
                (this.Id != null &&
                this.Id.Equals(input.Id))
            ) &&
            (
                this.Label == input.Label ||
                (this.Label != null &&
                this.Label.Equals(input.Label))
            ) &&
            (
                this.Params == input.Params ||
                this.Params != null &&
                input.Params != null &&
                this.Params.SequenceEqual(input.Params)
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
            if (this.Id != null)
            {
                hashCode = (hashCode * 59) + this.Id.GetHashCode();
            }
            if (this.Label != null)
            {
                hashCode = (hashCode * 59) + this.Label.GetHashCode();
            }
            if (this.Params != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Params);
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
