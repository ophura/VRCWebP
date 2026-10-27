
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A notification as its sender receives it back. The shape of &#x60;details&#x60; depends on &#x60;type&#x60;.
/// </summary>

[DataContract(Name = "SentNotification")]
public partial class SentNotification : AbstractOpenAPISchema, IEquatable<SentNotification>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationBoop" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationBoop.</param>
    public SentNotification(SentNotificationBoop actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationFriendRequest" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationFriendRequest.</param>
    public SentNotification(SentNotificationFriendRequest actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationInvite" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationInvite.</param>
    public SentNotification(SentNotificationInvite actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationInviteResponse" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationInviteResponse.</param>
    public SentNotification(SentNotificationInviteResponse actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationMessage" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationMessage.</param>
    public SentNotification(SentNotificationMessage actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationRequestInvite" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationRequestInvite.</param>
    public SentNotification(SentNotificationRequestInvite actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationRequestInviteResponse" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationRequestInviteResponse.</param>
    public SentNotification(SentNotificationRequestInviteResponse actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationVoteToKick" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationVoteToKick.</param>
    public SentNotification(SentNotificationVoteToKick actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentNotification" /> class
    /// with the <see cref="SentNotificationUnknown" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SentNotificationUnknown.</param>
    public SentNotification(SentNotificationUnknown actualInstance)
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
            if (value is SentNotificationBoop || value is SentNotificationBoop)
            {
                this._actualInstance = value;
            }
            else if (value is SentNotificationFriendRequest || value is SentNotificationFriendRequest)
            {
                this._actualInstance = value;
            }
            else if (value is SentNotificationInvite || value is SentNotificationInvite)
            {
                this._actualInstance = value;
            }
            else if (value is SentNotificationInviteResponse || value is SentNotificationInviteResponse)
            {
                this._actualInstance = value;
            }
            else if (value is SentNotificationMessage || value is SentNotificationMessage)
            {
                this._actualInstance = value;
            }
            else if (value is SentNotificationRequestInvite || value is SentNotificationRequestInvite)
            {
                this._actualInstance = value;
            }
            else if (value is SentNotificationRequestInviteResponse || value is SentNotificationRequestInviteResponse)
            {
                this._actualInstance = value;
            }
            else if (value is SentNotificationUnknown || value is SentNotificationUnknown)
            {
                this._actualInstance = value;
            }
            else if (value is SentNotificationVoteToKick || value is SentNotificationVoteToKick)
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
    /// Converts to the <c>SentNotificationBoop</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationBoop(SentNotification value) => (SentNotificationBoop)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>SentNotificationFriendRequest</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationFriendRequest(SentNotification value) => (SentNotificationFriendRequest)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>SentNotificationInvite</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationInvite(SentNotification value) => (SentNotificationInvite)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>SentNotificationInviteResponse</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationInviteResponse(SentNotification value) => (SentNotificationInviteResponse)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>SentNotificationMessage</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationMessage(SentNotification value) => (SentNotificationMessage)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>SentNotificationRequestInvite</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationRequestInvite(SentNotification value) => (SentNotificationRequestInvite)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>SentNotificationRequestInviteResponse</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationRequestInviteResponse(SentNotification value) => (SentNotificationRequestInviteResponse)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>SentNotificationVoteToKick</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationVoteToKick(SentNotification value) => (SentNotificationVoteToKick)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>SentNotificationUnknown</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator SentNotificationUnknown(SentNotification value) => (SentNotificationUnknown)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class SentNotification {\n");
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
        return this.Equals(input as SentNotification);
    }

    /// <summary>
    /// Returns true if SentNotification instances are equal
    /// </summary>
    /// <param name="input">Instance of SentNotification to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(SentNotification input)
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
