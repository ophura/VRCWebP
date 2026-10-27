using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// AccountDeletionLog
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AccountDeletionLog" /> class.
/// </remarks>
/// <param name="dateTime">Date and time of the deletion request..</param>
/// <param name="deletionScheduled">When the deletion is scheduled to happen, standard is 14 days after the request..</param>
/// <param name="message">Typically \&quot;Deletion requested\&quot; or \&quot;Deletion canceled\&quot;. Other messages like \&quot;Deletion completed\&quot; may exist, but are these are not possible to see as a regular user. (default to &quot;Deletion requested&quot;).</param>
[DataContract(Name = "AccountDeletionLog")]
public partial class AccountDeletionLog(DateTime dateTime = default, DateTime? deletionScheduled = default, string message = @"Deletion requested") : IEquatable<AccountDeletionLog>, IValidatableObject
{
    /// <summary>
    /// Date and time of the deletion request.
    /// </summary>
    /// <value>Date and time of the deletion request.</value>
    [DataMember(Name = "dateTime", EmitDefaultValue = false)]
    public DateTime DateTime { get; set; } = dateTime;

    /// <summary>
    /// When the deletion is scheduled to happen, standard is 14 days after the request.
    /// </summary>
    /// <value>When the deletion is scheduled to happen, standard is 14 days after the request.</value>
    [DataMember(Name = "deletionScheduled", EmitDefaultValue = true)]
    public DateTime? DeletionScheduled { get; set; } = deletionScheduled;

    /// <summary>
    /// Typically \&quot;Deletion requested\&quot; or \&quot;Deletion canceled\&quot;. Other messages like \&quot;Deletion completed\&quot; may exist, but are these are not possible to see as a regular user.
    /// </summary>
    /// <value>Typically \&quot;Deletion requested\&quot; or \&quot;Deletion canceled\&quot;. Other messages like \&quot;Deletion completed\&quot; may exist, but are these are not possible to see as a regular user.</value>
    [DataMember(Name = "message", EmitDefaultValue = false)]
    public string Message { get; set; } = message ?? @"Deletion requested";

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder Builder = new();

        _ = Builder.Append("class AccountDeletionLog {\n");

        _ = Builder.Append("  DateTime: ").Append(DateTime).Append('\n');

        _ = Builder.Append("  DeletionScheduled: ").Append(DeletionScheduled).Append('\n');

        _ = Builder.Append("  Message: ").Append(Message).Append('\n');

        _ = Builder.Append("}\n");

        return Builder.ToString();
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
        return Equals(input as AccountDeletionLog);
    }

    /// <summary>
    /// Returns true if AccountDeletionLog instances are equal
    /// </summary>
    /// <param name="input">Instance of AccountDeletionLog to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(AccountDeletionLog input)
    {
        if (input == null)
        {
            return false;
        }

        return
            (
                DateTime == input.DateTime || DateTime.Equals(input.DateTime)
            ) &&
            (
                DeletionScheduled == input.DeletionScheduled || (DeletionScheduled != null && DeletionScheduled.Equals(input.DeletionScheduled))
            ) &&
            (
                Message == input.Message || (Message != null && Message.Equals(input.Message))
            );
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

            hashCode = (hashCode * 59) + DateTime.GetHashCode();

            if (DeletionScheduled != null)
            {
                hashCode = (hashCode * 59) + DeletionScheduled.GetHashCode();
            }

            if (Message != null)
            {
                hashCode = (hashCode * 59) + Message.GetHashCode();
            }

            return hashCode;
        }
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        yield break;
    }
}
