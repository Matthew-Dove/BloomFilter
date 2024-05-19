using BloomFilter.DataStructures;

namespace Tests.BloomFilter.DataStructures
{
    public class IntegerFilterTests
    {
        [Fact]
        public void ContainsItem_IsFound()
        {
            var items = Enumerable.Range(97, 26).ToArray();

            var filter = new IntegerFilter(items);

            Assert.True(filter.Contains(97));
            Assert.True(filter.Contains(122));
        }

        [Fact]
        public void DoesNotContainsItem_IsNotFound()
        {
            var items = Enumerable.Range(97, 26).ToArray();

            var filter = new IntegerFilter(items);

            Assert.False(filter.Contains(96));
            Assert.False(filter.Contains(123));
        }
    }
}
