namespace Archi.Library.Sorting
{
    /// <summary>
    /// Parser pour les paramètres de tri (asc/desc)
    /// </summary>
    public class SortingParser
    {
        /// <summary>
        /// Parse les paramètres de tri de l'URL
        /// </summary>
        /// <param name="ascParam">Paramètre asc (ex: "name,price")</param>
        /// <param name="descParam">Paramètre desc (ex: "createdAt")</param>
        /// <returns>Liste de tuples (PropertyName, IsAscending)</returns>
        public static List<(string PropertyName, bool IsAscending)> ParseSorting(
            string? ascParam,
            string? descParam)
        {
            var sortings = new List<(string, bool)>();

            // Parser les tris ascendants
            if (!string.IsNullOrEmpty(ascParam))
            {
                var properties = ascParam.Split(',');
                foreach (var prop in properties)
                {
                    var trimmed = prop.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        sortings.Add((trimmed, true));
                    }
                }
            }

            // Parser les tris descendants
            if (!string.IsNullOrEmpty(descParam))
            {
                var properties = descParam.Split(',');
                foreach (var prop in properties)
                {
                    var trimmed = prop.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        sortings.Add((trimmed, false));
                    }
                }
            }

            return sortings;
        }
    }
}
