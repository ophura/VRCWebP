
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// UserActivityTimestamp
/// </summary>

[DataContract(Name = "UserActivityTimestamp")]
public partial class UserActivityTimestamp : AbstractOpenAPISchema, IEquatable<UserActivityTimestamp>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserActivityTimestamp" /> class
    /// with the <see cref="DateTime" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of DateTime.</param>
    public UserActivityTimestamp(DateTime actualInstance)
    {
        IsNullable = false;
        SchemaType= "anyOf";
        ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserActivityTimestamp" /> class
    /// with the <see cref="string" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of string.</param>
    public UserActivityTimestamp(string actualInstance)
    {
        IsNullable = false;
        SchemaType= "anyOf";
        ActualInstance = actualInstance;
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
            if (value is DateTime)
            {
                _actualInstance = value;
            }
            else if (value is string)
            {
                _actualInstance = value;
            }
            else
            {
                // Allow setting unknown types to handle unexpected responses gracefully
                System.Diagnostics.Debug.WriteLine(string.Format("Warning: Setting ActualInstance to a type not in anyOf schema: {0}", value?.GetType()?.Name ?? "null"));
                _actualInstance = value;
            }
        }
    }


    /// <summary>
    /// Converts to the <c>DateTime</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator DateTime(UserActivityTimestamp value) => (DateTime)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>string</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator string(UserActivityTimestamp value) => (string)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class UserActivityTimestamp {\n");
        sb.Append("  ActualInstance: ").Append(ActualInstance).Append('\n');
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
        return Equals(input as UserActivityTimestamp);
    }

    /// <summary>
    /// Returns true if UserActivityTimestamp instances are equal
    /// </summary>
    /// <param name="input">Instance of UserActivityTimestamp to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UserActivityTimestamp input)
    {
        if (input == null)
            return false;

        return object.Equals(ActualInstance, input.ActualInstance);
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
            if (ActualInstance != null)
                hashCode = hashCode * 59 + ActualInstance.GetHashCode();
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
