using QueryBlaze.Processor.Abstractions;

namespace QueryBlaze.Processor.Implementation
{
    public class DefaultSortProcessorOptionsProvider : ISortProcessorOptionsProvider
    {
        private readonly SortProcessorOptions _options;

        public DefaultSortProcessorOptionsProvider()
        {
            _options = new SortProcessorOptions();
        }

        public SortProcessorOptions Provide()
        {
            return _options;
        }
    }
}
