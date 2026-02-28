using Archi.Library.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Archi.Tests
{
    /// <summary>
    /// Tests unitaires pour QueryFilterParser
    /// </summary>
    public class QueryFilterParserTests
    {
        private static IQueryCollection CreateQuery(Dictionary<string, string> parameters)
        {
            var dict = new Dictionary<string, StringValues>();
            foreach (var kvp in parameters)
                dict[kvp.Key] = new StringValues(kvp.Value);
            return new QueryCollection(dict);
        }

        // ============================
        //  Filtre Exact
        // ============================

        [Fact]
        public void ParseFilters_ExactFilter()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "name", "Margherita" } });
            var filters = QueryFilterParser.ParseFilters(query);

            Assert.Single(filters);
            Assert.Equal("name", filters[0].PropertyName);
            Assert.Equal(FilterType.Exact, filters[0].Type);
            Assert.Equal("Margherita", filters[0].Values[0]);
        }

        // ============================
        //  Filtre Multiple
        // ============================

        [Fact]
        public void ParseFilters_MultipleValues()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "name", "Margherita,Royale" } });
            var filters = QueryFilterParser.ParseFilters(query);

            Assert.Single(filters);
            Assert.Equal(FilterType.Multiple, filters[0].Type);
            Assert.Equal(2, filters[0].Values.Count);
            Assert.Contains("Margherita", filters[0].Values);
            Assert.Contains("Royale", filters[0].Values);
        }

        // ============================
        //  Filtre Range
        // ============================

        [Fact]
        public void ParseFilters_RangeFilter()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "price", "[5,15]" } });
            var filters = QueryFilterParser.ParseFilters(query);

            Assert.Single(filters);
            Assert.Equal(FilterType.Range, filters[0].Type);
            Assert.Equal("5", filters[0].Values[0]);
            Assert.Equal("15", filters[0].Values[1]);
        }

        [Fact]
        public void ParseFilters_GreaterThanFilter()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "price", "[10,]" } });
            var filters = QueryFilterParser.ParseFilters(query);

            Assert.Single(filters);
            Assert.Equal(FilterType.GreaterThan, filters[0].Type);
            Assert.Equal("10", filters[0].Values[0]);
        }

        [Fact]
        public void ParseFilters_LessThanFilter()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "price", "[,20]" } });
            var filters = QueryFilterParser.ParseFilters(query);

            Assert.Single(filters);
            Assert.Equal(FilterType.LessThan, filters[0].Type);
            Assert.Equal("20", filters[0].Values[0]);
        }

        // ============================
        //  Paramètres ignorés
        // ============================

        [Fact]
        public void ParseFilters_IgnoresReservedParams()
        {
            var query = CreateQuery(new Dictionary<string, string>
            {
                { "range", "0-10" },
                { "asc", "name" },
                { "desc", "price" },
                { "fields", "id,name" },
                { "search", "name:*test*" },
                { "name", "Pizza" }    // seul filtre valide
            });
            var filters = QueryFilterParser.ParseFilters(query);

            Assert.Single(filters);
            Assert.Equal("name", filters[0].PropertyName);
        }

        // ============================
        //  Pas de filtres
        // ============================

        [Fact]
        public void ParseFilters_EmptyQuery_ReturnsEmpty()
        {
            var query = CreateQuery(new Dictionary<string, string>());
            var filters = QueryFilterParser.ParseFilters(query);

            Assert.Empty(filters);
        }

        [Fact]
        public void ParseFilters_MultipleFilters()
        {
            var query = CreateQuery(new Dictionary<string, string>
            {
                { "name", "Margherita" },
                { "price", "[5,15]" }
            });
            var filters = QueryFilterParser.ParseFilters(query);

            Assert.Equal(2, filters.Count);
        }
    }
}
