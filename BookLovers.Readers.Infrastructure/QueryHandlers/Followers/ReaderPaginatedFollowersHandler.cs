using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.FilteringExtensions;
using BookLovers.Readers.Infrastructure.Queries.Readers.Follows;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Followers
{
    internal class ReaderPaginatedFollowersHandler :
        IQueryHandler<ReaderPaginatedFollowersQuery, PaginatedResult<int>>
    {
        private readonly ReadersContext _context;

        public ReaderPaginatedFollowersHandler(ReadersContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> HandleAsync(
            ReaderPaginatedFollowersQuery query)
        {
            var readersQuery = this._context.Readers.AsNoTracking()
                .Include(p => p.Followers.Select(s => s.Follower))
                .Where(p => p.ReaderId == query.ReaderId).SelectMany(sm => sm.Followers)
                .Where(p => p.Follower.Status == AggregateStatus.Active.Value)
                .FilterFollowersByUserName(query.Value)
                .Select(s => s.Follower.ReaderId);

            var totalCountQuery = readersQuery.DeferredCount();

            var followersIdsQuery = readersQuery.OrderBy(p => p)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await followersIdsQuery.ToListAsync();

            var paginatedResult = new PaginatedResult<int>(
                results, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}