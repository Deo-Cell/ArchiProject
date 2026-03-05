using System.Linq.Expressions;
using System.Reflection;

namespace Archi.Library.Search
{
    /// <summary>
    /// Extension pour appliquer dynamiquement une recherche avec wildcards sur IQueryable
    /// </summary>
    public static class SearchExtensions
    {
        public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, List<SearchQuery> searches)
        {
            if (searches == null || !searches.Any())
                return query;

            foreach (var search in searches)
            {
                query = ApplySingleSearch(query, search);
            }

            return query;
        }

        private static IQueryable<T> ApplySingleSearch<T>(IQueryable<T> query, SearchQuery search)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var propertyInfo = typeof(T).GetProperty(
                search.PropertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null || propertyInfo.PropertyType != typeof(string))
            {
                // On ne supporte la recherche (Contains/StartsWith) que sur les strings pour l'instant
                return query;
            }

            var property = Expression.Property(parameter, propertyInfo);
            var cleanTerm = search.CleanTerm;

            // Constante de recherche (insensible à la casse si possible, mais avec SQLite LINQ on fait souvent sans StringComparison directement)
            var constant = Expression.Constant(cleanTerm, typeof(string));

            Expression? callExpression = null;

            // Déterminer la méthode à appeler en fonction des wildcards
            var stringType = typeof(string);

            if (search.StartsWithWildcard && search.EndsWithWildcard)
            {
                // *terme* -> Contains
                var containsMethod = stringType.GetMethod("Contains", new[] { typeof(string) });
                if (containsMethod != null)
                {
                    callExpression = Expression.Call(property, containsMethod, constant);
                }
            }
            else if (search.StartsWithWildcard)
            {
                // *terme -> EndsWith
                var endsWithMethod = stringType.GetMethod("EndsWith", new[] { typeof(string) });
                if (endsWithMethod != null)
                {
                    callExpression = Expression.Call(property, endsWithMethod, constant);
                }
            }
            else if (search.EndsWithWildcard)
            {
                // terme* -> StartsWith
                var startsWithMethod = stringType.GetMethod("StartsWith", new[] { typeof(string) });
                if (startsWithMethod != null)
                {
                    callExpression = Expression.Call(property, startsWithMethod, constant);
                }
            }
            else
            {
                // terme (exact) -> On pourrait faire Equal ou Contains par défaut.
                // Par convention de recherche, si pas d'étoile on peut faire Contains quand même ou Exact.
                // Faisons Exact si pas d'étoile :
                var equalsMethod = stringType.GetMethod("Equals", new[] { typeof(string) });
                if (equalsMethod != null)
                {
                    callExpression = Expression.Call(property, equalsMethod, constant);
                }
            }

            if (callExpression != null)
            {
                // Gestion des strings null: x.Prop != null && x.Prop.Method(term)
                var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
                var condition = Expression.AndAlso(nullCheck, callExpression);

                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                query = query.Where(lambda);
            }

            return query;
        }
    }
}
