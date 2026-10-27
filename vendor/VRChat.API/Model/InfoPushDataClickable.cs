
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// InfoPushDataClickable
/// </summary>
[DataContract(Name = "InfoPushDataClickable")]
public partial class InfoPushDataClickable : IEquatable<InfoPushDataClickable>, IValidatableObject
{
    /// <summary>
    /// Defines Command
    /// </summary>

    public enum CommandEnum
    {
        /// <summary>
        /// Enum CannedWorldSearch for value: CannedWorldSearch
        /// </summary>
        [EnumMember(Value = "CannedWorldSearch")]
        CannedWorldSearch = 1,

        /// <summary>
        /// Enum OpenAccountUpgrade for value: OpenAccountUpgrade
        /// </summary>
        [EnumMember(Value = "OpenAccountUpgrade")]
        OpenAccountUpgrade = 2,

        /// <summary>
        /// Enum OpenAvatarsMenu for value: OpenAvatarsMenu
        /// </summary>
        [EnumMember(Value = "OpenAvatarsMenu")]
        OpenAvatarsMenu = 3,

        /// <summary>
        /// Enum OpenHelpArticle for value: OpenHelpArticle
        /// </summary>
        [EnumMember(Value = "OpenHelpArticle")]
        OpenHelpArticle = 4,

        /// <summary>
        /// Enum OpenListingDetails for value: OpenListingDetails
        /// </summary>
        [EnumMember(Value = "OpenListingDetails")]
        OpenListingDetails = 5,

        /// <summary>
        /// Enum OpenSafetyMenu for value: OpenSafetyMenu
        /// </summary>
        [EnumMember(Value = "OpenSafetyMenu")]
        OpenSafetyMenu = 6,

        /// <summary>
        /// Enum OpenURL for value: OpenURL
        /// </summary>
        [EnumMember(Value = "OpenURL")]
        OpenURL = 7,

        /// <summary>
        /// Enum OpenVRCPlusMenu for value: OpenVRCPlusMenu
        /// </summary>
        [EnumMember(Value = "OpenVRCPlusMenu")]
        OpenVRCPlusMenu = 8,

        /// <summary>
        /// Enum OpenVRChatStore for value: OpenVRChatStore
        /// </summary>
        [EnumMember(Value = "OpenVRChatStore")]
        OpenVRChatStore = 9,

        /// <summary>
        /// Enum OpenWorldDetails for value: OpenWorldDetails
        /// </summary>
        [EnumMember(Value = "OpenWorldDetails")]
        OpenWorldDetails = 10,

        /// <summary>
        /// Enum OpenWorldsMenu for value: OpenWorldsMenu
        /// </summary>
        [EnumMember(Value = "OpenWorldsMenu")]
        OpenWorldsMenu = 11,

        /// <summary>
        /// Enum ScrollToShelf for value: scrollToShelf
        /// </summary>
        [EnumMember(Value = "scrollToShelf")]
        ScrollToShelf = 12
    }

    /// <summary>
    /// Gets or Sets Command
    /// </summary>
    [DataMember(Name = "command", IsRequired = true, EmitDefaultValue = true)]
    public CommandEnum Command { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="InfoPushDataClickable" /> class.
    /// </summary>

    protected InfoPushDataClickable() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InfoPushDataClickable" /> class.
    /// </summary>
    /// <param name="parameters">In case of OpenURL, this would contain the link..</param>
    /// <param name="command">command (required).</param>
    public InfoPushDataClickable(List<string> parameters = default, CommandEnum command = default)
    {
        this.Command = command;
        this.Parameters = parameters;
    }

    /// <summary>
    /// In case of OpenURL, this would contain the link.
    /// </summary>
    /// <value>In case of OpenURL, this would contain the link.</value>
    [DataMember(Name = "parameters", EmitDefaultValue = false)]
    public List<string> Parameters { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class InfoPushDataClickable {\n");
        sb.Append("  Parameters: ").Append(Parameters).Append('\n');
        sb.Append("  Command: ").Append(Command).Append('\n');
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
        return this.Equals(input as InfoPushDataClickable);
    }

    /// <summary>
    /// Returns true if InfoPushDataClickable instances are equal
    /// </summary>
    /// <param name="input">Instance of InfoPushDataClickable to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(InfoPushDataClickable input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Parameters == input.Parameters ||
                this.Parameters != null &&
                input.Parameters != null &&
                this.Parameters.SequenceEqual(input.Parameters)
            ) &&
            (
                this.Command == input.Command ||
                this.Command.Equals(input.Command)
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
            if (this.Parameters != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Parameters);
            }
            hashCode = (hashCode * 59) + this.Command.GetHashCode();
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
