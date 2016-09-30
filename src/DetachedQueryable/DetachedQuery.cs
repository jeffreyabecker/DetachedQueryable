using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DetachedQueryable
{
    public static class DetachedQuery
    {

        public static IQueryable<T> Attach<T>(IQueryable<T> detachedQueryable, IQueryableFactory queryableFactory)
        {
            var rewriter = new DetachedQueryRewriter(queryableFactory);
            var newExpression = rewriter.Visit(detachedQueryable.Expression);
            return rewriter.LastProvider.CreateQuery<T>(newExpression);
        }
        public static IQueryable<T> Of<T>() where T: class 
        {
            return new DetachedQueryImpl<T>();
        }

        private abstract class DetachedQueryBase
        {
            public abstract IQueryable GetQueryable(IQueryableFactory queryableFactory);
        }
        private class DetachedQueryImpl<T> : DetachedQueryBase, IOrderedQueryable<T>, IQueryProvider where T:class
        {
            public DetachedQueryImpl(Expression expression)
            {
                Expression = expression;
            }
            public DetachedQueryImpl()
            {
                Expression = Expression.Constant(this);
            }
            public override IQueryable GetQueryable(IQueryableFactory queryableFactory)
            {
                return queryableFactory.GetQueryable<T>();
            }
            #region IOrderedQueryable Members

            public IEnumerator<T> GetEnumerator()
            {
                throw new DetachedQueryEvaluatedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }

            public Type ElementType { get { return typeof(T); } }

            public System.Linq.Expressions.Expression Expression { get; private set; }

            public IQueryProvider Provider { get { return this; } }
            #endregion

            #region IQueryProvider Members
            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                if (expression == null)
                    throw new ArgumentNullException("expression");
                if (!(typeof(IQueryable<TElement>).GetTypeInfo().IsAssignableFrom(expression.Type.GetTypeInfo())))
                    throw new ArgumentException("Invalid Expression", "expression");

                var resultType = typeof (DetachedQueryImpl<>).MakeGenericType(typeof (TElement));
                return (IQueryable<TElement>) Activator.CreateInstance(resultType, expression);
            }

            public IQueryable CreateQuery(Expression expression)
            {
                throw new NotImplementedException("Shouldnt need non-typesafe method");
            }

            public TResult Execute<TResult>(Expression expression) { throw new DetachedQueryEvaluatedException(); }

            public object Execute(Expression expression) { throw new DetachedQueryEvaluatedException(); }
            #endregion
        }

        private class DetachedQueryRewriter : ExpressionVisitor
        {

            private IQueryableFactory _queryableFactory;
            public DetachedQueryRewriter(IQueryableFactory queryableFactory)
            {
                _queryableFactory = queryableFactory;
            }
            protected override Expression VisitConstant(ConstantExpression node)
            {
                var detachedQuery = node.Value as DetachedQueryBase;
                if (detachedQuery != null)
                {
                    var newQueryable = detachedQuery.GetQueryable(_queryableFactory);
                    LastProvider = newQueryable.Provider;
                    return Expression.Constant(newQueryable);
                }
                return base.VisitConstant(node);
            }

            //Sometimes the compiler packs nested queryables into fields on constants.
            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Type.GetTypeInfo().IsGenericType && node.Type.GetTypeInfo().GetGenericTypeDefinition() == typeof (IQueryable<>) && node.Expression.NodeType == ExpressionType.Constant)
                {
                    var objectMember = Expression.Convert(node, typeof(object));

                    var getterLambda = Expression.Lambda<Func<object>>(objectMember);

                    var querableInstance = getterLambda.Compile()();

                    var detachedQuery = querableInstance as DetachedQueryBase;
                    if (detachedQuery != null)
                    {
                        var newQueryable = detachedQuery.GetQueryable(_queryableFactory);
                        LastProvider = newQueryable.Provider;
                        return Expression.Constant(newQueryable);
                    }

                    
                }
                return base.VisitMember(node);
            }

            public IQueryProvider LastProvider { get; set; }
        }
    }
}
