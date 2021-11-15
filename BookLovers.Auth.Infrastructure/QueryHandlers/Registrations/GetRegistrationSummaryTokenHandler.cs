using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Registrations;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Registrations
{
    internal class GetRegistrationSummaryTokenHandler :
        IQueryHandler<GetRegistrationSummaryTokenQuery, string>
    {
        private readonly AuthContext _context;

        public GetRegistrationSummaryTokenHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<string> HandleAsync(GetRegistrationSummaryTokenQuery query)
        {
            return _context.RegistrationSummaries
                .AsNoTracking()
                .Where(p => p.Email == query.Email)
                .Select(s => s.Token)
                .SingleOrDefaultAsync();
        }
    }
}