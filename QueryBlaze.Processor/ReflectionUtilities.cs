using QueryBlaze.Processor.Abstractions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace QueryBlaze.Processor
{
    internal class ReflectionUtilities
    {
        private readonly ISortProcessorOptionsProvider _optionsProvider;

        public ReflectionUtilities(ISortProcessorOptionsProvider optionsProvider)
        {
            _optionsProvider = optionsProvider;
        }

        /// <summary>
        /// Finds property info regardless of <paramref name="propertyName"/> casing
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public PropertyInfo? GetPropertyInfo(Type elementType, string propertyName)
        {
            return elementType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        }

        public LambdaExpression CreateExpression(Type elementType, PropertyInfo info)
        {
            var parameter = Expression.Parameter(elementType, "x");

            var body = Expression.Property(parameter, info);

            return Expression.Lambda(body, parameter);
        }

        public (string propertyName, bool descending) GetPropertyNameAndSortOrder(string sortPropertyParameter)
        {
            var opts = _optionsProvider.Provide();
            string name = sortPropertyParameter;

            var indicatorIndex = sortPropertyParameter.IndexOf(opts.DescendingIndicator, StringComparison.Ordinal);
            bool descending = indicatorIndex != -1;

            var regex = new Regex(opts.StripCharsPattern);
            name = regex.Replace(name, string.Empty);

            return (name, descending);
        }

        public string GetOrderMethodName(SortInfo sortInfo)
        {
            if (sortInfo.IsFirst)
            {
                return sortInfo.IsDescending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);
            }

            return sortInfo.IsDescending ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy);
        }
    }
}
