using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Profiles
{
    internal class ReaderProfilePrivacyHandler :
        IQueryHandler<ReaderProfilePrivacyQuery, ProfilePrivacyDto>
    {
        private readonly ReadersContext _context;

        public ReaderProfilePrivacyHandler(ReadersContext context)
        {
            this._context = context;
        }

        public async Task<ProfilePrivacyDto> HandleAsync(
            ReaderProfilePrivacyQuery query)
        {
            var dto = await _context.PrivacyManagers
                .AsNoTracking()
                .Include(p => p.PrivacyOptions)
                .Where(p => p.ReaderId == query.ReaderId)
                .Select(s => new ProfilePrivacyDto
                {
                    ProfileId = s.ProfileId,
                    PrivacyOptions = s.PrivacyOptions.Select(po => new ProfilePrivacyOptionDto
                    {
                        PrivacyOptionId = po.PrivacyTypeOptionId,
                        PrivacyOptionName = po.PrivacyOptionName,
                        PrivacyName = po.PrivacyTypeName,
                        PrivacyTypeId = po.PrivacyTypeId
                    }).ToList()
                }).SingleOrDefaultAsync();

            return dto;
        }
    }
}