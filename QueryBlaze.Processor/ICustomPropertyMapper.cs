using System.Collections.Generic;

namespace QueryBlaze.Processor
{
    public interface ICustomPropertyMapper
    {
        IDictionary<MapperKey, string> SortNameToPropertyNameMap { get; }
    }
}