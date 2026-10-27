
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A favorite alongside the object it points at. The object appears under a property named for the favorite&#39;s type.
/// </summary>

[DataContract(Name = "FavoriteGroupContentsItem")]
public partial class FavoriteGroupContentsItem : AbstractOpenAPISchema, IEquatable<FavoriteGroupContentsItem>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupContentsItem" /> class
    /// with the <see cref="FavoriteGroupContentsItemAvatar" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of FavoriteGroupContentsItemAvatar.</param>
    public FavoriteGroupContentsItem(FavoriteGroupContentsItemAvatar actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupContentsItem" /> class
    /// with the <see cref="FavoriteGroupContentsItemFriend" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of FavoriteGroupContentsItemFriend.</param>
    public FavoriteGroupContentsItem(FavoriteGroupContentsItemFriend actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupContentsItem" /> class
    /// with the <see cref="FavoriteGroupContentsItemVRCPlusWorld" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of FavoriteGroupContentsItemVRCPlusWorld.</param>
    public FavoriteGroupContentsItem(FavoriteGroupContentsItemVRCPlusWorld actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupContentsItem" /> class
    /// with the <see cref="FavoriteGroupContentsItemWorld" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of FavoriteGroupContentsItemWorld.</param>
    public FavoriteGroupContentsItem(FavoriteGroupContentsItemWorld actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoriteGroupContentsItem" /> class
    /// with the <see cref="FavoriteGroupContentsItemUnknown" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of FavoriteGroupContentsItemUnknown.</param>
    public FavoriteGroupContentsItem(FavoriteGroupContentsItemUnknown actualInstance)
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
            if (value is FavoriteGroupContentsItemAvatar || value is FavoriteGroupContentsItemAvatar)
            {
                this._actualInstance = value;
            }
            else if (value is FavoriteGroupContentsItemFriend || value is FavoriteGroupContentsItemFriend)
            {
                this._actualInstance = value;
            }
            else if (value is FavoriteGroupContentsItemUnknown || value is FavoriteGroupContentsItemUnknown)
            {
                this._actualInstance = value;
            }
            else if (value is FavoriteGroupContentsItemVRCPlusWorld || value is FavoriteGroupContentsItemVRCPlusWorld)
            {
                this._actualInstance = value;
            }
            else if (value is FavoriteGroupContentsItemWorld || value is FavoriteGroupContentsItemWorld)
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
    /// Converts to the <c>FavoriteGroupContentsItemAvatar</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator FavoriteGroupContentsItemAvatar(FavoriteGroupContentsItem value) => (FavoriteGroupContentsItemAvatar)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>FavoriteGroupContentsItemFriend</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator FavoriteGroupContentsItemFriend(FavoriteGroupContentsItem value) => (FavoriteGroupContentsItemFriend)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>FavoriteGroupContentsItemVRCPlusWorld</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator FavoriteGroupContentsItemVRCPlusWorld(FavoriteGroupContentsItem value) => (FavoriteGroupContentsItemVRCPlusWorld)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>FavoriteGroupContentsItemWorld</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator FavoriteGroupContentsItemWorld(FavoriteGroupContentsItem value) => (FavoriteGroupContentsItemWorld)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>FavoriteGroupContentsItemUnknown</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator FavoriteGroupContentsItemUnknown(FavoriteGroupContentsItem value) => (FavoriteGroupContentsItemUnknown)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class FavoriteGroupContentsItem {\n");
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
        return this.Equals(input as FavoriteGroupContentsItem);
    }

    /// <summary>
    /// Returns true if FavoriteGroupContentsItem instances are equal
    /// </summary>
    /// <param name="input">Instance of FavoriteGroupContentsItem to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(FavoriteGroupContentsItem input)
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
