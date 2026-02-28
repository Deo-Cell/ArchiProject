using Archi.Library.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Archi.Tests
{
    /// <summary>
    /// Tests unitaires pour SearchParser
    /// </summary>
    public class SearchParserTests
    {
        private static IQueryCollection CreateQuery(Dictionary<string, string> parameters)
        {
            var dict = new Dictionary<string, StringValues>();
            foreach (var kvp in parameters)
                dict[kvp.Key] = new StringValues(kvp.Value);
            return new QueryCollection(dict);
        }

        [Fact]
        public void ParseSearch_NoSearchParam_ReturnsEmpty()
        {
            var query = CreateQuery(new Dictionary<string, string>());
            var result = SearchParser.ParseSearch(query);

            Assert.Empty(result);
        }

        [Fact]
        public void ParseSearch_SingleSearch()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "search", "name:*napoli*" } });
            var result = SearchParser.ParseSearch(query);

            Assert.Single(result);
            Assert.Equal("name", result[0].PropertyName);
            Assert.Equal("*napoli*", result[0].SearchTerm);
        }

        [Fact]
        public void ParseSearch_MultipleSearches()
        {
            var query = CreateQuery(new Dictionary<string, string>
            {
                { "search", "name:*napoli*,base:tomate" }
            });
            var result = SearchParser.ParseSearch(query);

            Assert.Equal(2, result.Count);
            Assert.Equal("name", result[0].PropertyName);
            Assert.Equal("*napoli*", result[0].SearchTerm);
            Assert.Equal("base", result[1].PropertyName);
            Assert.Equal("tomate", result[1].SearchTerm);
        }

        [Fact]
        public void ParseSearch_EmptySearch_ReturnsEmpty()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "search", "" } });
            var result = SearchParser.ParseSearch(query);

            Assert.Empty(result);
        }

        [Fact]
        public void ParseSearch_InvalidFormat_IgnoresEntry()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "search", "invalidformat" } });
            var result = SearchParser.ParseSearch(query);

            Assert.Empty(result); // Pas de ":" donc ignoré
        }
    }
}
