using System.Collections;
using System;

namespace BloomFilter.DataStructures;

/// <summary>
/// Bloom Filter: Add items to this filter, then query the filter later to see of the item was (potentially) previously added.
/// <para>Once created the filter cannot be resized. Once an item is added it cannot be removed.</para>
/// <para>This is different from a dictionary, because you cannot get the values of an item back, they aren't stored.</para>
/// <para>This makes the filter very memory efficient, it only uses a few bytes to store any object, and those bytes are shared.</para>
/// <para>The filter can tell you with 99% accuracy if an item was previously added to this filter (i.e. 1% false positive rate).</para>
/// </summary>
public abstract class AbstractBloomFilter<T> : IReadonlyFilter<T>
{
    private readonly int _hashFunctions = 0;
    private readonly BitArray _hashBits = null;

    /// <summary>Creates a bloom filter for generic elements with sensible defaults.</summary>
    /// <param name="items">An array of items to create the filter with, you shouldn't add any items later, as the capacity is set to the array's length.</param>
    public AbstractBloomFilter(T[] items) : this(items.Length)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        for (int i = 0; i < items.Length; i++)
        {
            Add(items[i]);
        }
    }

    /// <summary>Creates a bloom filter for strings with sensible defaults.</summary>
    /// <param name="items">An array of items to create the filter with, you can add items later, provided the capacity is set greather than the array's length.</param>
    public AbstractBloomFilter(T[] items, int capacity) : this(capacity)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        if (capacity < items.Length)
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity, $"{nameof(capacity)} must be greater than or equal to {nameof(items)} length.");

        for (int i = 0; i < items.Length; i++)
        {
            Add(items[i]);
        }
    }

    /// <summary>Creates a bloom filter for strings with sensible defaults.</summary>
    /// <param name="capacity">The expected number of items to be added to the filter. This number must be greater than (or equal to) the items added, or every test will be a false positive. Capacity must be greater than zero.</param>
    public AbstractBloomFilter(int capacity)
    {
        if (capacity < 1)
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity, $"{nameof(capacity)} must be greater than zero.");

        capacity = capacity + 1;
        var errorRate = GetErrorRate(capacity);
        var m = GetM(capacity, errorRate);
        var k = GetK(capacity, m);

        _hashFunctions = k;
        _hashBits = new BitArray(m);
    }

    /// <summary>Stops users of this instance from adding new items to the filter.</summary>
    /// <returns>A readonly BloomFilter.</returns>
    public IReadonlyFilter<T> Readonly() => this;

    /// <summary>Adds a new item to the filter. It cannot be removed.</summary>
    /// <param name="item">The item to add to the filter.</param>
    public void Add(T item)
    {
        var primaryHash = item.GetHashCode(); // Start flipping bits for each hash of item.
        var secondaryHash = Hash(item);

        for (var i = 0; i < _hashFunctions; i++)
        {
            var hash = ComputeHash(primaryHash, secondaryHash, i);
            _hashBits[hash] = true;
        }
    }

    /// <summary>Checks for the existence of the item in the filter.</summary>
    /// <param name="item">The value to check.</param>
    /// <returns>False if the item isn't in the filter.</returns>
    public bool Contains(T item)
    {
        var result = true;
        var primaryHash = item.GetHashCode();
        var secondaryHash = Hash(item);

        for (var i = 0; i < _hashFunctions; i++)
        {
            var hash = ComputeHash(primaryHash, secondaryHash, i);
            if (_hashBits[hash] == false)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    /// <summary>Maps two hash values, with their current iteration to an element in a vector.</summary>
    /// <param name="primaryHash">The result from the first hash function.</param>
    /// <param name="secondaryHash">The result from the second hash function.</param>
    /// <param name="i">The current iteration of the bit flipping loop.</param>
    /// <returns>A bit to check or flip in the bit vector.</returns>
    private int ComputeHash(int primaryHash, int secondaryHash, int i) => Math.Abs((primaryHash + (i * secondaryHash)) % _hashBits.Count);

    /// <summary>Computes the number of bits to use in a vector (flipping flags).</summary>
    private static int GetM(int capacity, double errorRate) => (int)Math.Ceiling(capacity * Math.Log(errorRate, 1.0D / Math.Pow(2.0D, Math.Log(2.0D))));

    /// <summary>Computes the number of hash functions to use.</summary>
    private static int GetK(int capacity, int m) => (int)Math.Round(Math.Log(2.0D) * m / capacity);

    /// <summary>Calculates an error rate (false positive) based on the capacity.</summary>
    /// <param name="capacity">The size of the filter.</param>
    /// <returns>The best guess for an error rate.</returns>
    private static double GetErrorRate(int capacity)
    {
        var c = 1.0D / capacity;

        if (c == 0D)
        {
            c = Math.Pow(0.6185D, int.MaxValue / capacity);
        }

        return c;
    }

    /// <summary>Hashes a type to an int.</summary>
    /// <param name="input">The element to hash.</param>
    /// <returns>The hashed result.</returns>
    protected abstract int Hash(T input);
}
