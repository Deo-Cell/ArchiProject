using System.Linq.Expressions;
using System.Reflection;

namespace Archi.Library.Filters
{
    /// <summary>
    /// Classe permettant de construire dynamiquement des expressions LINQ
    /// pour appliquer les filtres sur IQueryable
    /// </summary>
    public static class FilterExpression
    {
        public static IQueryable<T> ApplyFilters<T>(
            this IQueryable<T> query,
            List<Filter> filters)
        {
            if (filters == null || !filters.Any())
                return query;

            foreach (var filter in filters)
            {
                query = ApplyFilter(query, filter);
            }
            return query;
        }

        private static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, Filter filter)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            // Trouver la propriété en ignorant la casse (ex: "price" -> "Price")
            var propertyInfo = typeof(T).GetProperty(
                filter.PropertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                // Propriété non trouvée sur le modèle, on ignore silencieusement ce filtre
                return query;
            }

            var property = Expression.Property(parameter, propertyInfo);
            Expression? predicate = null;

            try
            {
                switch (filter.Type)
                {
                    case FilterType.Exact:
                        predicate = BuildExactFilter(property, filter.Values[0]);
                        break;
                    case FilterType.Multiple:
                        predicate = BuildMultipleFilter(property, filter.Values);
                        break;
                    case FilterType.Range:
                        predicate = BuildRangeFilter(property, filter.Values[0], filter.Values[1]);
                        break;
                    case FilterType.GreaterThan:
                        predicate = BuildGreaterThanFilter(property, filter.Values[0]);
                        break;
                    case FilterType.LessThan:
                        predicate = BuildLessThanFilter(property, filter.Values[0]);
                        break;
                }
            }
            catch (Exception)
            {
                // En cas d'erreur de conversion de type (ex: passer "abc" pour un champ Price), 
                // on ignore simplement le filtre plutôt que de crasher.
                return query;
            }

            if (predicate != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        private static Expression BuildExactFilter(MemberExpression property, string value)
        {
            var constant = ConvertToPropertyType(property.Type, value);
            return Expression.Equal(property, constant);
        }

        private static Expression BuildMultipleFilter(MemberExpression property, List<string> values)
        {
            Expression? combined = null;

            foreach (var value in values)
            {
                var constant = ConvertToPropertyType(property.Type, value);
                var equals = Expression.Equal(property, constant);

                combined = combined == null ? equals : Expression.OrElse(combined, equals);
            }

            return combined!;
        }

        private static Expression BuildRangeFilter(MemberExpression property, string min, string max)
        {
            var minConstant = ConvertToPropertyType(property.Type, min);
            var maxConstant = ConvertToPropertyType(property.Type, max);

            var greaterThan = Expression.GreaterThanOrEqual(property, minConstant);
            var lessThan = Expression.LessThanOrEqual(property, maxConstant);

            return Expression.AndAlso(greaterThan, lessThan);
        }

        private static Expression BuildGreaterThanFilter(MemberExpression property, string value)
        {
            var constant = ConvertToPropertyType(property.Type, value);
            return Expression.GreaterThanOrEqual(property, constant);
        }

        private static Expression BuildLessThanFilter(MemberExpression property, string value)
        {
            var constant = ConvertToPropertyType(property.Type, value);
            return Expression.LessThanOrEqual(property, constant);
        }

        private static ConstantExpression ConvertToPropertyType(Type propertyType, string value)
        {
            // Gérer les types Nullable (ex: double?, DateTime?)
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            object? convertedValue = null;

            if (underlyingType == typeof(string))
            {
                // Sqlite LIKE or StringComparison can be tricky. Pour l'exact on fait juste un == 
                convertedValue = value;
            }
            else if (underlyingType == typeof(int))
            {
                convertedValue = int.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (underlyingType == typeof(double))
            {
                convertedValue = double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (underlyingType == typeof(decimal))
            {
                convertedValue = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (underlyingType == typeof(bool))
            {
                convertedValue = bool.Parse(value);
            }
            else if (underlyingType == typeof(DateTime))
            {
                convertedValue = DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind);
            }
            else
            {
                throw new NotSupportedException($"Type {propertyType} non supporté pour les filtres");
            }

            // Retourner la constante castée correctement pour le type cible
            return Expression.Constant(convertedValue, propertyType);
        }
    }
}
