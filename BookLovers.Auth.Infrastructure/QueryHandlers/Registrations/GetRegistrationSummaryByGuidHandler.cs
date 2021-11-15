using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Registrations;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Registrations
{
    internal class GetRegistrationSummaryByGuidHandler :
        IQueryHandler<GetRegistrationSummaryByGuidQuery, RegistrationSummaryReadModel>
    {
        private readonly AuthContext _context;

        public GetRegistrationSummaryByGuidHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<RegistrationSummaryReadModel> HandleAsync(
            GetRegistrationSummaryByGuidQuery query)
        {
            return _context.RegistrationSummaries
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Guid == query.RegistrationSummaryGuid);
        }
    }
}