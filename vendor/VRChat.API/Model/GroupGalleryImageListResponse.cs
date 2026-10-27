
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// GroupGalleryImageListResponse
/// </summary>

[DataContract(Name = "GroupGalleryImageListResponse")]
public partial class GroupGalleryImageListResponse : AbstractOpenAPISchema, IEquatable<GroupGalleryImageListResponse>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupGalleryImageListResponse" /> class
    /// with the <see cref="List{GroupGalleryImage}" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of List&lt;GroupGalleryImage&gt;.</param>
    public GroupGalleryImageListResponse(List<GroupGalleryImage> actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupGalleryImageListResponse" /> class
    /// with the <see cref="GroupGalleryImageList" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupGalleryImageList.</param>
    public GroupGalleryImageListResponse(GroupGalleryImageList actualInstance)
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
            if (value is GroupGalleryImageList || value is GroupGalleryImageList)
            {
                this._actualInstance = value;
            }
            else if (value is List<GroupGalleryImage> || value is List<GroupGalleryImage>)
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
    /// Converts to the <c>List&lt;GroupGalleryImage&gt;</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator List<GroupGalleryImage>(GroupGalleryImageListResponse value) => (List<GroupGalleryImage>)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupGalleryImageList</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupGalleryImageList(GroupGalleryImageListResponse value) => (GroupGalleryImageList)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class GroupGalleryImageListResponse {\n");
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
        return this.Equals(input as GroupGalleryImageListResponse);
    }

    /// <summary>
    /// Returns true if GroupGalleryImageListResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupGalleryImageListResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupGalleryImageListResponse input)
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
