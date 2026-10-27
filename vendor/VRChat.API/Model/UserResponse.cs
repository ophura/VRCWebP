
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// UserResponse
/// </summary>

[DataContract(Name = "UserResponse")]
public partial class UserResponse : AbstractOpenAPISchema, IEquatable<UserResponse>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserResponse" /> class
    /// with the <see cref="CurrentUser" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of CurrentUser.</param>
    public UserResponse(CurrentUser actualInstance)
    {
        IsNullable = false;
        SchemaType= "anyOf";
        ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserResponse" /> class
    /// with the <see cref="User" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of User.</param>
    public UserResponse(User actualInstance)
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
            if (value is CurrentUser)
            {
                _actualInstance = value;
            }
            else if (value is User)
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
    /// Converts to the <c>CurrentUser</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator CurrentUser(UserResponse value) => (CurrentUser)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>User</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator User(UserResponse value) => (User)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class UserResponse {\n");
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
        return Equals(input as UserResponse);
    }

    /// <summary>
    /// Returns true if UserResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of UserResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UserResponse input)
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
