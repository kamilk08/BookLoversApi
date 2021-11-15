using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries;
using BookLovers.Shared.Privacy;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers
{
    internal class ShowBookcaseToOthersHandler : IQueryHandler<ShowBookcaseToOthersQuery, bool>
    {
        private readonly BookcaseContext _context;

        public ShowBookcaseToOthersHandler(BookcaseContext context)
        {
            _context = context;
        }

        public async Task<bool> HandleAsync(ShowBookcaseToOthersQuery query)
        {
            var managerReadModel =
                await _context.SettingsManagers
                    .SingleOrDefaultAsync(p => p.BookcaseId == query.BookcaseId);

            return managerReadModel != null && managerReadModel.Privacy == PrivacyOption.Private.Value;
        }
    }
}