using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace QueryBlaze.Processor.Implementation
{
    public class CustomPropertyMapper
    {
        /// <summary>
        /// Represents mapping of property names of one type via <see cref="MapperKey"/> to name of property
        /// </summary>
        public IDictionary<MapperKey, string> SortNameToPropertyNameMap { get; } = new Dictionary<MapperKey, string>();
    }

    public struct MapperKey : IEquatable<MapperKey>
    {
        public string PropertyName { get; }
        public Type EntityType { get; }

        public MapperKey(string propertyName, Type entityType)
        {
            PropertyName = propertyName;
            EntityType = entityType;
        }


        public bool Equals([AllowNull] MapperKey other) => 
            other.EntityType == EntityType
                && string.Equals(other.PropertyName, PropertyName, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj) => obj is MapperKey other &&
                other.EntityType == EntityType && string.Equals(other.PropertyName, PropertyName, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => HashCode.Combine(PropertyName, EntityType);

        public override string? ToString()
        {
            return $"{EntityType.Name} + {PropertyName}";
        }
    }
}
