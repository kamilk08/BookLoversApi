using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Reviews
{
    internal class ReaderReviewsIdsHandler : IQueryHandler<ReaderReviewsIdsQuery, PaginatedResult<int>>
    {
        private readonly ReadersContext _context;

        public ReaderReviewsIdsHandler(ReadersContext readContext)
        {
            this._context = readContext;
        }

        public async Task<PaginatedResult<int>> HandleAsync(
            ReaderReviewsIdsQuery query)
        {
            var baseQuery = this._context.Reviews.AsNoTracking()
                .Include(p => p.Reader)
                .Where(p => p.Reader.ReaderId == query.ReaderId)
                .Select(s => s.Id);

            var totalCountQuery = baseQuery.DeferredCount();
            var resultsQuery = baseQuery.OrderBy(p => p)
                .Paginate(query.Page, query.Count)
                .Future();

            var totalItems = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var paginatedResult = results != null
                ? new PaginatedResult<int>(results, query.Page, query.Count, totalItems)
                : new PaginatedResult<int>(query.Page);

            return paginatedResult;
        }
    }
}