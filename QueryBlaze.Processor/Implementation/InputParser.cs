using QueryBlaze.Processor.Abstractions;
using System;
using System.Text.RegularExpressions;

namespace QueryBlaze.Processor.Implementation
{
    public class InputParser : IInputParser
    {
        private readonly ISortProcessorOptionsProvider _optionsProvider;

        public InputParser(ISortProcessorOptionsProvider optionsProvider)
        {
            _optionsProvider = optionsProvider;
        }

        public ParserResult ParseNameAndOrder(string sortPropertyParameter)
        {
            var opts = _optionsProvider.Provide();
            string name = sortPropertyParameter;

            var indicatorIndex = sortPropertyParameter.IndexOf(opts.DescendingIndicator, StringComparison.Ordinal);
            bool descending = indicatorIndex != -1;

            var regex = new Regex(opts.StripCharsPattern);
            name = regex.Replace(name, string.Empty);

            return new ParserResult(name, descending);
        }
    }
}
