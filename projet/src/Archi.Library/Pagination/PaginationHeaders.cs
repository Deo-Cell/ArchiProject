using Microsoft.AspNetCore.Http;

namespace Archi.Library.Pagination
{
    /// <summary>
    /// Helper pour générer les headers HTTP de pagination
    /// </summary>
    public class PaginationHeaders
    {
        /// <summary>
        /// Ajoute les headers de pagination à la réponse HTTP
        /// </summary>
        /// <param name="response">Réponse HTTP</param>
        /// <param name="start">Index de début</param>
        /// <param name="end">Index de fin (ajusté au total)</param>
        /// <param name="total">Nombre total d'éléments</param>
        /// <param name="baseUrl">URL de base de la requête</param>
        /// <param name="maxPerPage">Nombre maximum d'éléments par page (défaut: 50)</param>
        public static void AddPaginationHeaders(
            HttpResponse response,
            int start,
            int end,
            int total,
            string baseUrl,
            int maxPerPage = 50)
        {
            // Content-Range: items 0-24/100
            // Indique les éléments retournés et le total
            response.Headers.Append("Content-Range", $"items {start}-{end}/{total}");

            // Accept-Range: items 50
            // Indique que l'API supporte la pagination et le max par page
            response.Headers.Append("Accept-Range", $"items {maxPerPage}");

            // Link: <url>; rel="first", <url>; rel="prev", <url>; rel="next", <url>; rel="last"
            // Fournit les liens de navigation
            var links = new List<string>();

            // First page
            links.Add($"<{baseUrl}?range=0-{Math.Min(maxPerPage - 1, total - 1)}>; rel=\"first\"");

            // Previous page
            if (start > 0)
            {
                int prevStart = Math.Max(0, start - maxPerPage);
                int prevEnd = start - 1;
                links.Add($"<{baseUrl}?range={prevStart}-{prevEnd}>; rel=\"prev\"");
            }

            // Next page
            if (end < total - 1)
            {
                int nextStart = end + 1;
                int nextEnd = Math.Min(total - 1, nextStart + maxPerPage - 1);
                links.Add($"<{baseUrl}?range={nextStart}-{nextEnd}>; rel=\"next\"");
            }

            // Last page
            if (total > 0)
            {
                int lastStart = Math.Max(0, ((total - 1) / maxPerPage) * maxPerPage);
                int lastEnd = total - 1;
                links.Add($"<{baseUrl}?range={lastStart}-{lastEnd}>; rel=\"last\"");
            }

            response.Headers.Append("Link", string.Join(", ", links));
        }
    }
}
