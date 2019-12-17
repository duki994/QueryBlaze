namespace QueryBlaze.Processor
{
    public class SortProcessorOptions
    {
        public static SortProcessorOptions Default { get; } = new SortProcessorOptions();

        public char[] StripChars { get; set; } = new[] { '+', '%', ' ' };
        public string DescendingIndicator { get; set; } = "-";
    }
}
