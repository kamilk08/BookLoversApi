using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices
{
    internal class PublisherCollectionTitleSorter : IPublisherCollectionSorter
    {
        private readonly PublicationsContext _context;

        public PublisherCollectionSortingType SortingType => PublisherCollectionSortingType.ByTitle;

        public PublisherCollectionTitleSorter(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> Sort(
            PaginatedPublishersCollectionQuery query)
        {
            var baseQuery = this._context.Books.AsNoTracking()
                .Include(p => p.Publisher)
                .Where(p => p.Publisher.Id == query.PublisherId)
                .FilterBooksByTitle(query.Title);

            var totalCountQuery = baseQuery.DeferredCount();

            IOrderedQueryable<BookReadModel> orderedQuery;
            if (!query.Descending)
                orderedQuery = baseQuery.OrderByDescending(p => p.Title);
            else
                orderedQuery = baseQuery.OrderBy(p => p.Title);

            var resultsQuery = orderedQuery.Paginate(query.Page, query.Count)
                .Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var paginatedResult = results == null
                ? new PaginatedResult<int>(query.Page)
                : new PaginatedResult<int>(results.Select(s => s.Id).ToList(), query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}