using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace DetachedQueryable.NHibernate
{
    public class NhQueryableFactory : IQueryableFactory
    {
        private readonly ISession _session;
        public NhQueryableFactory(ISession session)
        {
            _session = session;
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return _session.Query<T>();
        }
    }
}
