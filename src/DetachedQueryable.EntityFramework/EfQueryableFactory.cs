using System.Linq;

namespace DetachedQueryable.EntityFramework
{
    public class EfQueryableFactory : IQueryableFactory
    {
        private readonly DbContext _ctx;

        public EfQueryableFactory(DbContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return _ctx.Set<T>();
        }
    }
}
