using System.Linq;

namespace DetachedQueryable
{
    public interface IQueryableFactory
    {
        IQueryable<T> GetQueryable<T>() where T : class;
    }
}