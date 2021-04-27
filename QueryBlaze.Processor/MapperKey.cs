using System;
using System.Diagnostics.CodeAnalysis;

namespace QueryBlaze.Processor
{
    public struct MapperKey : IEquatable<MapperKey>
    {
        public string SortPropertyName { get; }
        public Type EntityType { get; }

        public MapperKey(string propertyName, Type entityType)
        {
            SortPropertyName = propertyName;
            EntityType = entityType;
        }


        public bool Equals([AllowNull] MapperKey other) =>
            other.EntityType == EntityType
                && string.Equals(other.SortPropertyName, SortPropertyName, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj) => obj is MapperKey other &&
                other.EntityType == EntityType && string.Equals(other.SortPropertyName, SortPropertyName, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => HashCode.Combine(SortPropertyName, EntityType);

        public override string? ToString()
        {
            return $"{EntityType.Name} + {SortPropertyName}";
        }

        public void Deconstruct(out string propertyName, out Type entityType)
        {
            propertyName = SortPropertyName;
            entityType = EntityType;
        }

    }
}
