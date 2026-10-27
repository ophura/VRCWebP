
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A notification. The shape of &#x60;data&#x60; depends on &#x60;type&#x60;.
/// </summary>

[DataContract(Name = "NotificationV2")]
public partial class NotificationV2 : AbstractOpenAPISchema, IEquatable<NotificationV2>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2" /> class
    /// with the <see cref="NotificationV2GroupAnnouncement" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationV2GroupAnnouncement.</param>
    public NotificationV2(NotificationV2GroupAnnouncement actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2" /> class
    /// with the <see cref="NotificationV2GroupInvite" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationV2GroupInvite.</param>
    public NotificationV2(NotificationV2GroupInvite actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2" /> class
    /// with the <see cref="NotificationV2Unknown" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationV2Unknown.</param>
    public NotificationV2(NotificationV2Unknown actualInstance)
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
            if (value is NotificationV2GroupAnnouncement || value is NotificationV2GroupAnnouncement)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationV2GroupInvite || value is NotificationV2GroupInvite)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationV2Unknown || value is NotificationV2Unknown)
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
    /// Converts to the <c>NotificationV2GroupAnnouncement</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationV2GroupAnnouncement(NotificationV2 value) => (NotificationV2GroupAnnouncement)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationV2GroupInvite</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationV2GroupInvite(NotificationV2 value) => (NotificationV2GroupInvite)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationV2Unknown</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationV2Unknown(NotificationV2 value) => (NotificationV2Unknown)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class NotificationV2 {\n");
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
        return this.Equals(input as NotificationV2);
    }

    /// <summary>
    /// Returns true if NotificationV2 instances are equal
    /// </summary>
    /// <param name="input">Instance of NotificationV2 to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(NotificationV2 input)
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
