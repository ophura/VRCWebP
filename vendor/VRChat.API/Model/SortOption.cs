
using System.Runtime.Serialization;

namespace VRChat.API.Model;

/// <summary>
/// Defines SortOption
/// </summary>

public enum SortOption
{
    /// <summary>
    /// Enum CreatedAt for value: _created_at
    /// </summary>
    [EnumMember(Value = "_created_at")]
    CreatedAt = 1,

    /// <summary>
    /// Enum UpdatedAt for value: _updated_at
    /// </summary>
    [EnumMember(Value = "_updated_at")]
    UpdatedAt = 2,

    /// <summary>
    /// Enum Contains for value: contains
    /// </summary>
    [EnumMember(Value = "contains")]
    Contains = 3,

    /// <summary>
    /// Enum Created for value: created
    /// </summary>
    [EnumMember(Value = "created")]
    Created = 4,

    /// <summary>
    /// Enum Exact for value: exact
    /// </summary>
    [EnumMember(Value = "exact")]
    Exact = 5,

    /// <summary>
    /// Enum Favorites for value: favorites
    /// </summary>
    [EnumMember(Value = "favorites")]
    Favorites = 6,

    /// <summary>
    /// Enum Heat for value: heat
    /// </summary>
    [EnumMember(Value = "heat")]
    Heat = 7,

    /// <summary>
    /// Enum Hotness for value: hotness
    /// </summary>
    [EnumMember(Value = "hotness")]
    Hotness = 8,

    /// <summary>
    /// Enum LabsPublicationDate for value: labsPublicationDate
    /// </summary>
    [EnumMember(Value = "labsPublicationDate")]
    LabsPublicationDate = 9,

    /// <summary>
    /// Enum Magic for value: magic
    /// </summary>
    [EnumMember(Value = "magic")]
    Magic = 10,

    /// <summary>
    /// Enum Name for value: name
    /// </summary>
    [EnumMember(Value = "name")]
    Name = 11,

    /// <summary>
    /// Enum Order for value: order
    /// </summary>
    [EnumMember(Value = "order")]
    Order = 12,

    /// <summary>
    /// Enum Popularity for value: popularity
    /// </summary>
    [EnumMember(Value = "popularity")]
    Popularity = 13,

    /// <summary>
    /// Enum PublicationDate for value: publicationDate
    /// </summary>
    [EnumMember(Value = "publicationDate")]
    PublicationDate = 14,

    /// <summary>
    /// Enum Random for value: random
    /// </summary>
    [EnumMember(Value = "random")]
    Random = 15,

    /// <summary>
    /// Enum Relevance for value: relevance
    /// </summary>
    [EnumMember(Value = "relevance")]
    Relevance = 16,

    /// <summary>
    /// Enum ReportCount for value: reportCount
    /// </summary>
    [EnumMember(Value = "reportCount")]
    ReportCount = 17,

    /// <summary>
    /// Enum ReportScore for value: reportScore
    /// </summary>
    [EnumMember(Value = "reportScore")]
    ReportScore = 18,

    /// <summary>
    /// Enum Shuffle for value: shuffle
    /// </summary>
    [EnumMember(Value = "shuffle")]
    Shuffle = 19,

    /// <summary>
    /// Enum Trending for value: trending
    /// </summary>
    [EnumMember(Value = "trending")]
    Trending = 20,

    /// <summary>
    /// Enum Trust for value: trust
    /// </summary>
    [EnumMember(Value = "trust")]
    Trust = 21,

    /// <summary>
    /// Enum Updated for value: updated
    /// </summary>
    [EnumMember(Value = "updated")]
    Updated = 22
}
