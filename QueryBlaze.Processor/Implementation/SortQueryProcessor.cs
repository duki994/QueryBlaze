﻿using QueryBlaze.Processor.Abstractions;
using QueryBlaze.Processor.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBlaze.Processor.Implementation
{
    public class SortQueryProcessor : ISortQueryProcessor
    {
        private readonly LambdaExpressionFactory _expressionFactory;

        public SortQueryProcessor(ISortProcessorOptionsProvider provider)
        {
            _expressionFactory = new LambdaExpressionFactory(provider);
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
                var lambda = _expressionFactory.CreateExpression(typeof(TEntity), info.PropertyName);

                var newExpression = Expression.Call(
                    typeof(Queryable),
                    info.GetOrderMethodName(),
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
                .Select(x => _expressionFactory.ParseNameAndOrder(x))
                .Select((x, index) => new SortInfo(x.propertyName, index == 0, x.descending))
                .ToList();
    }
}
