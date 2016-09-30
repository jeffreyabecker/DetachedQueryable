using System.Collections.Generic;
using System.Linq;

namespace DetachedQueryable.EnumerableExtensions
{
    public class EnumerableQueryableFactory<T> : IQueryableFactory
    {
        private readonly IEnumerable<T> _enumerable;
        public EnumerableQueryableFactory(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return (IQueryable<T>) _enumerable.AsQueryable();
        }
    }

    public static class EnumerableExtensions
    {
        public static IQueryable<T> Attach<T>(this IEnumerable<T> ctx, IQueryable<T> detachedQuery)
        {
            return DetachedQuery.Attach(detachedQuery, new EnumerableQueryableFactory<T>(ctx));
        }


    }
}
