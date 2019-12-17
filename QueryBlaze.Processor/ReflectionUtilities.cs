using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryBlaze.Processor
{
    public static class ReflectionUtilities
    {
        /// <summary>
        /// Finds property info regardless of <paramref name="propertyName"/> casing
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo? GetPropertyInfo(Type elementType, string propertyName)
        {
            return elementType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        }

        public static LambdaExpression CreateExpression(Type elementType, PropertyInfo info)
        {
            var parameter = Expression.Parameter(elementType, "x");

            var body = Expression.Property(parameter, info);

            return Expression.Lambda(body, parameter);
        }

        public static (string propertyName, bool descending) GetPropertyNameAndSortOrder(string sortPropertyParameter, string descendingIndicator)
        {
            string name = sortPropertyParameter;

            var indicatorIndex = sortPropertyParameter.IndexOf(descendingIndicator, StringComparison.Ordinal);
            bool descending = indicatorIndex != -1;
            if (descending)
            {
                name = sortPropertyParameter.Substring(0, indicatorIndex);
            }

            return (name, descending);
        }

        public static string GetOrderMethodName(SortInfo sortInfo)
        {
            if (sortInfo.IsFirst)
            {
                return sortInfo.IsDescending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);
            }

            return sortInfo.IsDescending ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy);
        }
    }
}
