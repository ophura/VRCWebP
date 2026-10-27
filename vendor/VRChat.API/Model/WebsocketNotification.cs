
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A notification delivered over the websocket. The shape of &#x60;details&#x60; depends on &#x60;type&#x60;.
/// </summary>

[DataContract(Name = "WebsocketNotification")]
public partial class WebsocketNotification : AbstractOpenAPISchema, IEquatable<WebsocketNotification>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotification" /> class
    /// with the <see cref="WebsocketNotificationDetailBoop" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationDetailBoop.</param>
    public WebsocketNotification(WebsocketNotificationDetailBoop actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotification" /> class
    /// with the <see cref="WebsocketNotificationDetailInvite" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationDetailInvite.</param>
    public WebsocketNotification(WebsocketNotificationDetailInvite actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotification" /> class
    /// with the <see cref="WebsocketNotificationDetailInviteResponse" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationDetailInviteResponse.</param>
    public WebsocketNotification(WebsocketNotificationDetailInviteResponse actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotification" /> class
    /// with the <see cref="WebsocketNotificationDetailRequestInvite" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationDetailRequestInvite.</param>
    public WebsocketNotification(WebsocketNotificationDetailRequestInvite actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotification" /> class
    /// with the <see cref="WebsocketNotificationDetailRequestInviteResponse" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationDetailRequestInviteResponse.</param>
    public WebsocketNotification(WebsocketNotificationDetailRequestInviteResponse actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotification" /> class
    /// with the <see cref="WebsocketNotificationDetailVoteToKick" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationDetailVoteToKick.</param>
    public WebsocketNotification(WebsocketNotificationDetailVoteToKick actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketNotification" /> class
    /// with the <see cref="WebsocketNotificationUnknown" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationUnknown.</param>
    public WebsocketNotification(WebsocketNotificationUnknown actualInstance)
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
            if (value is WebsocketNotificationDetailBoop || value is WebsocketNotificationDetailBoop)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationDetailInvite || value is WebsocketNotificationDetailInvite)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationDetailInviteResponse || value is WebsocketNotificationDetailInviteResponse)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationDetailRequestInvite || value is WebsocketNotificationDetailRequestInvite)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationDetailRequestInviteResponse || value is WebsocketNotificationDetailRequestInviteResponse)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationDetailVoteToKick || value is WebsocketNotificationDetailVoteToKick)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationUnknown || value is WebsocketNotificationUnknown)
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
    /// Converts to the <c>WebsocketNotificationDetailBoop</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationDetailBoop(WebsocketNotification value) => (WebsocketNotificationDetailBoop)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationDetailInvite</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationDetailInvite(WebsocketNotification value) => (WebsocketNotificationDetailInvite)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationDetailInviteResponse</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationDetailInviteResponse(WebsocketNotification value) => (WebsocketNotificationDetailInviteResponse)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationDetailRequestInvite</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationDetailRequestInvite(WebsocketNotification value) => (WebsocketNotificationDetailRequestInvite)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationDetailRequestInviteResponse</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationDetailRequestInviteResponse(WebsocketNotification value) => (WebsocketNotificationDetailRequestInviteResponse)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationDetailVoteToKick</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationDetailVoteToKick(WebsocketNotification value) => (WebsocketNotificationDetailVoteToKick)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationUnknown</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationUnknown(WebsocketNotification value) => (WebsocketNotificationUnknown)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class WebsocketNotification {\n");
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
        return this.Equals(input as WebsocketNotification);
    }

    /// <summary>
    /// Returns true if WebsocketNotification instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketNotification to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketNotification input)
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
