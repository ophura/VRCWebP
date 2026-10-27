using System.Collections.Generic;

namespace VRChat.API.Client;

/// <summary>Hashes ordered collection contents using the element equality used by generated SequenceEqual methods.</summary>
internal static class CollectionHash
{
    /// <summary>Matches dictionary pair equality independently of insertion order.</summary>
    internal static int Unordered<T>(IEnumerable<T> Values)
    {
        int Hash = 0;

        foreach (T Value in Values)
        {
            Hash = unchecked(Hash + (Value is null ? 0 : EqualityComparer<T>.Default.GetHashCode(Value)));
        }

        return Hash;
    }

    internal static int Get<T>(IEnumerable<T> Values)
    {
        System.HashCode Hash = new();

        foreach (T Value in Values)
        {
            Hash.Add(Value);
        }

        return Hash.ToHashCode();
    }
}
