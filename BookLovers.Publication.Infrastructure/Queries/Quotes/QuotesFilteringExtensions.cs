using System.Linq;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;

namespace BookLovers.Publication.Infrastructure.Queries.Quotes
{
    internal static class QuotesFilteringExtensions
    {
        private static IOrderedQueryable<QuoteReadModel> QuotesByLikes(
            this IQueryable<QuoteReadModel> query,
            bool descending)
        {
            if (!descending)
                return query.OrderBy(p => p.QuoteLikes.Count);

            return query.OrderByDescending(p => p.QuoteLikes.Count);
        }

        internal static IOrderedQueryable<QuoteReadModel> OrderQuotesBy(
            this IQueryable<QuoteReadModel> query,
            int order,
            bool descending = true)
        {
            var quotesOrder =
                QuotesAvailableOrders.Orders.SingleOrDefault(p => p.Value == order);

            if (quotesOrder == QuotesOrder.ByLikes)
                return query.QuotesByLikes(descending);

            if (quotesOrder == QuotesOrder.ById)
            {
                if (!descending)
                    return query.OrderBy(p => p.Id);
                return query.OrderByDescending(p => p.Id);
            }

            return query.OrderByDescending(p => p.Id);
        }

        internal static IQueryable<QuoteReadModel> WithBook(
            this IQueryable<QuoteReadModel> query,
            int bookId)
        {
            return query.Where(p => p.Book.Id == bookId);
        }

        internal static IQueryable<QuoteReadModel> WithAuthor(
            this IQueryable<QuoteReadModel> query,
            int authorId)
        {
            return query.Where(p => p.Author.Id == authorId);
        }
    }
}