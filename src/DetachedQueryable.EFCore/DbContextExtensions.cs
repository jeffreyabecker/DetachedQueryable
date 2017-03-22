using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DetachedQueryable.EFCore
{
    public static class DbContextExtensions
    {
        public static IQueryable<T> Attach<T>(this DbContext ctx, IQueryable<T> detachedQuery)
        {
            return DetachedQuery.Attach(detachedQuery, new EfQueryableFactory(ctx));
        }


    }
}