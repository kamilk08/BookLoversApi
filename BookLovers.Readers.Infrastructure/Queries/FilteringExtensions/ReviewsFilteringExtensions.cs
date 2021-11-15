using System.Linq;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Queries.FilteringExtensions
{
    internal static class ReviewsFilteringExtensions
    {
        internal static IQueryable<ReviewReadModel> WithContent(
            this IQueryable<ReviewReadModel> query)
        {
            return query.Where(p => p.Review.Length > 0 && p.Review != default(string));
        }

        internal static string LimitReviewSize(this string review, int reviewSize) =>
            review.Length <= reviewSize ? review : review.Take(reviewSize).ToString();
    }
}