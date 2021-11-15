using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.FilteringExtensions;
using BookLovers.Readers.Infrastructure.Queries.Readers.Follows;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Followings
{
    internal class ReaderPaginatedFollowingsHandler :
        IQueryHandler<ReaderPaginatedFollowingsQuery, PaginatedResult<int>>
    {
        private readonly ReadersContext _context;

        public ReaderPaginatedFollowingsHandler(ReadersContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> HandleAsync(
            ReaderPaginatedFollowingsQuery query)
        {
            var source = this._context.FollowObjects
                .AsNoTracking()
                .Include(p => p.Follower)
                .Include(p => p.Followed)
                .Where(p => p.Follower.ReaderId == query.ReaderId
                            && p.Followed.Status == AggregateStatus.Active.Value)
                .FilterFollowingsByUserName(query.Value)
                .Select(s => s.Followed.ReaderId);

            var totalCountQuery = source.DeferredCount();

            var followersIdsQuery = source
                .OrderBy(p => p)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await followersIdsQuery.ToListAsync();

            var paginatedResult = new PaginatedResult<int>(
                results, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}