﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBlaze.Processor.Implementation
{
    public class SortQueryProcessor : ISortQueryProcessor
    {
        private readonly SortProcessorOptions _options;

        public SortQueryProcessor(ISortProcessorOptionsProvider provider)
        {
            _options = provider.Provide();
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
                LambdaExpression lambda = ReflectionUtilities.CreateExpression(typeof(TEntity), info.AccessorInfo);

                var newExpression = Expression.Call(
                    typeof(Queryable),
                    ReflectionUtilities.GetOrderMethodName(info),
                    new[] { typeof(TEntity), info.AccessorInfo.PropertyType },
                    query.Expression,
                    Expression.Quote(lambda)
                );

                query = query.Provider.CreateQuery<TEntity>(newExpression);
            }

            return query;
        }

        private ICollection<SortInfo> GetSortPropertyInfos<TEntity>(SortParams sortParams) =>
            sortParams.SortProperties
                .Select(x => ReflectionUtilities.GetPropertyNameAndSortOrder(x, _options.DescendingIndicator))
                .Select(x =>
                (
                    propertyInfo: ReflectionUtilities.GetPropertyInfo(typeof(TEntity), x.propertyName),
                    x.descending
                ))
                .Where(tuple => tuple.propertyInfo != null)
                .Select((x, index) => new SortInfo(x.propertyInfo!, index == 0, x.descending))
                .ToList();
    }
}