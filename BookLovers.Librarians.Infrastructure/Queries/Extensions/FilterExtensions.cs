using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BookLovers.Librarians.Infrastructure.Queries.Extensions
{
    internal static class FilterExtensions
    {
        internal static Expression<Func<TicketReadModel, bool>> AndWithTitle(
            this Expression<Func<TicketReadModel, bool>> expr, string title)
        {
            return expr.AndIf(p => p.Title.ToUpper().StartsWith(title.ToUpper().Trim()), title != string.Empty);
        }

        internal static IQueryable<PromotionWaiterReadModel> SkipUnAvailable(
            this IQueryable<PromotionWaiterReadModel> query)
        {
            return query.Where(p =>
                p.PromotionAvailability == PromotionAvailability.Available.Value ||
                p.PromotionAvailability == PromotionAvailability.Promoted.Value);
        }
    }
}