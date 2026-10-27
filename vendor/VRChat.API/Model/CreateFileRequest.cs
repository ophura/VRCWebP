
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// CreateFileRequest
/// </summary>
[DataContract(Name = "CreateFileRequest")]
public partial class CreateFileRequest : IEquatable<CreateFileRequest>, IValidatableObject
{

    /// <summary>
    /// Gets or Sets MimeType
    /// </summary>
    [DataMember(Name = "mimeType", IsRequired = true, EmitDefaultValue = true)]
    public CreateFileRequestMIMEType MimeType { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateFileRequest" /> class.
    /// </summary>

    protected CreateFileRequest() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateFileRequest" /> class.
    /// </summary>
    /// <param name="extension">extension (required).</param>
    /// <param name="mimeType">mimeType (required).</param>
    /// <param name="name">name (required).</param>
    /// <param name="tags">tags.</param>
    public CreateFileRequest(string extension = default, CreateFileRequestMIMEType mimeType = default, string name = default, List<string> tags = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Extension = extension;
        this.MimeType = mimeType;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Name = name;
        this.Tags = tags;
    }

    /// <summary>
    /// Gets or Sets Extension
    /// </summary>
    [DataMember(Name = "extension", IsRequired = true, EmitDefaultValue = true)]
    public string Extension { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Tags
    /// </summary>
    [DataMember(Name = "tags", EmitDefaultValue = false)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class CreateFileRequest {\n");
        sb.Append("  Extension: ").Append(Extension).Append('\n');
        sb.Append("  MimeType: ").Append(MimeType).Append('\n');
        sb.Append("  Name: ").Append(Name).Append('\n');
        sb.Append("  Tags: ").Append(Tags).Append('\n');
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
        return this.Equals(input as CreateFileRequest);
    }

    /// <summary>
    /// Returns true if CreateFileRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of CreateFileRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(CreateFileRequest input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Extension == input.Extension ||
                (this.Extension != null &&
                this.Extension.Equals(input.Extension))
            ) &&
            (
                this.MimeType == input.MimeType ||
                this.MimeType.Equals(input.MimeType)
            ) &&
            (
                this.Name == input.Name ||
                (this.Name != null &&
                this.Name.Equals(input.Name))
            ) &&
            (
                this.Tags == input.Tags ||
                this.Tags != null &&
                input.Tags != null &&
                this.Tags.SequenceEqual(input.Tags)
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
            if (this.Extension != null)
            {
                hashCode = (hashCode * 59) + this.Extension.GetHashCode();
            }
            hashCode = (hashCode * 59) + this.MimeType.GetHashCode();
            if (this.Name != null)
            {
                hashCode = (hashCode * 59) + this.Name.GetHashCode();
            }
            if (this.Tags != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Tags);
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
        // Extension (string) minLength
        if (this.Extension != null && this.Extension.Length < 1)
        {
            yield return new ValidationResult("Invalid value for Extension, length must be greater than 1.", new [] { "Extension" });
        }

        // Name (string) minLength
        if (this.Name != null && this.Name.Length < 0)
        {
            yield return new ValidationResult("Invalid value for Name, length must be greater than 0.", new [] { "Name" });
        }

        yield break;
    }
}
