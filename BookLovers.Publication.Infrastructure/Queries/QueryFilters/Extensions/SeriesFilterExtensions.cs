using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions
{
    public static class SeriesFilterExtensions
    {
        internal static IQueryable<SeriesReadModel> FilterSeriesWithAuthorBooks(
            this IQueryable<SeriesReadModel> query, int id)
        {
            return query.Where(p => p.Books.Any(a => a.Authors.Any(authorId => authorId.Id == id)));
        }

        internal static Task<SeriesReadModel> GetSeriesWithBook(this IQueryable<SeriesReadModel> query, int bookId)
        {
            return query.SingleOrDefaultAsync(p => p.Books.Any(a => a.Id == bookId));
        }

        internal static Task<SeriesReadModel> FindSeriesWithExactTitle(
            this IQueryable<SeriesReadModel> query,
            string title)
        {
            return query.SingleOrDefaultAsync(p => p.Name.ToUpper() == title.ToUpper());
        }
    }
}