using System.Reflection;

namespace Archi.Library.PartialResponse
{
    /// <summary>
    /// Extension pour projeter dynamiquement des objets en mémoire
    /// afin de ne retourner que les champs sélectionnés.
    /// </summary>
    public static class DynamicProjection
    {
        /// <summary>
        /// Applique une projection dynamique pour ne conserver que les champs spécifiés.
        /// À utiliser de préférence APRES la pagination (sur une collection en mémoire)
        /// pour des raisons de performance.
        /// </summary>
        public static IEnumerable<object> ApplyProjection<T>(this IEnumerable<T> source, List<string> fields)
        {
            if (fields == null || !fields.Any())
                return (IEnumerable<object>)source;

            // Récupérer uniquement les propriétés publiques qui correspondent aux champs demandés (insensible à la casse)
            var propertyInfos = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                .Where(p => fields.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (!propertyInfos.Any())
            {
                // Si aucun champ valide n'est trouvé, on retourne la source intacte
                return (IEnumerable<object>)source;
            }

            return source.Select(item =>
            {
                // Un dictionnaire sera sérialisé en JSON comme un vrai objet JavaScript
                var dict = new Dictionary<string, object?>();

                foreach (var prop in propertyInfos)
                {
                    // Utiliser le vrai nom de la propriété (ex: "Name" même si "name" a été demandé)
                    // pour la constance, ou utiliser camelCase si nécessaire. On garde le format du modèle:
                    dict[prop.Name] = prop.GetValue(item);
                }

                return dict;
            });
        }
    }
}
