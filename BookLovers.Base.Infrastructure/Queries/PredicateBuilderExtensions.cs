using System;
using System.Linq.Expressions;
using LinqKit;

namespace BookLovers.Base.Infrastructure.Queries
{
    public static class PredicateBuilderExtensions
    {
        public static Expression<Func<T, bool>> OrIf<T>(
            this Expression<Func<T, bool>> firstExpression,
            Expression<Func<T, bool>> secondExpression,
            bool condition)
        {
            return condition ? firstExpression.Or(secondExpression) : firstExpression;
        }

        public static Expression<Func<T, bool>> AndIf<T>(
            this Expression<Func<T, bool>> firstExpression,
            Expression<Func<T, bool>> secondExpression,
            bool condition)
        {
            return condition ? firstExpression.And(secondExpression) : firstExpression;
        }
    }
}