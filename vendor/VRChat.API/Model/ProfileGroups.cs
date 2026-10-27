
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// ProfileGroups
/// </summary>
[DataContract(Name = "ProfileGroups")]
public partial class ProfileGroups : IEquatable<ProfileGroups>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileGroups" /> class.
    /// </summary>
    /// <param name="count">count.</param>
    /// <param name="list">list.</param>
    public ProfileGroups(int count = default, List<ProfileGroup> list = default)
    {
        this.Count = count;
        this.List = list;
    }

    /// <summary>
    /// Gets or Sets Count
    /// </summary>
    [DataMember(Name = "count", EmitDefaultValue = false)]
    public int Count { get; set; }

    /// <summary>
    /// Gets or Sets List
    /// </summary>
    [DataMember(Name = "list", EmitDefaultValue = false)]
    public List<ProfileGroup> List { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class ProfileGroups {\n");
        sb.Append("  Count: ").Append(Count).Append('\n');
        sb.Append("  List: ").Append(List).Append('\n');
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
        return this.Equals(input as ProfileGroups);
    }

    /// <summary>
    /// Returns true if ProfileGroups instances are equal
    /// </summary>
    /// <param name="input">Instance of ProfileGroups to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(ProfileGroups input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Count == input.Count ||
                this.Count.Equals(input.Count)
            ) &&
            (
                this.List == input.List ||
                this.List != null &&
                input.List != null &&
                this.List.SequenceEqual(input.List)
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
            hashCode = (hashCode * 59) + this.Count.GetHashCode();
            if (this.List != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.List);
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
