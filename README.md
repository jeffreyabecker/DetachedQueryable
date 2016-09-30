#DetachedQueryable
A set of libraries to make linq using programs easier to test.

##Basic Usage
```C#
  var query = DetachedQuery.Of<Cat>();
  query = query.Where(c=>c.Mother.Name == "Linda");
  if(sexRestriction != null)
  {
    query = query.Where(c=>c.Sex == sexRestriction.Value);
  }
  
  var kittens = DetachedQuery.Attach(query, myQueryableFactory).ToList();
```

## NHibernate
```C#
  var query = BuildQuery();
  var results = session.Attach(query).ToList();
```

## EntityFramework 6
```C#
  var query = BuildQuery();
  var results = dbContext.Attach(query).ToList();
```
