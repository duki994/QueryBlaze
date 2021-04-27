using System.Linq;

namespace QueryBlaze.Processor.Abstractions
{
    public interface ISortQueryProcessor
    {
        IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> query, SortParams parameters);
    }
}