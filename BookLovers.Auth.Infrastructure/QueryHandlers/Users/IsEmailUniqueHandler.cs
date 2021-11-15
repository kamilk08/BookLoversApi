using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Users
{
    internal class IsEmailUniqueHandler : IQueryHandler<IsEmailUniqueQuery, bool>
    {
        private readonly AuthContext _context;

        public IsEmailUniqueHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<bool> HandleAsync(IsEmailUniqueQuery query)
        {
            return _context.Users
                .AsNoTracking()
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .AnyAsync(p => p.Account.Email == query.Email);
        }
    }
}