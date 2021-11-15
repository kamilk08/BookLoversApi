using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Tokens;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Tokens
{
    internal class GetRefreshTokenReadModelHandler :
        IQueryHandler<GetRefreshTokenReadModelQuery, RefreshTokenReadModel>
    {
        private readonly AuthContext _context;

        public GetRefreshTokenReadModelHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<RefreshTokenReadModel> HandleAsync(
            GetRefreshTokenReadModelQuery query)
        {
            return _context.RefreshTokens
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.TokenGuid == query.TokenGuid);
        }
    }
}