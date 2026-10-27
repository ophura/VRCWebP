
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// GroupMemberSearchResponse
/// </summary>
[DataContract(Name = "GroupMemberSearchResponse")]
public partial class GroupMemberSearchResponse : IEquatable<GroupMemberSearchResponse>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupMemberSearchResponse" /> class.
    /// </summary>
    /// <param name="results">results.</param>
    /// <param name="total">Number of members returned.</param>
    public GroupMemberSearchResponse(List<GroupMember> results = default, int total = default)
    {
        this.Results = results;
        this.Total = total;
    }

    /// <summary>
    /// Gets or Sets Results
    /// </summary>
    [DataMember(Name = "results", EmitDefaultValue = false)]
    public List<GroupMember> Results { get; set; }

    /// <summary>
    /// Number of members returned
    /// </summary>
    /// <value>Number of members returned</value>
    [DataMember(Name = "total", EmitDefaultValue = false)]
    public int Total { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class GroupMemberSearchResponse {\n");
        sb.Append("  Results: ").Append(Results).Append('\n');
        sb.Append("  Total: ").Append(Total).Append('\n');
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
        return this.Equals(input as GroupMemberSearchResponse);
    }

    /// <summary>
    /// Returns true if GroupMemberSearchResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupMemberSearchResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupMemberSearchResponse input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Results == input.Results ||
                this.Results != null &&
                input.Results != null &&
                this.Results.SequenceEqual(input.Results)
            ) &&
            (
                this.Total == input.Total ||
                this.Total.Equals(input.Total)
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
            if (this.Results != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Results);
            }
            hashCode = (hashCode * 59) + this.Total.GetHashCode();
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
