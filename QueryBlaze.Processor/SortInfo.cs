using System;
using System.Reflection;

namespace QueryBlaze.Processor
{
    public struct SortInfo : IEquatable<SortInfo>
    {
        public PropertyInfo AccessorInfo { get; }
        public bool IsFirst { get; }
        public bool IsDescending { get; }

        public SortInfo(PropertyInfo info, bool isFirst, bool isDescending)
        {
            AccessorInfo = info;
            IsFirst = isFirst;
            IsDescending = isDescending;
        }

        public override bool Equals(object? obj)
        {
            return obj is SortInfo info && 
                info.AccessorInfo == AccessorInfo;
        }

        public override int GetHashCode()
        {
            return AccessorInfo.GetHashCode();
        }

        public static bool operator ==(SortInfo left, SortInfo right)
        {
            return left.AccessorInfo == right.AccessorInfo;
        }

        public static bool operator !=(SortInfo left, SortInfo right)
        {
            return !(left == right);
        }

        public bool Equals(SortInfo other)
        {
            return AccessorInfo == other.AccessorInfo;
        }
    }
}
