
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// WorldMetadataValue
/// </summary>

[DataContract(Name = "WorldMetadataValue")]
public partial class WorldMetadataValue : AbstractOpenAPISchema, IEquatable<WorldMetadataValue>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldMetadataValue" /> class
    /// with the <see cref="Dictionary{string, Object}" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of Dictionary&lt;string, Object&gt;.</param>
    public WorldMetadataValue(Dictionary<string, object> actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldMetadataValue" /> class
    /// with the <see cref="List{Object}" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of List&lt;Object&gt;.</param>
    public WorldMetadataValue(List<object> actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    private object _actualInstance;

    /// <summary>
    /// Gets or Sets ActualInstance
    /// </summary>
    public override object ActualInstance
    {
        get
        {
            return _actualInstance;
        }
        set
        {
            if (value is Dictionary<string, object> || value is Dictionary<string, object>)
            {
                this._actualInstance = value;
            }
            else if (value is List<object> || value is List<object>)
            {
                this._actualInstance = value;
            }
            else
            {
                // Allow setting unknown types to handle unexpected responses gracefully
                System.Diagnostics.Debug.WriteLine(string.Format("Warning: Setting ActualInstance to a type not in oneOf schema: {0}", value?.GetType()?.Name ?? "null"));
                this._actualInstance = value;
            }
        }
    }




    /// <summary>
    /// Converts to the <c>List&lt;Object&gt;</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator List<object>(WorldMetadataValue value) => (List<object>)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class WorldMetadataValue {\n");
        sb.Append("  ActualInstance: ").Append(this.ActualInstance).Append('\n');
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
        return this.Equals(input as WorldMetadataValue);
    }

    /// <summary>
    /// Returns true if WorldMetadataValue instances are equal
    /// </summary>
    /// <param name="input">Instance of WorldMetadataValue to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WorldMetadataValue input)
    {
        if (input == null)
            return false;

        return this.ActualInstance.Equals(input.ActualInstance);
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
            if (this.ActualInstance != null)
                hashCode = hashCode * 59 + this.ActualInstance.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        yield break;
    }
}
