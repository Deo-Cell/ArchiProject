using Archi.Library.Sorting;

namespace Archi.Tests
{
    /// <summary>
    /// Tests unitaires pour SortingParser
    /// </summary>
    public class SortingParserTests
    {
        [Fact]
        public void ParseSorting_NullParams_ReturnsEmpty()
        {
            var result = SortingParser.ParseSorting(null, null);
            Assert.Empty(result);
        }

        [Fact]
        public void ParseSorting_AscOnly_ReturnsAscending()
        {
            var result = SortingParser.ParseSorting("name", null);

            Assert.Single(result);
            Assert.Equal("name", result[0].PropertyName);
            Assert.True(result[0].IsAscending);
        }

        [Fact]
        public void ParseSorting_DescOnly_ReturnsDescending()
        {
            var result = SortingParser.ParseSorting(null, "price");

            Assert.Single(result);
            Assert.Equal("price", result[0].PropertyName);
            Assert.False(result[0].IsAscending);
        }

        [Fact]
        public void ParseSorting_MultipleAsc_ParsesAll()
        {
            var result = SortingParser.ParseSorting("name,price", null);

            Assert.Equal(2, result.Count);
            Assert.Equal("name", result[0].PropertyName);
            Assert.Equal("price", result[1].PropertyName);
            Assert.True(result[0].IsAscending);
            Assert.True(result[1].IsAscending);
        }

        [Fact]
        public void ParseSorting_BothAscAndDesc()
        {
            var result = SortingParser.ParseSorting("name", "price");

            Assert.Equal(2, result.Count);
            Assert.Equal("name", result[0].PropertyName);
            Assert.True(result[0].IsAscending);
            Assert.Equal("price", result[1].PropertyName);
            Assert.False(result[1].IsAscending);
        }

        [Fact]
        public void ParseSorting_WhitespaceHandled()
        {
            var result = SortingParser.ParseSorting(" name , price ", null);

            Assert.Equal(2, result.Count);
            Assert.Equal("name", result[0].PropertyName);
            Assert.Equal("price", result[1].PropertyName);
        }

        [Fact]
        public void ParseSorting_EmptyStrings_ReturnsEmpty()
        {
            var result = SortingParser.ParseSorting("", "");
            Assert.Empty(result);
        }
    }
}
