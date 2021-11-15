using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers
{
    internal class GetResetPasswordTokenHandler : IQueryHandler<GetResetPasswordTokenQuery, string>
    {
        private readonly AuthContext _context;

        public GetResetPasswordTokenHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<string> HandleAsync(GetResetPasswordTokenQuery query)
        {
            return _context.PasswordResetTokens
                .AsNoTracking()
                .Where(p => p.Email == query.Email)
                .OrderByDescending(p => p.ExpiresAt)
                .Select(s => s.Token)
                .SingleOrDefaultAsync();
        }
    }
}