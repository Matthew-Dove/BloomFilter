namespace BloomFilter.DataStructures;

/// <summary>
/// Bloom Filter: Add items to this filter, then query the filter later to see if the item was (potentially) previously added.
/// <para>Once created the filter cannot be resized. Once an item is added it cannot be removed.</para>
/// <para>This is different from a dictionary, because you cannot get the values of an item back, they aren't stored.</para>
/// <para>This makes the filter very memory efficient, it only uses a few bytes to store any object, and those bytes are shared.</para>
/// <para>The filter can tell you with 99% accuracy if an item was previously added to this filter (i.e. 1% false positive rate).</para>
/// </summary>
public sealed class IntegerFilter : AbstractBloomFilter<int>
{
    /// <summary>Creates a bloom filter for integers with sensible defaults.</summary>
    /// <param name="items">An array of items to create the filter with, you shouldn't add any items later, as the capacity is set to the array's length.</param>
    public IntegerFilter(int[] items) : base(items) { }

    /// <summary>Creates a bloom filter for integers with sensible defaults.</summary>
    /// <param name="items">An array of items to create the filter with, you can add items later, provided the capacity is set greather than the array's length.</param>
    /// <param name="capacity">The expected number of items to be added to the filter. This number must be greater than (or equal to) the items added, or every test will be a false positive. Capacity must be greater than zero.</param>
    public IntegerFilter(int[] items, int capacity) : base(items, capacity) { }

    /// <summary>Creates a bloom filter for integers with sensible defaults.</summary>
    /// <param name="capacity">The expected number of items to be added to the filter. This number must be greater than (or equal to) the items added, or every test will be a false positive. Capacity must be greater than zero.</param>
    public IntegerFilter(int capacity) : base(capacity) { }

    protected override int Hash(int input) => HashInt(input);

    private static int HashInt(int input)
    {
        unchecked
        {
            var x = (uint)input;
            x = ~x + (x << 15);
            x = x ^ (x >> 12);
            x = x + (x << 2);
            x = x ^ (x >> 4);
            x = x * 2057;
            x = x ^ (x >> 16);
            return (int)x;
        }
    }
}
