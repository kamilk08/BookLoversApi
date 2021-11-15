using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Users
{
    internal class IsSuperAdminCreatedHandler : IQueryHandler<IsSuperAdminCreatedQuery, bool>
    {
        private readonly AuthContext _context;

        public IsSuperAdminCreatedHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<bool> HandleAsync(IsSuperAdminCreatedQuery query)
        {
            return _context.Users.Include(p => p.Roles)
                .AsNoTracking().ActiveRecords()
                .AnyAsync(p => p.Roles.Any(a => a.Value == Role.SuperAdmin.Value));
        }
    }
}