
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// FavoriteGroupWorld
/// </summary>

[DataContract(Name = "FavoriteGroupWorld")]
public partial class FavoriteGroupWorld : AbstractOpenAPISchema, IEquatable<FavoriteGroupWorld>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupWorld" /> class
    /// with the <see cref="FavoritedWorld" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of FavoritedWorld.</param>
    public FavoriteGroupWorld(FavoritedWorld actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupWorld" /> class
    /// with the <see cref="UnavailableWorld" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of UnavailableWorld.</param>
    public FavoriteGroupWorld(UnavailableWorld actualInstance)
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
            if (value is FavoritedWorld || value is FavoritedWorld)
            {
                this._actualInstance = value;
            }
            else if (value is UnavailableWorld || value is UnavailableWorld)
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
    /// Converts to the <c>FavoritedWorld</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator FavoritedWorld(FavoriteGroupWorld value) => (FavoritedWorld)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>UnavailableWorld</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator UnavailableWorld(FavoriteGroupWorld value) => (UnavailableWorld)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class FavoriteGroupWorld {\n");
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
        return this.Equals(input as FavoriteGroupWorld);
    }

    /// <summary>
    /// Returns true if FavoriteGroupWorld instances are equal
    /// </summary>
    /// <param name="input">Instance of FavoriteGroupWorld to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(FavoriteGroupWorld input)
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
