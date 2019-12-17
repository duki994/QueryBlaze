using System.Collections.Generic;

namespace QueryBlaze.Processor.Implementation
{
    public class CustomPropertyMapper : ICustomPropertyMapper
    {
        /// <summary>
        /// Represents custom mapping of property names of one type via <see cref="MapperKey"/> to name of property of target type
        /// </summary>
        public IDictionary<MapperKey, string> SortNameToPropertyNameMap { get; } = new Dictionary<MapperKey, string>();
    }
}
