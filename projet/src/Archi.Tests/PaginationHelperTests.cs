using Archi.Library.Pagination;

namespace Archi.Tests
{
    /// <summary>
    /// Tests unitaires pour PaginationHelper (ParseRange et ApplyPagination)
    /// </summary>
    public class PaginationHelperTests
    {
        // ============================
        //  ParseRange
        // ============================

        [Fact]
        public void ParseRange_NullParam_ReturnsDefaults()
        {
            var (start, end) = PaginationHelper.ParseRange(null);
            Assert.Equal(0, start);
            Assert.Equal(49, end);
        }

        [Fact]
        public void ParseRange_EmptyString_ReturnsDefaults()
        {
            var (start, end) = PaginationHelper.ParseRange("");
            Assert.Equal(0, start);
            Assert.Equal(49, end);
        }

        [Fact]
        public void ParseRange_ValidRange_ReturnsParsed()
        {
            var (start, end) = PaginationHelper.ParseRange("0-25");
            Assert.Equal(0, start);
            Assert.Equal(25, end);
        }

        [Fact]
        public void ParseRange_CustomRange_ReturnsParsed()
        {
            var (start, end) = PaginationHelper.ParseRange("10-50");
            Assert.Equal(10, start);
            Assert.Equal(50, end);
        }

        [Fact]
        public void ParseRange_InvalidFormat_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => PaginationHelper.ParseRange("invalid"));
        }

        [Fact]
        public void ParseRange_NonNumeric_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => PaginationHelper.ParseRange("a-b"));
        }

        [Fact]
        public void ParseRange_NegativeStart_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => PaginationHelper.ParseRange("-1-10"));
        }

        [Fact]
        public void ParseRange_EndLessThanStart_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => PaginationHelper.ParseRange("10-5"));
        }

        // ============================
        //  ApplyPagination
        // ============================

        [Fact]
        public void ApplyPagination_ReturnsCorrectSubset()
        {
            var data = Enumerable.Range(1, 100).AsQueryable();
            var result = PaginationHelper.ApplyPagination(data, 0, 9).ToList();

            Assert.Equal(10, result.Count);
            Assert.Equal(1, result.First());
            Assert.Equal(10, result.Last());
        }

        [Fact]
        public void ApplyPagination_MiddleRange()
        {
            var data = Enumerable.Range(1, 100).AsQueryable();
            var result = PaginationHelper.ApplyPagination(data, 10, 19).ToList();

            Assert.Equal(10, result.Count);
            Assert.Equal(11, result.First());
            Assert.Equal(20, result.Last());
        }

        [Fact]
        public void ApplyPagination_BeyondData_ReturnsAvailable()
        {
            var data = Enumerable.Range(1, 5).AsQueryable();
            var result = PaginationHelper.ApplyPagination(data, 0, 49).ToList();

            Assert.Equal(5, result.Count);
        }
    }
}
