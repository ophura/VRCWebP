
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// A group audit log entry. The shape of &#x60;data&#x60; depends on &#x60;eventType&#x60;.
/// </summary>

[DataContract(Name = "GroupAuditLog")]
public partial class GroupAuditLog : AbstractOpenAPISchema, IEquatable<GroupAuditLog>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupAnnouncement" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupAnnouncement.</param>
    public GroupAuditLog(GroupAuditLogGroupAnnouncement actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupCalendarEventCreate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupCalendarEventCreate.</param>
    public GroupAuditLog(GroupAuditLogGroupCalendarEventCreate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupCalendarEventDelete" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupCalendarEventDelete.</param>
    public GroupAuditLog(GroupAuditLogGroupCalendarEventDelete actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupCreate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupCreate.</param>
    public GroupAuditLog(GroupAuditLogGroupCreate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupGalleryCreate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupGalleryCreate.</param>
    public GroupAuditLog(GroupAuditLogGroupGalleryCreate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupGalleryDelete" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupGalleryDelete.</param>
    public GroupAuditLog(GroupAuditLogGroupGalleryDelete actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupGalleryUpdate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupGalleryUpdate.</param>
    public GroupAuditLog(GroupAuditLogGroupGalleryUpdate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupInstanceAnnouncement" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupInstanceAnnouncement.</param>
    public GroupAuditLog(GroupAuditLogGroupInstanceAnnouncement actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupInstanceClose" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupInstanceClose.</param>
    public GroupAuditLog(GroupAuditLogGroupInstanceClose actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupInstanceCreate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupInstanceCreate.</param>
    public GroupAuditLog(GroupAuditLogGroupInstanceCreate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupInstanceKick" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupInstanceKick.</param>
    public GroupAuditLog(GroupAuditLogGroupInstanceKick actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupInstanceWarn" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupInstanceWarn.</param>
    public GroupAuditLog(GroupAuditLogGroupInstanceWarn actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupInviteCancel" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupInviteCancel.</param>
    public GroupAuditLog(GroupAuditLogGroupInviteCancel actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupInviteCreate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupInviteCreate.</param>
    public GroupAuditLog(GroupAuditLogGroupInviteCreate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupMemberJoin" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupMemberJoin.</param>
    public GroupAuditLog(GroupAuditLogGroupMemberJoin actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupMemberLeave" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupMemberLeave.</param>
    public GroupAuditLog(GroupAuditLogGroupMemberLeave actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupMemberRemove" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupMemberRemove.</param>
    public GroupAuditLog(GroupAuditLogGroupMemberRemove actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupMemberRoleAssign" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupMemberRoleAssign.</param>
    public GroupAuditLog(GroupAuditLogGroupMemberRoleAssign actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupMemberRoleUnassign" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupMemberRoleUnassign.</param>
    public GroupAuditLog(GroupAuditLogGroupMemberRoleUnassign actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupMemberUserUpdate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupMemberUserUpdate.</param>
    public GroupAuditLog(GroupAuditLogGroupMemberUserUpdate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupPostCreate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupPostCreate.</param>
    public GroupAuditLog(GroupAuditLogGroupPostCreate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupPostDelete" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupPostDelete.</param>
    public GroupAuditLog(GroupAuditLogGroupPostDelete actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupPostUpdate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupPostUpdate.</param>
    public GroupAuditLog(GroupAuditLogGroupPostUpdate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupRequestBlock" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupRequestBlock.</param>
    public GroupAuditLog(GroupAuditLogGroupRequestBlock actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupRequestCreate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupRequestCreate.</param>
    public GroupAuditLog(GroupAuditLogGroupRequestCreate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupRequestReject" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupRequestReject.</param>
    public GroupAuditLog(GroupAuditLogGroupRequestReject actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupRequestWithdraw" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupRequestWithdraw.</param>
    public GroupAuditLog(GroupAuditLogGroupRequestWithdraw actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupRoleCreate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupRoleCreate.</param>
    public GroupAuditLog(GroupAuditLogGroupRoleCreate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupRoleDelete" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupRoleDelete.</param>
    public GroupAuditLog(GroupAuditLogGroupRoleDelete actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupRoleUpdate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupRoleUpdate.</param>
    public GroupAuditLog(GroupAuditLogGroupRoleUpdate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupUpdate" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupUpdate.</param>
    public GroupAuditLog(GroupAuditLogGroupUpdate actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupUserBan" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupUserBan.</param>
    public GroupAuditLog(GroupAuditLogGroupUserBan actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogGroupUserUnban" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogGroupUserUnban.</param>
    public GroupAuditLog(GroupAuditLogGroupUserUnban actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupAuditLog" /> class
    /// with the <see cref="GroupAuditLogUnknown" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of GroupAuditLogUnknown.</param>
    public GroupAuditLog(GroupAuditLogUnknown actualInstance)
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
            if (value is GroupAuditLogGroupAnnouncement || value is GroupAuditLogGroupAnnouncement)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupCalendarEventCreate || value is GroupAuditLogGroupCalendarEventCreate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupCalendarEventDelete || value is GroupAuditLogGroupCalendarEventDelete)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupCreate || value is GroupAuditLogGroupCreate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupGalleryCreate || value is GroupAuditLogGroupGalleryCreate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupGalleryDelete || value is GroupAuditLogGroupGalleryDelete)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupGalleryUpdate || value is GroupAuditLogGroupGalleryUpdate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupInstanceAnnouncement || value is GroupAuditLogGroupInstanceAnnouncement)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupInstanceClose || value is GroupAuditLogGroupInstanceClose)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupInstanceCreate || value is GroupAuditLogGroupInstanceCreate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupInstanceKick || value is GroupAuditLogGroupInstanceKick)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupInstanceWarn || value is GroupAuditLogGroupInstanceWarn)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupInviteCancel || value is GroupAuditLogGroupInviteCancel)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupInviteCreate || value is GroupAuditLogGroupInviteCreate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupMemberJoin || value is GroupAuditLogGroupMemberJoin)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupMemberLeave || value is GroupAuditLogGroupMemberLeave)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupMemberRemove || value is GroupAuditLogGroupMemberRemove)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupMemberRoleAssign || value is GroupAuditLogGroupMemberRoleAssign)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupMemberRoleUnassign || value is GroupAuditLogGroupMemberRoleUnassign)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupMemberUserUpdate || value is GroupAuditLogGroupMemberUserUpdate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupPostCreate || value is GroupAuditLogGroupPostCreate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupPostDelete || value is GroupAuditLogGroupPostDelete)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupPostUpdate || value is GroupAuditLogGroupPostUpdate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupRequestBlock || value is GroupAuditLogGroupRequestBlock)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupRequestCreate || value is GroupAuditLogGroupRequestCreate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupRequestReject || value is GroupAuditLogGroupRequestReject)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupRequestWithdraw || value is GroupAuditLogGroupRequestWithdraw)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupRoleCreate || value is GroupAuditLogGroupRoleCreate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupRoleDelete || value is GroupAuditLogGroupRoleDelete)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupRoleUpdate || value is GroupAuditLogGroupRoleUpdate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupUpdate || value is GroupAuditLogGroupUpdate)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupUserBan || value is GroupAuditLogGroupUserBan)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogGroupUserUnban || value is GroupAuditLogGroupUserUnban)
            {
                this._actualInstance = value;
            }
            else if (value is GroupAuditLogUnknown || value is GroupAuditLogUnknown)
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
    /// Converts to the <c>GroupAuditLogGroupAnnouncement</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupAnnouncement(GroupAuditLog value) => (GroupAuditLogGroupAnnouncement)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupCalendarEventCreate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupCalendarEventCreate(GroupAuditLog value) => (GroupAuditLogGroupCalendarEventCreate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupCalendarEventDelete</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupCalendarEventDelete(GroupAuditLog value) => (GroupAuditLogGroupCalendarEventDelete)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupCreate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupCreate(GroupAuditLog value) => (GroupAuditLogGroupCreate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupGalleryCreate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupGalleryCreate(GroupAuditLog value) => (GroupAuditLogGroupGalleryCreate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupGalleryDelete</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupGalleryDelete(GroupAuditLog value) => (GroupAuditLogGroupGalleryDelete)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupGalleryUpdate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupGalleryUpdate(GroupAuditLog value) => (GroupAuditLogGroupGalleryUpdate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupInstanceAnnouncement</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupInstanceAnnouncement(GroupAuditLog value) => (GroupAuditLogGroupInstanceAnnouncement)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupInstanceClose</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupInstanceClose(GroupAuditLog value) => (GroupAuditLogGroupInstanceClose)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupInstanceCreate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupInstanceCreate(GroupAuditLog value) => (GroupAuditLogGroupInstanceCreate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupInstanceKick</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupInstanceKick(GroupAuditLog value) => (GroupAuditLogGroupInstanceKick)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupInstanceWarn</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupInstanceWarn(GroupAuditLog value) => (GroupAuditLogGroupInstanceWarn)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupInviteCancel</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupInviteCancel(GroupAuditLog value) => (GroupAuditLogGroupInviteCancel)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupInviteCreate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupInviteCreate(GroupAuditLog value) => (GroupAuditLogGroupInviteCreate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupMemberJoin</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupMemberJoin(GroupAuditLog value) => (GroupAuditLogGroupMemberJoin)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupMemberLeave</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupMemberLeave(GroupAuditLog value) => (GroupAuditLogGroupMemberLeave)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupMemberRemove</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupMemberRemove(GroupAuditLog value) => (GroupAuditLogGroupMemberRemove)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupMemberRoleAssign</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupMemberRoleAssign(GroupAuditLog value) => (GroupAuditLogGroupMemberRoleAssign)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupMemberRoleUnassign</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupMemberRoleUnassign(GroupAuditLog value) => (GroupAuditLogGroupMemberRoleUnassign)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupMemberUserUpdate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupMemberUserUpdate(GroupAuditLog value) => (GroupAuditLogGroupMemberUserUpdate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupPostCreate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupPostCreate(GroupAuditLog value) => (GroupAuditLogGroupPostCreate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupPostDelete</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupPostDelete(GroupAuditLog value) => (GroupAuditLogGroupPostDelete)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupPostUpdate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupPostUpdate(GroupAuditLog value) => (GroupAuditLogGroupPostUpdate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupRequestBlock</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupRequestBlock(GroupAuditLog value) => (GroupAuditLogGroupRequestBlock)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupRequestCreate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupRequestCreate(GroupAuditLog value) => (GroupAuditLogGroupRequestCreate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupRequestReject</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupRequestReject(GroupAuditLog value) => (GroupAuditLogGroupRequestReject)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupRequestWithdraw</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupRequestWithdraw(GroupAuditLog value) => (GroupAuditLogGroupRequestWithdraw)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupRoleCreate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupRoleCreate(GroupAuditLog value) => (GroupAuditLogGroupRoleCreate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupRoleDelete</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupRoleDelete(GroupAuditLog value) => (GroupAuditLogGroupRoleDelete)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupRoleUpdate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupRoleUpdate(GroupAuditLog value) => (GroupAuditLogGroupRoleUpdate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupUpdate</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupUpdate(GroupAuditLog value) => (GroupAuditLogGroupUpdate)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupUserBan</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupUserBan(GroupAuditLog value) => (GroupAuditLogGroupUserBan)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogGroupUserUnban</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogGroupUserUnban(GroupAuditLog value) => (GroupAuditLogGroupUserUnban)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>GroupAuditLogUnknown</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator GroupAuditLogUnknown(GroupAuditLog value) => (GroupAuditLogUnknown)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class GroupAuditLog {\n");
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
        return this.Equals(input as GroupAuditLog);
    }

    /// <summary>
    /// Returns true if GroupAuditLog instances are equal
    /// </summary>
    /// <param name="input">Instance of GroupAuditLog to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GroupAuditLog input)
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
