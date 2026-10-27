
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// NotificationV2Data
/// </summary>

[DataContract(Name = "NotificationV2Data")]
public partial class NotificationV2Data : AbstractOpenAPISchema, IEquatable<NotificationV2Data>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2Data" /> class
    /// with the <see cref="object" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of Object.</param>
    public NotificationV2Data(object actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2Data" /> class
    /// with the <see cref="NotificationV2DataBadgeEarned" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationV2DataBadgeEarned.</param>
    public NotificationV2Data(NotificationV2DataBadgeEarned actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2Data" /> class
    /// with the <see cref="NotificationV2DataBoop" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationV2DataBoop.</param>
    public NotificationV2Data(NotificationV2DataBoop actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2Data" /> class
    /// with the <see cref="NotificationV2DataEventAnnouncement" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationV2DataEventAnnouncement.</param>
    public NotificationV2Data(NotificationV2DataEventAnnouncement actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2Data" /> class
    /// with the <see cref="NotificationV2DataGroupInformative" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationV2DataGroupInformative.</param>
    public NotificationV2Data(NotificationV2DataGroupInformative actualInstance)
    {
        this.IsNullable = false;
        this.SchemaType= "oneOf";
        this.ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationV2Data" /> class
    /// with the <see cref="NotificationV2DataGroupTransfer" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of NotificationV2DataGroupTransfer.</param>
    public NotificationV2Data(NotificationV2DataGroupTransfer actualInstance)
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
            if (value is NotificationV2DataBadgeEarned || value is NotificationV2DataBadgeEarned)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationV2DataBoop || value is NotificationV2DataBoop)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationV2DataEventAnnouncement || value is NotificationV2DataEventAnnouncement)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationV2DataGroupInformative || value is NotificationV2DataGroupInformative)
            {
                this._actualInstance = value;
            }
            else if (value is NotificationV2DataGroupTransfer || value is NotificationV2DataGroupTransfer)
            {
                this._actualInstance = value;
            }
            else if (value is object || value is object)
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
    /// Converts to the <c>NotificationV2DataBadgeEarned</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationV2DataBadgeEarned(NotificationV2Data value) => (NotificationV2DataBadgeEarned)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationV2DataBoop</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationV2DataBoop(NotificationV2Data value) => (NotificationV2DataBoop)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationV2DataEventAnnouncement</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationV2DataEventAnnouncement(NotificationV2Data value) => (NotificationV2DataEventAnnouncement)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationV2DataGroupInformative</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationV2DataGroupInformative(NotificationV2Data value) => (NotificationV2DataGroupInformative)value?.ActualInstance;



    /// <summary>
    /// Converts to the <c>NotificationV2DataGroupTransfer</c> this instance holds, throwing <see cref="InvalidCastException"/> when it holds another type.
    /// </summary>
    public static implicit operator NotificationV2DataGroupTransfer(NotificationV2Data value) => (NotificationV2DataGroupTransfer)value?.ActualInstance;


    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class NotificationV2Data {\n");
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
        return this.Equals(input as NotificationV2Data);
    }

    /// <summary>
    /// Returns true if NotificationV2Data instances are equal
    /// </summary>
    /// <param name="input">Instance of NotificationV2Data to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(NotificationV2Data input)
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
