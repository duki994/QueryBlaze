namespace QueryBlaze.Processor
{
    public class SortProcessorOptionsProvider : ISortProcessorOptionsProvider
    {
        private readonly SortProcessorOptions? _options;

        public SortProcessorOptionsProvider()
        {

        }

        public SortProcessorOptionsProvider(SortProcessorOptions options)
        {
            _options = options;
        }

        public SortProcessorOptions Provide()
        {
            return _options ?? SortProcessorOptions.Default;
        }
    }
}
