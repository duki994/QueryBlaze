using System.Linq;

namespace QueryBlaze.Processor.Extensions
{
    public static class SortInfoExtensions
    {
        public static string GetOrderMethodName(this SortInfo sortInfo)
        {
            if (sortInfo.IsFirst)
            {
                return sortInfo.IsDescending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);
            }

            return sortInfo.IsDescending ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy);
        }
    }
}
