namespace QueryBlaze.Processor
{
    public class ParserResult
    {
        public string PropertyName { get; }
        public bool Descending { get; }

        public ParserResult(string propertyName, bool descending)
        {
            PropertyName = propertyName;
            Descending = descending;
        }
    }
}
