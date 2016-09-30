using System.Linq;
using DetachedQueryable.Test.Repo;
using Xunit;

namespace DetachedQueryable.Test
{

    public class DetachedQueryTest
    {
        [Fact]
        public void UnattachedQueries_ThrowExceptions()
        {
            Assert.Throws<DetachedQueryEvaluatedException>(() =>
            {
                DetachedQuery.Of<Foo>().ToList();
            });
        }
        [Fact]
        public void CanAttachAQuery()
        {
            var repo = new Repo.Repo();
            repo.Add<Foo>(Enumerable.Range(1,10).Select(x=>new Foo {Num = x}).ToArray());
            var query = DetachedQuery.Of<Foo>().Where(f => f.Num < 5).OrderBy(f=>f.Num);
            var attached = DetachedQuery.Attach(query, repo);
            var result = attached.ToList();
            Assert.Equal(4, result.Count);
            Assert.Equal(1, result[0].Num);
            Assert.Equal(4, result[3].Num);

        }

        [Fact]
        public void CanAttachComposedQuery()
        {
            var repo = new Repo.Repo();
            var bars = Enumerable.Range(1, 10)
                .Select(x => new Bar
                {
                    Name = x.ToString(),
                    Foos = Enumerable.Range(x,10).Select(y=>new Foo {Num = y}).ToList()
                }).ToList();

            var foos = bars.SelectMany(x => x.Foos).ToList();
            repo.AddRange(bars);
            repo.AddRange(foos);

            var fooQuery = DetachedQuery.Of<Foo>()
                .Where(f => f.Num < 5);
            var barQuery = DetachedQuery.Of<Bar>()
                .Where(b => b.Foos.Any(f => fooQuery.Contains(f)));
            var attached = DetachedQuery.Attach(barQuery, repo);
            var resultBars = attached.ToList();
        }
    }
}
