
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// ShareInventoryItemDirectRequest
/// </summary>
[DataContract(Name = "ShareInventoryItemDirectRequest")]
public partial class ShareInventoryItemDirectRequest : IEquatable<ShareInventoryItemDirectRequest>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShareInventoryItemDirectRequest" /> class.
    /// </summary>

    protected ShareInventoryItemDirectRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ShareInventoryItemDirectRequest" /> class.
    /// </summary>
    /// <param name="itemId">itemId (required).</param>
    /// <param name="users">users (required).</param>
    public ShareInventoryItemDirectRequest(string itemId = default, List<string> users = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.ItemId = itemId;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Users = users;
    }

    /// <summary>
    /// Gets or Sets ItemId
    /// </summary>
    [DataMember(Name = "itemId", IsRequired = true, EmitDefaultValue = true)]
    public string ItemId { get; set; }

    /// <summary>
    /// Gets or Sets Users
    /// </summary>
    [DataMember(Name = "users", IsRequired = true, EmitDefaultValue = true)]
    public List<string> Users { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class ShareInventoryItemDirectRequest {\n");
        sb.Append("  ItemId: ").Append(ItemId).Append('\n');
        sb.Append("  Users: ").Append(Users).Append('\n');
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
        return this.Equals(input as ShareInventoryItemDirectRequest);
    }

    /// <summary>
    /// Returns true if ShareInventoryItemDirectRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of ShareInventoryItemDirectRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(ShareInventoryItemDirectRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.ItemId == input.ItemId ||
                (this.ItemId != null &&
                this.ItemId.Equals(input.ItemId))
            ) &&
            (
                this.Users == input.Users ||
                this.Users != null &&
                input.Users != null &&
                this.Users.SequenceEqual(input.Users)
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
            if (this.ItemId != null)
            {
                hashCode = (hashCode * 59) + this.ItemId.GetHashCode();
            }
            if (this.Users != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Users);
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
