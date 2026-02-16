using System.Linq.Expressions;

namespace Archi.Library.Sorting
{
    /// <summary>
    /// Extensions pour appliquer le tri dynamique sur les requêtes LINQ
    /// </summary>
    public static class SortingExtensions
    {
        /// <summary>
        /// Applique le tri dynamique sur une requête LINQ
        /// </summary>
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="query">Requête LINQ</param>
        /// <param name="sortings">Liste des tris à appliquer</param>
        /// <returns>Requête triée</returns>
        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            List<(string PropertyName, bool IsAscending)> sortings)
        {
            if (sortings == null || !sortings.Any())
                return query;

            IOrderedQueryable<T>? orderedQuery = null;

            foreach (var (propertyName, isAscending) in sortings)
            {
                var parameter = Expression.Parameter(typeof(T), "x");

                // Récupérer la propriété (insensible à la casse)
                var property = typeof(T).GetProperty(
                    propertyName,
                    System.Reflection.BindingFlags.IgnoreCase |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance
                );

                if (property == null)
                {
                    // Ignorer les propriétés inexistantes
                    continue;
                }

                var propertyAccess = Expression.Property(parameter, property);
                var lambda = Expression.Lambda(propertyAccess, parameter);

                string methodName;
                if (orderedQuery == null)
                {
                    // Premier tri : OrderBy ou OrderByDescending
                    methodName = isAscending ? "OrderBy" : "OrderByDescending";
                    orderedQuery = (IOrderedQueryable<T>)typeof(Queryable)
                        .GetMethods()
                        .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                        .MakeGenericMethod(typeof(T), property.PropertyType)
                        .Invoke(null, new object[] { query, lambda })!;
                }
                else
                {
                    // Tris suivants : ThenBy ou ThenByDescending
                    methodName = isAscending ? "ThenBy" : "ThenByDescending";
                    orderedQuery = (IOrderedQueryable<T>)typeof(Queryable)
                        .GetMethods()
                        .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                        .MakeGenericMethod(typeof(T), property.PropertyType)
                        .Invoke(null, new object[] { orderedQuery, lambda })!;
                }
            }

            return orderedQuery ?? query;
        }
    }
}
