
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A notification. The shape of &#x60;details&#x60; depends on &#x60;type&#x60;.
/// </summary>

[DataContract(Name = "Notification")]
public partial class Notification : AbstractOpenAPISchema, IEquatable<Notification>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationBoop" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationBoop.</param>
    public Notification(NotificationBoop actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationFriendRequest" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationFriendRequest.</param>
    public Notification(NotificationFriendRequest actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationInvite" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationInvite.</param>
    public Notification(NotificationInvite actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationInviteResponse" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationInviteResponse.</param>
    public Notification(NotificationInviteResponse actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationMessage" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationMessage.</param>
    public Notification(NotificationMessage actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationRequestInvite" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationRequestInvite.</param>
    public Notification(NotificationRequestInvite actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationRequestInviteResponse" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationRequestInviteResponse.</param>
    public Notification(NotificationRequestInviteResponse actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationVoteToKick" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationVoteToKick.</param>
    public Notification(NotificationVoteToKick actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification" /> class
    /// with the <see cref="NotificationUnknown" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationUnknown.</param>
    public Notification(NotificationUnknown actualInstance)
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
            if (value is NotificationBoop || value is NotificationBoop)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationFriendRequest || value is NotificationFriendRequest)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationInvite || value is NotificationInvite)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationInviteResponse || value is NotificationInviteResponse)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationMessage || value is NotificationMessage)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationRequestInvite || value is NotificationRequestInvite)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationRequestInviteResponse || value is NotificationRequestInviteResponse)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationUnknown || value is NotificationUnknown)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationVoteToKick || value is NotificationVoteToKick)
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
    /// Converts to the <c>NotificationBoop</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationBoop(Notification value) => (NotificationBoop)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationFriendRequest</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationFriendRequest(Notification value) => (NotificationFriendRequest)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationInvite</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationInvite(Notification value) => (NotificationInvite)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationInviteResponse</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationInviteResponse(Notification value) => (NotificationInviteResponse)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationMessage</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationMessage(Notification value) => (NotificationMessage)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationRequestInvite</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationRequestInvite(Notification value) => (NotificationRequestInvite)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationRequestInviteResponse</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationRequestInviteResponse(Notification value) => (NotificationRequestInviteResponse)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationVoteToKick</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationVoteToKick(Notification value) => (NotificationVoteToKick)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationUnknown</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationUnknown(Notification value) => (NotificationUnknown)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Notification {\n");
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
        return this.Equals(input as Notification);
    }

    /// <summary>
    /// Returns true if Notification instances are equal
    /// </summary>
    /// <param name="input">Instance of Notification to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Notification input)
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
