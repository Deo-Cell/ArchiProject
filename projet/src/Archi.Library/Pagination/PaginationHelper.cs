namespace Archi.Library.Pagination
{
    /// <summary>
    /// Helper pour gérer la pagination avec le format ?range=0-25
    /// </summary>
    public class PaginationHelper
    {
        /// <summary>
        /// Parse le paramètre range de l'URL
        /// </summary>
        /// <param name="rangeParam">Paramètre range (ex: "0-25")</param>
        /// <returns>Tuple (Start, End)</returns>
        public static (int Start, int End) ParseRange(string? rangeParam)
        {
            if (string.IsNullOrEmpty(rangeParam))
            {
                return (0, 49); // Par défaut : 50 premiers éléments
            }

            var parts = rangeParam.Split('-');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Format invalide. Utilisez: ?range=0-25");
            }

            if (!int.TryParse(parts[0], out int start) || !int.TryParse(parts[1], out int end))
            {
                throw new ArgumentException("Les valeurs doivent être des nombres entiers");
            }

            if (start < 0 || end < start)
            {
                throw new ArgumentException("Range invalide: start doit être >= 0 et end >= start");
            }

            return (start, end);
        }

        /// <summary>
        /// Applique la pagination sur une requête LINQ
        /// </summary>
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="query">Requête LINQ</param>
        /// <param name="start">Index de début (0-based)</param>
        /// <param name="end">Index de fin (inclusif)</param>
        /// <returns>Requête paginée</returns>
        public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int start, int end)
        {
            return query.Skip(start).Take(end - start + 1);
        }
    }
}
