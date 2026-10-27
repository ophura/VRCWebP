
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// GroupPostsResponse
/// </summary>
[DataContract(Name = "GroupPostsResponse")]
public partial class GroupPostsResponse : IEquatable<GroupPostsResponse>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupPostsResponse" /> class.
    /// </summary>
    /// <param name="posts">posts.</param>
    /// <param name="total">total.</param>
    public GroupPostsResponse(List<GroupPost> posts = default, int total = default)
    {
        this.Posts = posts;
        this.Total = total;
    }

    /// <summary>
    /// Gets or Sets Posts
    /// </summary>
    [DataMember(Name = "posts", EmitDefaultValue = false)]
    public List<GroupPost> Posts { get; set; }

    /// <summary>
    /// Gets or Sets Total
    /// </summary>
    [DataMember(Name = "total", EmitDefaultValue = false)]
    public int Total { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class GroupPostsResponse {\n");
        sb.Append("  Posts: ").Append(Posts).Append('\n');
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
        return this.Equals(input as GroupPostsResponse);
    }

    /// <summary>
    /// Returns true if GroupPostsResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupPostsResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupPostsResponse input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Posts == input.Posts ||
                this.Posts != null &&
                input.Posts != null &&
                this.Posts.SequenceEqual(input.Posts)
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
            if (this.Posts != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Posts);
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
