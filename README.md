# Bloom Filter

[PM> NuGet\Install-Package md.bloomfilter](https://www.nuget.org/packages/md.bloomfilter/)  

A **Bloom Filter** is a space efficient, probabilistic data structure.  
Storing a value as only several bits - after running it though several hashing algorithms.  

![Bloom Filter Data Structure](assets/bloom_filter.png)  

Add items to a **Bloom Filter**, then query the filter later to see if the item was (*potentially*) previously added.  
Once created the filter cannot be resized, once an item is added it cannot be removed.  
The filter very memory efficient, it only uses a few bytes to store an object; and those bytes are shared.  
The filter can tell you with **99%** accuracy if an item was previously added (*i.e. it has a 1% false positive rate*).  

A **Bloom Filter** can tell you if an item is definitely **not** in the filter, and with 99% certainty, tell you if it has seen the item before.  
It can't be 100% certain - as hash functions have collisions (*i.e. two different values with the same hash*).  
Use a filter over an expensive resource to check if it's likey there or not.  
For example as a blacklist, on a match you go to a database to make sure the item is actually blacklisted.

# Type Implementations

This project contains two explicit types of **Bloom Filters** - `integer`, and `string`.  
To create a specific type of **Bloom Filter**, inherent from `AbstractBloomFilter<T>`, and implement the `Hash` function.  

Note a good hash function for a **Bloom Filter** should be fast as possible, with each value of `T` uniformly distributed across the `int32` range.  
Cryptographic hashes are a poor choice, as the size of the filter grows; the speed to compute hashes grows too.   
A good example of a non-cryptographic hash function would be [MurmurHash3](https://en.wikipedia.org/wiki/MurmurHash).  

If you don't want to roll your own type's hash, then find a way to represent your type as an `int`, or `string`, and use the provided filters.  
The values used as input to the hash function should not change over time, otherwise future matches will be incorrect.  

### `IntFilter`

```cs
// The source of items we want to add to the filter: numbers 97 - 122 (inclusive).
var items = Enumerable.Range(97, 26).ToArray();

// The integer bloom filter.
var filter = new IntegerFilter(items);

// These items where added to the filter.
Assert.True(filter.Contains(97));
Assert.True(filter.Contains(122));

// These items where not added (outside of the range).
Assert.False(filter.Contains(96));
Assert.False(filter.Contains(123));
```

### `StringFilter`

```cs
// The source of items we want to add to the filter: characters a - z (inclusive).
var items = Enumerable.Range(97, 26).Select(c => Convert.ToChar(c).ToString()).ToArray();

// The string bloom filter.
var filter = new StringFilter(items);

// These items where added to the filter.
Assert.True(filter.Contains("a"));
Assert.True(filter.Contains("z"));

// These items where not added (outside of the range).
Assert.False(filter.Contains("`"));
Assert.False(filter.Contains("{"));
```

### `IReadonlyFilter<T>`

Any **Bloom Filter** can be changed to a readonly collection by calling the base method `Readonly()`.  
This prevents users of this instance from adding any new items to the filter.

```cs
// The filter's source.
var items = Enumerable.Range(97, 26).ToArray();

// The bloom filter.
var filter = new IntegerFilter(items);

// The readonly instance of the bloom filter.
var readonlyFilter = filter.Readonly();
```

# Credits
* [Icon](https://www.flaticon.com/free-icon/bird_2630452) made by [Vitaly Gorbachev](https://www.flaticon.com/authors/vitaly-gorbachev) from [Flaticon](https://www.flaticon.com/)
* [Bloom Filter Example](https://en.wikipedia.org/wiki/Bloom_filter) from wikipedia.

# Changelog

## 1.0.0

* Added **Bloom Filter** implementations for `integer`, and `string` types (*i.e. their hash functions*).
