using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Base.Infrastructure.Queries
{
    public static class FilterExtensions
    {
        public static IQueryable<TReadModel> WhereIf<TReadModel>(
            this IQueryable<TReadModel> query,
            Expression<Func<TReadModel, bool>> predicate,
            bool condition)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<TReadModel> WithId<TReadModel>(
            this IQueryable<TReadModel> query,
            int id)
            where TReadModel : class, IReadModel<TReadModel>
        {
            return query.Where(p => p.Id == id);
        }

        public static IQueryable<TReadModel> ActiveRecords<TReadModel>(
            this IQueryable<TReadModel> query)
            where TReadModel : class, IReadModel<TReadModel>
        {
            return query.Where(p => p.Status == AggregateStatus.Active.Value);
        }

        public static IOrderedQueryable<TReadModel> Paginate<TReadModel>(
            this IOrderedQueryable<TReadModel> query,
            int page,
            int count)
        {
            return query.Skip(page * count).Take(count) as IOrderedQueryable<TReadModel>;
        }

        public static string ToStringValueList(this IEnumerable<int> values)
        {
            var stringBuilder = new StringBuilder();
            foreach (var num in values)
                stringBuilder.Append(num).Append(',');

            return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
        }
    }
}