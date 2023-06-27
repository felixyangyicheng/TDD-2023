using System;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace TDD.Test.Helpers
{
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var resultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = _inner.Execute(expression);
            var convertedResult = typeof(Enumerable)
                .GetMethod("ToListAsync")
                .MakeGenericMethod(resultType)
                .Invoke(null, new[] { executionResult, cancellationToken });

            return (TResult)convertedResult;
        }

        public TResult ExecuteAsync<TResult>(Expression expression)
        {
            return ExecuteAsync<TResult>(expression, CancellationToken.None);
        }
    }

}

