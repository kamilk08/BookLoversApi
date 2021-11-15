using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Follows;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Followings
{
    internal class IsFollowingHandler : IQueryHandler<IsFollowingQuery, bool>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ReadersContext _context;

        public IsFollowingHandler(IHttpContextAccessor accessor, ReadersContext context)
        {
            this._accessor = accessor;
            this._context = context;
        }

        public Task<bool> HandleAsync(IsFollowingQuery query)
        {
            if (!this._accessor.IsAuthenticated)
                return Task.FromResult(false);

            return this._context.FollowObjects
                .AsNoTracking()
                .Include(p => p.Followed)
                .Include(p => p.Follower)
                .AnyAsync(p => p.Follower.Guid == this._accessor.UserGuid
                               && p.Followed.ReaderId == query.FollowingId);
        }
    }
}