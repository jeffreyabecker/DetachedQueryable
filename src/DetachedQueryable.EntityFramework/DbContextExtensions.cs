using System.Data.Entity;
using System.Linq;

namespace DetachedQueryable.EntityFramework
{
    public static class DbContextExtensions
    {
        public static IQueryable<T> Attach<T>(this DbContext ctx, IQueryable<T> detachedQuery)
        {
            return DetachedQuery.Attach(detachedQuery, new EfQueryableFactory(ctx));
        }


    }
}