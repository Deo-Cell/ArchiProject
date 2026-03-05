using Microsoft.AspNetCore.Http;

namespace Archi.Library.Filters
{
    /// <summary>
    /// Parser pour extraire les filtres des paramètres de requête HTTP
    /// </summary>
    public class QueryFilterParser
    {
        /// <summary>
        /// Parse tous les paramètres de requête en excluant les mots-clés réservés
        /// </summary>
        /// <param name="queryParams">Collection de paramètres d'URL</param>
        /// <returns>Liste des filtres détectés</returns>
        public static List<Filter> ParseFilters(IQueryCollection queryParams)
        {
            var filters = new List<Filter>();

            // Ignorer les paramètres spéciaux utilisés pour d'autres fonctionnalités
            var ignoredParams = new[] { "range", "asc", "desc", "fields", "search" };

            foreach (var param in queryParams)
            {
                if (ignoredParams.Contains(param.Key.ToLower()))
                    continue;

                // param.Value est une StringValues, on prend le premier élément s'il existe
                var filter = ParseFilter(param.Key, param.Value.ToString());
                if (filter != null)
                    filters.Add(filter);
            }

            return filters;
        }

        private static Filter? ParseFilter(string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var filter = new Filter { PropertyName = propertyName };

            // Vérification du format Range: [min,max] ou [min,] ou [,max]
            if (value.StartsWith("[") && value.EndsWith("]"))
            {
                var rangeContent = value.Trim('[', ']');
                var parts = rangeContent.Split(',');

                if (parts.Length == 2)
                {
                    if (string.IsNullOrEmpty(parts[0]))
                    {
                        // Format [,20] → Moins que ou égal (LessThan)
                        filter.Type = FilterType.LessThan;
                        filter.Values.Add(parts[1]);
                    }
                    else if (string.IsNullOrEmpty(parts[1]))
                    {
                        // Format [10,] → Plus que ou égal (GreaterThan)
                        filter.Type = FilterType.GreaterThan;
                        filter.Values.Add(parts[0]);
                    }
                    else
                    {
                        // Format [10,20] → Entre (Range)
                        filter.Type = FilterType.Range;
                        filter.Values.Add(parts[0]);
                        filter.Values.Add(parts[1]);
                    }
                }
                else
                {
                    // Si mal formatté (ex: [10]), on ignore ou on traite comme exact? 
                    // Pour l'instant, traitons comme exact de la chaîne incluant les crochets.
                    filter.Type = FilterType.Exact;
                    filter.Values.Add(value);
                }
            }
            // Format Multiple: valeur1,valeur2
            else if (value.Contains(','))
            {
                filter.Type = FilterType.Multiple;
                filter.Values.AddRange(value.Split(',').Select(v => v.Trim()));
            }
            // Format Exact par défaut
            else
            {
                filter.Type = FilterType.Exact;
                filter.Values.Add(value);
            }

            return filter;
        }
    }
}
