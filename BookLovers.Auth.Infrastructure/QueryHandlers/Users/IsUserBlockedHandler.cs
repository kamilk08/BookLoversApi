using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Users
{
    internal class IsUserBlockedHandler : IQueryHandler<IsUserBlockedQuery, bool>
    {
        private readonly AuthContext _context;

        public IsUserBlockedHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<bool> HandleAsync(IsUserBlockedQuery query)
        {
            return _context.Users
                .AsNoTracking()
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .Where(p => p.Account.Email == query.WriteModel.Email
                            || p.UserName == query.WriteModel.UserName)
                .Select(s => s.Account.IsBlocked)
                .FirstOrDefaultAsync();
        }
    }
}