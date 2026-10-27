
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VRChat.API.Model;

/// <summary>
/// PermissionData
/// </summary>
[DataContract(Name = "PermissionData")]
public partial class PermissionData : IEquatable<PermissionData>, IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionData" /> class.
    /// </summary>
    /// <param name="badges">Badges afforded the user by this permission.</param>
    /// <param name="max">Maximum value afforded the user by this permission.</param>
    /// <param name="maxFavoriteGroups">Maximum favorite groups afforded the user by this permission.</param>
    /// <param name="maxFavoritesPerGroup">Maximum favorites per group afforded the user by this permission.</param>
    /// <param name="tags">Tags afforded the user by this permission.</param>
    public PermissionData(List<string> badges = default, int max = default, Dictionary<string, int> maxFavoriteGroups = default, Dictionary<string, int> maxFavoritesPerGroup = default, List<string> tags = default)
    {
        this.Badges = badges;
        this.Max = max;
        this.MaxFavoriteGroups = maxFavoriteGroups;
        this.MaxFavoritesPerGroup = maxFavoritesPerGroup;
        this.Tags = tags;
    }

    /// <summary>
    /// Badges afforded the user by this permission
    /// </summary>
    /// <value>Badges afforded the user by this permission</value>
    [DataMember(Name = "badges", EmitDefaultValue = false)]
    public List<string> Badges { get; set; }

    /// <summary>
    /// Maximum value afforded the user by this permission
    /// </summary>
    /// <value>Maximum value afforded the user by this permission</value>
    [DataMember(Name = "max", EmitDefaultValue = false)]
    public int Max { get; set; }

    /// <summary>
    /// Maximum favorite groups afforded the user by this permission
    /// </summary>
    /// <value>Maximum favorite groups afforded the user by this permission</value>
    [DataMember(Name = "maxFavoriteGroups", EmitDefaultValue = false)]
    public Dictionary<string, int> MaxFavoriteGroups { get; set; }

    /// <summary>
    /// Maximum favorites per group afforded the user by this permission
    /// </summary>
    /// <value>Maximum favorites per group afforded the user by this permission</value>
    [DataMember(Name = "maxFavoritesPerGroup", EmitDefaultValue = false)]
    public Dictionary<string, int> MaxFavoritesPerGroup { get; set; }

    /// <summary>
    /// Tags afforded the user by this permission
    /// </summary>
    /// <value>Tags afforded the user by this permission</value>
    [DataMember(Name = "tags", EmitDefaultValue = false)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class PermissionData {\n");
        sb.Append("  Badges: ").Append(Badges).Append('\n');
        sb.Append("  Max: ").Append(Max).Append('\n');
        sb.Append("  MaxFavoriteGroups: ").Append(MaxFavoriteGroups).Append('\n');
        sb.Append("  MaxFavoritesPerGroup: ").Append(MaxFavoritesPerGroup).Append('\n');
        sb.Append("  Tags: ").Append(Tags).Append('\n');
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
        return this.Equals(input as PermissionData);
    }

    /// <summary>
    /// Returns true if PermissionData instances are equal
    /// </summary>
    /// <param name="input">Instance of PermissionData to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(PermissionData input)
    {
        if (input == null)
        {
            return false;
        }
        return
            (
                this.Badges == input.Badges ||
                this.Badges != null &&
                input.Badges != null &&
                this.Badges.SequenceEqual(input.Badges)
            ) &&
            (
                this.Max == input.Max ||
                this.Max.Equals(input.Max)
            ) &&
            (
                this.MaxFavoriteGroups == input.MaxFavoriteGroups ||
                this.MaxFavoriteGroups != null &&
                input.MaxFavoriteGroups != null &&
                this.MaxFavoriteGroups.SequenceEqual(input.MaxFavoriteGroups)
            ) &&
            (
                this.MaxFavoritesPerGroup == input.MaxFavoritesPerGroup ||
                this.MaxFavoritesPerGroup != null &&
                input.MaxFavoritesPerGroup != null &&
                this.MaxFavoritesPerGroup.SequenceEqual(input.MaxFavoritesPerGroup)
            ) &&
            (
                this.Tags == input.Tags ||
                this.Tags != null &&
                input.Tags != null &&
                this.Tags.SequenceEqual(input.Tags)
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
            if (this.Badges != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Badges);
            }
            hashCode = (hashCode * 59) + this.Max.GetHashCode();
            if (this.MaxFavoriteGroups != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.MaxFavoriteGroups);
            }
            if (this.MaxFavoritesPerGroup != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.MaxFavoritesPerGroup);
            }
            if (this.Tags != null)
            {
                hashCode = (hashCode * 59) + VRChat.API.Client.CollectionHash.Get(this.Tags);
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
