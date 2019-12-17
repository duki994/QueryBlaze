using System;
using System.Diagnostics.CodeAnalysis;

namespace QueryBlaze.Processor
{
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
