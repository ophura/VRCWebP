
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// RespondGroupJoinRequest
/// </summary>
[DataContract(Name = "RespondGroupJoinRequest")]
public partial class RespondGroupJoinRequest : IEquatable<RespondGroupJoinRequest>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets Action
    /// </summary>
    [DataMember(Name = "action", IsRequired = true, EmitDefaultValue = true)]
    public GroupJoinRequestAction Action { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="RespondGroupJoinRequest" /> class.
    /// </summary>

    protected RespondGroupJoinRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="RespondGroupJoinRequest" /> class.
    /// </summary>
    /// <param name="action">action (required).</param>
    /// <param name="block">Whether to block the user from requesting again.</param>
    public RespondGroupJoinRequest(GroupJoinRequestAction action = default, bool block = default)
    {
        this.Action = action;
        this.Block = block;
    }

    /// <summary>
    /// Whether to block the user from requesting again
    /// </summary>
    /// <value>Whether to block the user from requesting again</value>
    [DataMember(Name = "block", EmitDefaultValue = true)]
    public bool Block { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class RespondGroupJoinRequest {\n");
        sb.Append("  Action: ").Append(Action).Append('\n');
        sb.Append("  Block: ").Append(Block).Append('\n');
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
        return this.Equals(input as RespondGroupJoinRequest);
    }

    /// <summary>
    /// Returns true if RespondGroupJoinRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of RespondGroupJoinRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(RespondGroupJoinRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Action == input.Action ||
                this.Action.Equals(input.Action)
            ) &&
            (
                this.Block == input.Block ||
                this.Block.Equals(input.Block)
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
            hashCode = (hashCode * 59) + this.Action.GetHashCode();
            hashCode = (hashCode * 59) + this.Block.GetHashCode();
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
