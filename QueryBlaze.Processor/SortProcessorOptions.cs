namespace QueryBlaze.Processor
{
    public class SortProcessorOptions
    {
        public string StripCharsPattern { get; set; } = @"[\+\-\%]";
        public char NestedPropSeparator { get; set; } = '.';
        public string DescendingIndicator { get; set; } = "-";
    }
}
