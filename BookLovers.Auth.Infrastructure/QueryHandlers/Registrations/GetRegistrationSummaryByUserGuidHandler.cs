using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Registrations;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Registrations
{
    internal class GetRegistrationSummaryByUserGuidHandler :
        IQueryHandler<GetRegistrationSummaryByUserGuidQuery, RegistrationSummaryReadModel>
    {
        private readonly AuthContext _context;

        public GetRegistrationSummaryByUserGuidHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<RegistrationSummaryReadModel> HandleAsync(
            GetRegistrationSummaryByUserGuidQuery query)
        {
            return _context.RegistrationSummaries
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.UserGuid == query.UserGuid);
        }
    }
}