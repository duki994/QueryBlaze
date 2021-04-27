using System.Collections.Generic;

namespace QueryBlaze.Processor.Abstractions
{
    public interface ICustomPropertyMapper
    {
        IReadOnlyDictionary<MapperKey, string> SortNameToPropertyNameMap { get; }
    }
}