
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WebsocketNotificationV2Delete
/// </summary>
[DataContract(Name = "WebsocketNotificationV2Delete")]
public partial class WebsocketNotificationV2Delete : IEquatable<WebsocketNotificationV2Delete>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotificationV2Delete" /> class.
    /// </summary>

    protected WebsocketNotificationV2Delete() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotificationV2Delete" /> class.
    /// </summary>
    /// <param name="id">id (required).</param>
    /// <param name="varVersion">varVersion (required).</param>
    public WebsocketNotificationV2Delete(string id = default, int varVersion = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Id = id;
        this.VarVersion = varVersion;
    }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets VarVersion
    /// </summary>
    [DataMember(Name = "version", IsRequired = true, EmitDefaultValue = true)]
    public int VarVersion { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class WebsocketNotificationV2Delete {\n");
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  VarVersion: ").Append(VarVersion).Append('\n');
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
        return this.Equals(input as WebsocketNotificationV2Delete);
    }

    /// <summary>
    /// Returns true if WebsocketNotificationV2Delete instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketNotificationV2Delete to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketNotificationV2Delete input)
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
                this.VarVersion == input.VarVersion ||
                this.VarVersion.Equals(input.VarVersion)
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
            hashCode = (hashCode * 59) + this.VarVersion.GetHashCode();
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
