
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// CreateGroupPostRequest
/// </summary>
[DataContract(Name = "CreateGroupPostRequest")]
public partial class CreateGroupPostRequest : IEquatable<CreateGroupPostRequest>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets Visibility
    /// </summary>
    [DataMember(Name = "visibility", IsRequired = true, EmitDefaultValue = true)]
    public GroupPostVisibility Visibility { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGroupPostRequest" /> class.
    /// </summary>

    protected CreateGroupPostRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGroupPostRequest" /> class.
    /// </summary>
    /// <param name="imageId">imageId.</param>
    /// <param name="roleIds">roleIds.</param>
    /// <param name="sendNotification">Send notification to group members. (required) (default to false).</param>
    /// <param name="text">Post text (required).</param>
    /// <param name="title">Post title (required).</param>
    /// <param name="visibility">visibility (required).</param>
    public CreateGroupPostRequest(string imageId = default, List<string> roleIds = default, bool sendNotification = false, string text = default, string title = default, GroupPostVisibility visibility = default)
    {
        this.SendNotification = sendNotification;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Text = text;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Title = title;
        this.Visibility = visibility;
        this.ImageId = imageId;
        this.RoleIds = roleIds;
    }

    /// <summary>
    /// Gets or Sets ImageId
    /// </summary>
    [DataMember(Name = "imageId", EmitDefaultValue = false)]
    public string ImageId { get; set; }

    /// <summary>
    /// Gets or Sets RoleIds
    /// </summary>
    [DataMember(Name = "roleIds", EmitDefaultValue = false)]
    public List<string> RoleIds { get; set; }

    /// <summary>
    /// Send notification to group members.
    /// </summary>
    /// <value>Send notification to group members.</value>
    [DataMember(Name = "sendNotification", IsRequired = true, EmitDefaultValue = true)]
    public bool SendNotification { get; set; }

    /// <summary>
    /// Post text
    /// </summary>
    /// <value>Post text</value>
    [DataMember(Name = "text", IsRequired = true, EmitDefaultValue = true)]
    public string Text { get; set; }

    /// <summary>
    /// Post title
    /// </summary>
    /// <value>Post title</value>
    [DataMember(Name = "title", IsRequired = true, EmitDefaultValue = true)]
    public string Title { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class CreateGroupPostRequest {\n");
        sb.Append("  ImageId: ").Append(ImageId).Append('\n');
        sb.Append("  RoleIds: ").Append(RoleIds).Append('\n');
        sb.Append("  SendNotification: ").Append(SendNotification).Append('\n');
        sb.Append("  Text: ").Append(Text).Append('\n');
        sb.Append("  Title: ").Append(Title).Append('\n');
        sb.Append("  Visibility: ").Append(Visibility).Append('\n');
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
        return this.Equals(input as CreateGroupPostRequest);
    }

    /// <summary>
    /// Returns true if CreateGroupPostRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of CreateGroupPostRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(CreateGroupPostRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.ImageId == input.ImageId ||
                (this.ImageId != null &&
                this.ImageId.Equals(input.ImageId))
            ) &&
            (
                this.RoleIds == input.RoleIds ||
                this.RoleIds != null &&
                input.RoleIds != null &&
                this.RoleIds.SequenceEqual(input.RoleIds)
            ) &&
            (
                this.SendNotification == input.SendNotification ||
                this.SendNotification.Equals(input.SendNotification)
            ) &&
            (
                this.Text == input.Text ||
                (this.Text != null &&
                this.Text.Equals(input.Text))
            ) &&
            (
                this.Title == input.Title ||
                (this.Title != null &&
                this.Title.Equals(input.Title))
            ) &&
            (
                this.Visibility == input.Visibility ||
                this.Visibility.Equals(input.Visibility)
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
            if (this.ImageId != null)
            {
                hashCode = (hashCode * 59) + this.ImageId.GetHashCode();
            }
            if (this.RoleIds != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.RoleIds);
            }
            hashCode = (hashCode * 59) + this.SendNotification.GetHashCode();
            if (this.Text != null)
            {
                hashCode = (hashCode * 59) + this.Text.GetHashCode();
            }
            if (this.Title != null)
            {
                hashCode = (hashCode * 59) + this.Title.GetHashCode();
            }
            hashCode = (hashCode * 59) + this.Visibility.GetHashCode();
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
        // Text (string) minLength
        if (this.Text != null && this.Text.Length < 1)
        {
            yield return new ValidationResult("Invalid value for Text, length must be greater than 1.", new [] { "Text" });
        }

        // Title (string) minLength
        if (this.Title != null && this.Title.Length < 1)
        {
            yield return new ValidationResult("Invalid value for Title, length must be greater than 1.", new [] { "Title" });
        }

        yield break;
    }
}
