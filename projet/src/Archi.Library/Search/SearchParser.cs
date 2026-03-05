using Microsoft.AspNetCore.Http;

namespace Archi.Library.Search
{
    /// <summary>
    /// Parser pour extraire les requêtes de recherche depuis l'URL
    /// Exemple : ?search=name:*napoli*
    /// </summary>
    public class SearchParser
    {
        public static List<SearchQuery> ParseSearch(IQueryCollection queryParams)
        {
            var searches = new List<SearchQuery>();

            if (queryParams.TryGetValue("search", out var searchValues))
            {
                foreach (var value in searchValues)
                {
                    if (string.IsNullOrEmpty(value)) continue;

                    // On peut avoir plusieurs recherches séparées par des virgules
                    // Exemple: ?search=name:*napoli*,base:tomate
                    var searchParts = value.Split(',');

                    foreach (var part in searchParts)
                    {
                        var keyValue = part.Split(':', 2);
                        if (keyValue.Length == 2)
                        {
                            searches.Add(new SearchQuery
                            {
                                PropertyName = keyValue[0].Trim(),
                                SearchTerm = keyValue[1].Trim()
                            });
                        }
                    }
                }
            }

            return searches;
        }
    }
}
