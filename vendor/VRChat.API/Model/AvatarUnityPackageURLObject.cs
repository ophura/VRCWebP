
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// AvatarUnityPackageURLObject
/// </summary>
[DataContract(Name = "AvatarUnityPackageURLObject")]
public partial class AvatarUnityPackageURLObject : IEquatable<AvatarUnityPackageURLObject>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvatarUnityPackageURLObject" /> class.
    /// </summary>
    /// <param name="unityPackageUrl">unityPackageUrl.</param>
    public AvatarUnityPackageURLObject(string unityPackageUrl = default)
    {
        this.UnityPackageUrl = unityPackageUrl;
    }

    /// <summary>
    /// Gets or Sets UnityPackageUrl
    /// </summary>
    [DataMember(Name = "unityPackageUrl", EmitDefaultValue = false)]
    public string UnityPackageUrl { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AvatarUnityPackageURLObject {\n");
        sb.Append("  UnityPackageUrl: ").Append(UnityPackageUrl).Append('\n');
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
        return this.Equals(input as AvatarUnityPackageURLObject);
    }

    /// <summary>
    /// Returns true if AvatarUnityPackageURLObject instances are equal
    /// </summary>
    /// <param name="input">Instance of AvatarUnityPackageURLObject to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(AvatarUnityPackageURLObject input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.UnityPackageUrl == input.UnityPackageUrl ||
                (this.UnityPackageUrl != null &&
                this.UnityPackageUrl.Equals(input.UnityPackageUrl))
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
            if (this.UnityPackageUrl != null)
            {
                hashCode = (hashCode * 59) + this.UnityPackageUrl.GetHashCode();
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
