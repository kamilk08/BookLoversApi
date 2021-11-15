using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Audiences;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Audiences
{
    internal class GetAudienceHandler : IQueryHandler<GetAudienceQuery, AudienceReadModel>
    {
        private readonly AuthContext _context;

        public GetAudienceHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<AudienceReadModel> HandleAsync(GetAudienceQuery query)
        {
            return _context.Audiences
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.AudienceGuid == query.AudienceGuid);
        }
    }
}