using System.Linq;
using NHibernate;
using NHibernate.Impl;

namespace DetachedQueryable.NHibernate
{
    public static class SessionExtensions
    {
        public static IQueryable<T> Attach<T>(this ISession ctx, IQueryable<T> detachedQuery)
        {
            return DetachedQuery.Attach(detachedQuery, new NhQueryableFactory(ctx));
        }
    }
}