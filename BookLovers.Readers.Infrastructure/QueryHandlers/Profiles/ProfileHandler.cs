using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Profiles
{
    internal class ProfileHandler : IQueryHandler<ReaderProfileQuery, ProfileDto>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ProfileHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ProfileDto> HandleAsync(ReaderProfileQuery query)
        {
            var profile = await this._context.Profiles.AsNoTracking()
                .ActiveRecords()
                .Include(p => p.Reader)
                .SingleOrDefaultAsync(p => p.Reader.ReaderId == query.ReaderId);

            return this._mapper.Map<ProfileReadModel, ProfileDto>(profile);
        }
    }
}