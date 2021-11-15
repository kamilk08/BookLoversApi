using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Auth.Infrastructure.Services.Authentication;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Users
{
    internal class GetUserClaimsHandler : IQueryHandler<GetUserClaimsQuery, ClaimsIdentity>
    {
        private readonly AuthContext _context;
        private readonly ClaimsBuilder _claimsBuilder;

        public GetUserClaimsHandler(
            AuthContext context,
            ClaimsBuilder claimsBuilder,
            IHashingService hashingService)
        {
            _context = context;
            _claimsBuilder = claimsBuilder;
        }

        public async Task<ClaimsIdentity> HandleAsync(GetUserClaimsQuery query)
        {
            var user = await _context.Users.AsNoTracking()
                .Include(p => p.Account)
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.Account.Email == query.Email);

            if (user == null)
                return null;

            var claimsIdentity = _claimsBuilder
                .AddSubject(user.Guid.ToString())
                .AddTokenIdentifier()
                .AddIssuedDate()
                .AddEmail(user.Account.Email)
                .AddCustomClaim("userId", user.Id)
                .AddRoles(user.Roles.Select(s => s.Name))
                .GetClaimsIdentity();

            return claimsIdentity;
        }
    }
}