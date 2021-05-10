namespace QueryBlaze.Processor.Abstractions
{
    public interface IInputParser
    {
        ParserResult ParseNameAndOrder(string sortPropertyParameter);
    }
}