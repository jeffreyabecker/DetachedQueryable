using System;
using System.Collections.Generic;
using System.Linq;

namespace DetachedQueryable.Test.Repo
{
    public class Repo : IQueryableFactory
    {
        private readonly Dictionary<Type, object> _objects = new Dictionary<Type, object>();

        public void Add<T>(params T[] items)
        {
            AddRange(items);
        }
        public void Add<T>(T item)
        {
            AddRange(new[] {item});
        }



        public IQueryable<T> GetQueryable<T>() where T : class
        {
            var type = typeof(T);
            if (!_objects.ContainsKey(type))
            {
                _objects[type] = new HashSet<T>();
            }
            return ((HashSet<T>)_objects[type]).AsQueryable();
        }

        public void AddRange<T>(IEnumerable<T> items)
        {
            var type = typeof(T);
            if (!_objects.ContainsKey(type))
            {
                _objects[type] = new HashSet<T>();
            }
            var set = ((HashSet<T>) _objects[type]);
            foreach (var item in items)
            {
                set.Add(item);
            }
        }
    }
}