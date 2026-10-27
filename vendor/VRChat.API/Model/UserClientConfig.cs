
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// UserClientConfig
/// </summary>
[DataContract(Name = "UserClientConfig")]
public partial class UserClientConfig : IEquatable<UserClientConfig>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserClientConfig" /> class.
    /// </summary>

    protected UserClientConfig() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="UserClientConfig" /> class.
    /// </summary>
    /// <param name="accessReduceDecorAnim">accessReduceDecorAnim (required).</param>
    /// <param name="configString">configString (required).</param>
    public UserClientConfig(bool accessReduceDecorAnim = default, string configString = default)
    {
        this.AccessReduceDecorAnim = accessReduceDecorAnim;
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.ConfigString = configString;
    }

    /// <summary>
    /// Gets or Sets AccessReduceDecorAnim
    /// </summary>
    [DataMember(Name = "accessReduceDecorAnim", IsRequired = true, EmitDefaultValue = true)]
    public bool AccessReduceDecorAnim { get; set; }

    /// <summary>
    /// Gets or Sets ConfigString
    /// </summary>
    [DataMember(Name = "configString", IsRequired = true, EmitDefaultValue = true)]
    public string ConfigString { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class UserClientConfig {\n");
        sb.Append("  AccessReduceDecorAnim: ").Append(AccessReduceDecorAnim).Append('\n');
        sb.Append("  ConfigString: ").Append(ConfigString).Append('\n');
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
        return this.Equals(input as UserClientConfig);
    }

    /// <summary>
    /// Returns true if UserClientConfig instances are equal
    /// </summary>
    /// <param name="input">Instance of UserClientConfig to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UserClientConfig input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.AccessReduceDecorAnim == input.AccessReduceDecorAnim ||
                this.AccessReduceDecorAnim.Equals(input.AccessReduceDecorAnim)
            ) &&
            (
                this.ConfigString == input.ConfigString ||
                (this.ConfigString != null &&
                this.ConfigString.Equals(input.ConfigString))
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
            hashCode = (hashCode * 59) + this.AccessReduceDecorAnim.GetHashCode();
            if (this.ConfigString != null)
            {
                hashCode = (hashCode * 59) + this.ConfigString.GetHashCode();
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
