using QueryBlaze.Processor.Abstractions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace QueryBlaze.Processor
{
    public class ReflectionUtilities
    {
        private readonly ISortProcessorOptionsProvider _optionsProvider;

        public ReflectionUtilities(ISortProcessorOptionsProvider optionsProvider)
        {
            _optionsProvider = optionsProvider;
        }

        public LambdaExpression CreateExpression(Type elementType, PropertyInfo info)
        {
            var parameter = Expression.Parameter(elementType, "x");

            var body = Expression.Property(parameter, info);

            return Expression.Lambda(body, parameter);
        }

        public LambdaExpression CreateExpression(Type elementType, string propertyName)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

            var parameter = Expression.Parameter(elementType);

            var names = propertyName.Split(_optionsProvider.Provide().NestedPropSeparator);

            var pInfo = elementType.GetProperty(names[0], flags);
            var body = Expression.Property(parameter, pInfo);

            for (int i = 1; i < names.Length; i++)
            {
                pInfo = elementType.GetProperty(names[i], flags);
                if (pInfo is null)
                {
                    // TODO: Try resolving this 'break of control flow' via functional 'Option<T>' type instead of exceptions or 'Result<T>' type that includes error info
                    throw new BadMemberAccessException($"'{names[i]}' is not member of type '{body.Type}'. Full property name passed is '{propertyName}'");
                }

                body = Expression.Property(body, pInfo);
            }

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
