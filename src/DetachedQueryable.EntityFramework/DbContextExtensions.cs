using System.Linq;

namespace DetachedQueryable.EntityFramework
{
    public static class DbContextExtensions
    {
        public static IQueryable<T> Attach<T>(this DbContext ctx, IQueryable<T> detachedQuery)
        {
            return DetachedQuery.Attach(detachedQuery, new EfQueryableFactory(ctx));
        }

        public static IQueryable<T> Search<T>(this DbContext ctx, ISearch<T> search)
        {
            return Attach(ctx, search.Spec);
        }
    }
}