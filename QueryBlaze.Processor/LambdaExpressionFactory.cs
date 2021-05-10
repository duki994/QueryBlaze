using Infrastructure.Expressions;
using QueryBlaze.Processor.Abstractions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace QueryBlaze.Processor
{
    public class LambdaExpressionFactory
    {
        private readonly ISortProcessorOptionsProvider _optionsProvider;

        public LambdaExpressionFactory(ISortProcessorOptionsProvider optionsProvider)
        {
            _optionsProvider = optionsProvider;
        }

        public LambdaExpression CreateExpression(Type elementType, string propertyName)
        {

            var names = propertyName.Split(_optionsProvider.Provide().NestedPropSeparator);
            var initial = LambdaExpressionBuilder.Create()
                .AddParameter(elementType)
                .AccessProperty(names[0]);

            initial = names.Skip(1).Aggregate(initial, (builder, prop) => builder.ThenProperty(prop));

            var result = initial
                .FinishAccess()
                .Build();

            if (!result.IsSuccessfull)
            {
                throw new BadMemberAccessException(string.Join('\n', result.Errors));
            }

            return result.Result;
        }
    }
}
