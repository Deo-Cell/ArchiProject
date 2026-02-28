using Archi.Library.PartialResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Archi.Tests
{
    /// <summary>
    /// Tests unitaires pour FieldSelector (réponses partielles)
    /// </summary>
    public class FieldSelectorTests
    {
        private static IQueryCollection CreateQuery(Dictionary<string, string> parameters)
        {
            var dict = new Dictionary<string, StringValues>();
            foreach (var kvp in parameters)
                dict[kvp.Key] = new StringValues(kvp.Value);
            return new QueryCollection(dict);
        }

        [Fact]
        public void ParseFields_NoFieldsParam_ReturnsEmpty()
        {
            var query = CreateQuery(new Dictionary<string, string>());
            var result = FieldSelector.ParseFields(query);

            Assert.Empty(result);
        }

        [Fact]
        public void ParseFields_SingleField()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "fields", "name" } });
            var result = FieldSelector.ParseFields(query);

            Assert.Single(result);
            Assert.Equal("name", result[0]);
        }

        [Fact]
        public void ParseFields_MultipleFields()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "fields", "id,name,price" } });
            var result = FieldSelector.ParseFields(query);

            Assert.Equal(3, result.Count);
            Assert.Contains("id", result);
            Assert.Contains("name", result);
            Assert.Contains("price", result);
        }

        [Fact]
        public void ParseFields_WhitespaceHandled()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "fields", " id , name " } });
            var result = FieldSelector.ParseFields(query);

            Assert.Equal(2, result.Count);
            Assert.Contains("id", result);
            Assert.Contains("name", result);
        }

        [Fact]
        public void ParseFields_EmptyValue_ReturnsEmpty()
        {
            var query = CreateQuery(new Dictionary<string, string> { { "fields", "" } });
            var result = FieldSelector.ParseFields(query);

            Assert.Empty(result);
        }
    }
}
