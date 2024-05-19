using BloomFilter.DataStructures;

namespace Tests.BloomFilter.DataStructures
{
    public class StringFilterTests
    {
        [Fact]
        public void ContainsItem_IsFound()
        {
            var items = Enumerable.Range(97, 26).Select(c => Convert.ToChar(c).ToString()).ToArray();

            var filter = new StringFilter(items);

            Assert.True(filter.Contains("a"));
            Assert.True(filter.Contains("z"));
        }

        [Fact]
        public void DoesNotContainsItem_IsNotFound()
        {
            var items = Enumerable.Range(97, 26).Select(c => Convert.ToChar(c).ToString()).ToArray();

            var filter = new StringFilter(items);

            Assert.False(filter.Contains("`"));
            Assert.False(filter.Contains("{"));
        }
    }
}
