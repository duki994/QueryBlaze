using QueryBlaze.Processor.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBlaze.Processor.Implementation
{
    public class SortQueryProcessor : ISortQueryProcessor
    {
        private readonly SortProcessorOptions _options;
        private readonly ReflectionUtilities _reflectionUtilities;

        public SortQueryProcessor(ISortProcessorOptionsProvider provider)
        {
            _options = provider.Provide();
            _reflectionUtilities = new ReflectionUtilities(provider);
        }

        public IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> query, SortParams parameters)
        {
            if (!parameters.SortProperties.Any())
            {
                return query;
            }

            var infos = GetSortPropertyInfos<TEntity>(parameters);

            foreach (var info in infos)
            {
                LambdaExpression lambda = _reflectionUtilities.CreateExpression(typeof(TEntity), info.PropertyName);

                var newExpression = Expression.Call(
                    typeof(Queryable),
                    _reflectionUtilities.GetOrderMethodName(info),
                    new[] { typeof(TEntity), lambda.ReturnType },
                    query.Expression,
                    Expression.Quote(lambda)
                );

                query = query.Provider.CreateQuery<TEntity>(newExpression);
            }

            return query;
        }

        private ICollection<SortInfo> GetSortPropertyInfos<TEntity>(SortParams sortParams) =>
            sortParams.SortProperties
                .Select(x => _reflectionUtilities.GetPropertyNameAndSortOrder(x))
                .Select((x, index) => new SortInfo(x.propertyName, index == 0, x.descending))
                .ToList();
    }
}
