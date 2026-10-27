
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InventorySpawn
/// </summary>
[DataContract(Name = "InventorySpawn")]
public partial class InventorySpawn : IEquatable<InventorySpawn>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventorySpawn" /> class.
    /// </summary>

    protected InventorySpawn() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InventorySpawn" /> class.
    /// </summary>
    /// <param name="token">token (required).</param>
    /// <param name="varVersion">varVersion (required).</param>
    public InventorySpawn(string token = default, int varVersion = default)
    {
        // Allow null values for required properties to handle unexpected API responses gracefully
        this.Token = token;
        this.VarVersion = varVersion;
    }

    /// <summary>
    /// Gets or Sets Token
    /// </summary>
    [DataMember(Name = "token", IsRequired = true, EmitDefaultValue = true)]
    public string Token { get; set; }

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
        sb.Append("class InventorySpawn {\n");
        sb.Append("  Token: ").Append(Token).Append('\n');
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
        return this.Equals(input as InventorySpawn);
    }

    /// <summary>
    /// Returns true if InventorySpawn instances are equal
    /// </summary>
    /// <param name="input">Instance of InventorySpawn to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InventorySpawn input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Token == input.Token ||
                (this.Token != null &&
                this.Token.Equals(input.Token))
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
            if (this.Token != null)
            {
                hashCode = (hashCode * 59) + this.Token.GetHashCode();
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
