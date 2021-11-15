using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Users
{
    internal class IsUserNameUniqueHandler : IQueryHandler<IsUserNameUniqueQuery, bool>
    {
        private readonly AuthContext _authContext;

        public IsUserNameUniqueHandler(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public Task<bool> HandleAsync(IsUserNameUniqueQuery query)
        {
            return _authContext.Users
                .AsNoTracking()
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .AnyAsync(p => p.UserName == query.UserName);
        }
    }
}