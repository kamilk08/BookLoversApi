using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions
{
    public static class PublisherFilterExtensions
    {
        internal static IQueryable<PublisherReadModel> FilterPublishersByTitle(
            this IQueryable<PublisherReadModel> query, string title)
        {
            return query.WhereIf(
                p => p.Publisher.Trim().ToUpper()
                    .StartsWith(title.Trim().ToUpper()),
                !title.IsEmpty());
        }

        internal static Task<PublisherReadModel> FindPublisherWithExactName(
            this IQueryable<PublisherReadModel> query, string cycleName)
        {
            return query.SingleOrDefaultAsync(p => p.Publisher.ToUpper() == cycleName.Trim().ToUpper());
        }

        internal static Task<PublisherReadModel> FindPublisherWithBook(
            this IQueryable<PublisherReadModel> query,
            int bookId)
        {
            return query.SingleOrDefaultAsync(p => p.Books.Any(a => a.Id == bookId));
        }
    }
}