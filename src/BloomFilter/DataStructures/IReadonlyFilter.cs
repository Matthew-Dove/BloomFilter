namespace BloomFilter.DataStructures;

/// <summary>Stops users of this instance from adding new items to the filter.</summary>
public interface IReadonlyFilter<T>
{
    /// <summary>Checks for the possible existence of the item in the filter.</summary>
    /// <param name="item">The value to check.</param>
    /// <returns>False if the item definitely isn't in the filter. True if the item might be in the filter.</returns>
    bool Contains(T item);
}
