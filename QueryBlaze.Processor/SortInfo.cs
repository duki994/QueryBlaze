using System;
using System.Reflection;

namespace QueryBlaze.Processor
{
    public struct SortInfo : IEquatable<SortInfo>
    {
        public string PropertyName { get; set; }
        public bool IsFirst { get; }
        public bool IsDescending { get; }

        public SortInfo(string propertyName, bool isFirst, bool isDescending)
        {
            PropertyName = propertyName;
            IsFirst = isFirst;
            IsDescending = isDescending;
        }

        public override bool Equals(object? obj)
        {
            return obj is SortInfo info && Equals(info);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PropertyName, IsFirst, IsDescending);
        }

        public static bool operator ==(SortInfo left, SortInfo right)
        {
            return left.PropertyName == right.PropertyName
                && left.IsFirst == right.IsFirst
                && left.IsDescending == right.IsDescending;
        }

        public static bool operator !=(SortInfo left, SortInfo right)
        {
            return !(left == right);
        }

        public bool Equals(SortInfo other)
        {
            return this == other;
        }
    }
}
