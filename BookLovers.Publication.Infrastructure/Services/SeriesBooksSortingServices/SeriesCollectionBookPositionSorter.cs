using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using BookLovers.Publication.Infrastructure.Queries.Series;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices
{
    internal class SeriesCollectionBookPositionSorter : ISeriesCollectionSorter
    {
        private readonly PublicationsContext _context;

        public SeriesCollectionSortingType SortingType => SeriesCollectionSortingType.ByPosition;

        public SeriesCollectionBookPositionSorter(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> Sort(
            PaginatedSeriesCollectionQuery query)
        {
            var source = this._context.Books.Include(p => p.Series)
                .Where(p => p.Series.Id == query.SeriesId)
                .FilterBooksByTitle(query.Title);

            var queryDeferred = source.DeferredCount();

            IOrderedQueryable<BookReadModel> orderedQuery;
            if (!query.Descending)
                orderedQuery = source.OrderBy(p => p.PositionInSeries);
            else
                orderedQuery = source.OrderByDescending(p => p.PositionInSeries);

            var resultsQuery = orderedQuery.Paginate(query.Page, query.Count)
                .Select(s => s.Id).Future();

            var totalCount = await queryDeferred.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var paginatedResult = new PaginatedResult<int>(
                results,
                query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}