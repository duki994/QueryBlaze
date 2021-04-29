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

            foreach (var name in names.Skip(1))
            {
                initial = initial.ThenProperty(name);
            }


            var final = initial.FinishAccess();
            var result = final.Build();

            if (!result.IsSuccessfull)
            {
                throw new BadMemberAccessException(string.Join('\n', result.Errors));
            }

            return result.Result;
        }

        public (string propertyName, bool descending) ParseNameAndOrder(string sortPropertyParameter)
        {
            var opts = _optionsProvider.Provide();
            string name = sortPropertyParameter;

            var indicatorIndex = sortPropertyParameter.IndexOf(opts.DescendingIndicator, StringComparison.Ordinal);
            bool descending = indicatorIndex != -1;

            var regex = new Regex(opts.StripCharsPattern);
            name = regex.Replace(name, string.Empty);

            return (name, descending);
        }
    }
}
