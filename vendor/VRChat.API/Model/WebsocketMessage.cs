
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A message received over the websocket. The shape of &#x60;content&#x60; depends on &#x60;type&#x60;.
/// </summary>

[DataContract(Name = "WebsocketMessage")]
public partial class WebsocketMessage : AbstractOpenAPISchema, IEquatable<WebsocketMessage>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketClearNotification" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketClearNotification.</param>
    public WebsocketMessage(WebsocketClearNotification actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketContentRefreshEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketContentRefreshEncoded.</param>
    public WebsocketMessage(WebsocketContentRefreshEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketFriendActiveEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketFriendActiveEncoded.</param>
    public WebsocketMessage(WebsocketFriendActiveEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketFriendAddEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketFriendAddEncoded.</param>
    public WebsocketMessage(WebsocketFriendAddEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketFriendDeleteEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketFriendDeleteEncoded.</param>
    public WebsocketMessage(WebsocketFriendDeleteEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketFriendLocationEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketFriendLocationEncoded.</param>
    public WebsocketMessage(WebsocketFriendLocationEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketFriendOfflineEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketFriendOfflineEncoded.</param>
    public WebsocketMessage(WebsocketFriendOfflineEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketFriendOnlineEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketFriendOnlineEncoded.</param>
    public WebsocketMessage(WebsocketFriendOnlineEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketFriendUpdateEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketFriendUpdateEncoded.</param>
    public WebsocketMessage(WebsocketFriendUpdateEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketGroupJoinedEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketGroupJoinedEncoded.</param>
    public WebsocketMessage(WebsocketGroupJoinedEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketGroupLeftEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketGroupLeftEncoded.</param>
    public WebsocketMessage(WebsocketGroupLeftEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketGroupMemberUpdatedEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketGroupMemberUpdatedEncoded.</param>
    public WebsocketMessage(WebsocketGroupMemberUpdatedEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketGroupRoleUpdatedEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketGroupRoleUpdatedEncoded.</param>
    public WebsocketMessage(WebsocketGroupRoleUpdatedEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketHideNotification" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketHideNotification.</param>
    public WebsocketMessage(WebsocketHideNotification actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketInstanceQueueJoinedEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketInstanceQueueJoinedEncoded.</param>
    public WebsocketMessage(WebsocketInstanceQueueJoinedEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketInstanceQueueReadyEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketInstanceQueueReadyEncoded.</param>
    public WebsocketMessage(WebsocketInstanceQueueReadyEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketNotificationEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationEncoded.</param>
    public WebsocketMessage(WebsocketNotificationEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketNotificationV2Encoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationV2Encoded.</param>
    public WebsocketMessage(WebsocketNotificationV2Encoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketNotificationV2DeleteEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationV2DeleteEncoded.</param>
    public WebsocketMessage(WebsocketNotificationV2DeleteEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketNotificationV2UpdateEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketNotificationV2UpdateEncoded.</param>
    public WebsocketMessage(WebsocketNotificationV2UpdateEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketResponseNotificationEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketResponseNotificationEncoded.</param>
    public WebsocketMessage(WebsocketResponseNotificationEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketSeeNotification" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketSeeNotification.</param>
    public WebsocketMessage(WebsocketSeeNotification actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketUserBadgeAssignedEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketUserBadgeAssignedEncoded.</param>
    public WebsocketMessage(WebsocketUserBadgeAssignedEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketUserBadgeUnassignedEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketUserBadgeUnassignedEncoded.</param>
    public WebsocketMessage(WebsocketUserBadgeUnassignedEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketUserLocationEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketUserLocationEncoded.</param>
    public WebsocketMessage(WebsocketUserLocationEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketUserUpdateEncoded" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketUserUpdateEncoded.</param>
    public WebsocketMessage(WebsocketUserUpdateEncoded actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketMessage" /> class
    /// with the <see cref="WebsocketMessageUnknown" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of WebsocketMessageUnknown.</param>
    public WebsocketMessage(WebsocketMessageUnknown actualInstance)
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
            if (value is WebsocketClearNotification || value is WebsocketClearNotification)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketContentRefreshEncoded || value is WebsocketContentRefreshEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketFriendActiveEncoded || value is WebsocketFriendActiveEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketFriendAddEncoded || value is WebsocketFriendAddEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketFriendDeleteEncoded || value is WebsocketFriendDeleteEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketFriendLocationEncoded || value is WebsocketFriendLocationEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketFriendOfflineEncoded || value is WebsocketFriendOfflineEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketFriendOnlineEncoded || value is WebsocketFriendOnlineEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketFriendUpdateEncoded || value is WebsocketFriendUpdateEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketGroupJoinedEncoded || value is WebsocketGroupJoinedEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketGroupLeftEncoded || value is WebsocketGroupLeftEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketGroupMemberUpdatedEncoded || value is WebsocketGroupMemberUpdatedEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketGroupRoleUpdatedEncoded || value is WebsocketGroupRoleUpdatedEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketHideNotification || value is WebsocketHideNotification)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketInstanceQueueJoinedEncoded || value is WebsocketInstanceQueueJoinedEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketInstanceQueueReadyEncoded || value is WebsocketInstanceQueueReadyEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketMessageUnknown || value is WebsocketMessageUnknown)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationEncoded || value is WebsocketNotificationEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationV2DeleteEncoded || value is WebsocketNotificationV2DeleteEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationV2Encoded || value is WebsocketNotificationV2Encoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketNotificationV2UpdateEncoded || value is WebsocketNotificationV2UpdateEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketResponseNotificationEncoded || value is WebsocketResponseNotificationEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketSeeNotification || value is WebsocketSeeNotification)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketUserBadgeAssignedEncoded || value is WebsocketUserBadgeAssignedEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketUserBadgeUnassignedEncoded || value is WebsocketUserBadgeUnassignedEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketUserLocationEncoded || value is WebsocketUserLocationEncoded)
            {
                this._actualInstance = value;
            }
            else if (value is WebsocketUserUpdateEncoded || value is WebsocketUserUpdateEncoded)
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
    /// Converts to the <c>WebsocketClearNotification</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketClearNotification(WebsocketMessage value) => (WebsocketClearNotification)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketContentRefreshEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketContentRefreshEncoded(WebsocketMessage value) => (WebsocketContentRefreshEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketFriendActiveEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketFriendActiveEncoded(WebsocketMessage value) => (WebsocketFriendActiveEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketFriendAddEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketFriendAddEncoded(WebsocketMessage value) => (WebsocketFriendAddEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketFriendDeleteEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketFriendDeleteEncoded(WebsocketMessage value) => (WebsocketFriendDeleteEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketFriendLocationEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketFriendLocationEncoded(WebsocketMessage value) => (WebsocketFriendLocationEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketFriendOfflineEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketFriendOfflineEncoded(WebsocketMessage value) => (WebsocketFriendOfflineEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketFriendOnlineEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketFriendOnlineEncoded(WebsocketMessage value) => (WebsocketFriendOnlineEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketFriendUpdateEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketFriendUpdateEncoded(WebsocketMessage value) => (WebsocketFriendUpdateEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketGroupJoinedEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketGroupJoinedEncoded(WebsocketMessage value) => (WebsocketGroupJoinedEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketGroupLeftEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketGroupLeftEncoded(WebsocketMessage value) => (WebsocketGroupLeftEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketGroupMemberUpdatedEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketGroupMemberUpdatedEncoded(WebsocketMessage value) => (WebsocketGroupMemberUpdatedEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketGroupRoleUpdatedEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketGroupRoleUpdatedEncoded(WebsocketMessage value) => (WebsocketGroupRoleUpdatedEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketHideNotification</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketHideNotification(WebsocketMessage value) => (WebsocketHideNotification)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketInstanceQueueJoinedEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketInstanceQueueJoinedEncoded(WebsocketMessage value) => (WebsocketInstanceQueueJoinedEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketInstanceQueueReadyEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketInstanceQueueReadyEncoded(WebsocketMessage value) => (WebsocketInstanceQueueReadyEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationEncoded(WebsocketMessage value) => (WebsocketNotificationEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationV2Encoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationV2Encoded(WebsocketMessage value) => (WebsocketNotificationV2Encoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationV2DeleteEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationV2DeleteEncoded(WebsocketMessage value) => (WebsocketNotificationV2DeleteEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketNotificationV2UpdateEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketNotificationV2UpdateEncoded(WebsocketMessage value) => (WebsocketNotificationV2UpdateEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketResponseNotificationEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketResponseNotificationEncoded(WebsocketMessage value) => (WebsocketResponseNotificationEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketSeeNotification</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketSeeNotification(WebsocketMessage value) => (WebsocketSeeNotification)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketUserBadgeAssignedEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketUserBadgeAssignedEncoded(WebsocketMessage value) => (WebsocketUserBadgeAssignedEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketUserBadgeUnassignedEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketUserBadgeUnassignedEncoded(WebsocketMessage value) => (WebsocketUserBadgeUnassignedEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketUserLocationEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketUserLocationEncoded(WebsocketMessage value) => (WebsocketUserLocationEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketUserUpdateEncoded</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketUserUpdateEncoded(WebsocketMessage value) => (WebsocketUserUpdateEncoded)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>WebsocketMessageUnknown</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator WebsocketMessageUnknown(WebsocketMessage value) => (WebsocketMessageUnknown)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class WebsocketMessage {\n");
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
        return this.Equals(input as WebsocketMessage);
    }

    /// <summary>
    /// Returns true if WebsocketMessage instances are equal
    /// </summary>
    /// <param name="input">Instance of WebsocketMessage to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(WebsocketMessage input)
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
