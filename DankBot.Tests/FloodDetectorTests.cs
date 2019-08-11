namespace DankBot.Tests
{
    using System.Linq;

    using Xunit;

    public class FloodDetectorTests
    {
        private const int _threshold = 10;
        private const int _id = 12345;

        [Fact]
        public void FloodDetector_AddLessThanThreshold_ShouldNotBeFlagged()
        {
            var floodDetector = new FloodDetector(60, _threshold);

            foreach (var i in Enumerable.Range(1, _threshold - 1))
            {
                floodDetector.Add(_id);
            }

            Assert.False(floodDetector.IsFlooding(_id));
        }

        [Fact]
        public void FloodDetector_AddEqualToThreshold_ShouldBeFlagged()
        {
            var floodDetector = new FloodDetector(60, _threshold);

            foreach (var i in Enumerable.Range(1, _threshold))
            {
                floodDetector.Add(_id);
            }

            Assert.True(floodDetector.IsFlooding(_id));
        }

        [Fact]
        public void FloodDetector_AddGreaterThanThreshold_ShouldBeFlagged()
        {
            var floodDetector = new FloodDetector(60, _threshold);

            foreach (var i in Enumerable.Range(1, _threshold + 1))
            {
                floodDetector.Add(_id);
            }

            Assert.True(floodDetector.IsFlooding(_id));
        }
    }
}
