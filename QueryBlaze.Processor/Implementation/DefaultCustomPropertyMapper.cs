using QueryBlaze.Processor.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QueryBlaze.Processor.Implementation
{
    public class DefaultCustomPropertyMapper : ICustomPropertyMapper
    {
        /// <summary>
        /// Represents custom mapping of property names of one type via <see cref="MapperKey"/> to name of property of target type
        /// </summary>
        public IReadOnlyDictionary<MapperKey, string> SortNameToPropertyNameMap { get; } = new ReadOnlyDictionary<MapperKey, string>(new Dictionary<MapperKey, string>());
    }
}
