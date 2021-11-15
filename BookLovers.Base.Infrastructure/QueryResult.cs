using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Validation;

namespace BookLovers.Base.Infrastructure
{
    public class QueryResult<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public TQuery Query { get; }

        public TResult Value { get; }

        public bool HasErrors => QueryErrors.Any();

        public IEnumerable<ValidationError> QueryErrors { get; }

        protected QueryResult(TQuery query)
        {
            Query = query;
            QueryErrors = Enumerable.Empty<ValidationError>();
        }

        protected QueryResult(TQuery query, TResult value)
        {
            Query = query;
            Value = value;
            QueryErrors = Enumerable.Empty<ValidationError>();
        }

        protected QueryResult(TQuery query, IEnumerable<ValidationError> queryErrors)
        {
            Query = query;
            QueryErrors = queryErrors;
        }

        public static QueryResult<TQuery, TResult> ValidatedQuery(TQuery query)
        {
            return new QueryResult<TQuery, TResult>(query);
        }

        public static QueryResult<TQuery, TResult> ValidQuery(TQuery query, TResult result)
        {
            return new QueryResult<TQuery, TResult>(query, result);
        }

        public static QueryResult<TQuery, TResult> InValidQuery(
            TQuery query,
            IEnumerable<ValidationError> errors)
        {
            return new QueryResult<TQuery, TResult>(query, errors);
        }
    }
}