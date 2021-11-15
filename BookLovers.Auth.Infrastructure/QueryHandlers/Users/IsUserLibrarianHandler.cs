using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Users
{
    internal class IsUserLibrarianHandler : IQueryHandler<IsUserLibrarianQuery, bool>
    {
        private readonly AuthContext _authContext;

        public IsUserLibrarianHandler(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public Task<bool> HandleAsync(IsUserLibrarianQuery query)
        {
            return _authContext.Users
                .AsNoTracking()
                .Include(p => p.Roles)
                .AnyAsync(p => p.Guid == query.UserGuid
                               && p.Roles.Any(a => a.Value == Role.Librarian.Value));
        }
    }
}