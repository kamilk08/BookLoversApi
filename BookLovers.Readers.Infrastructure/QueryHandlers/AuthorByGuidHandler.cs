using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.QueryHandlers
{
    internal class AuthorByGuidHandler : IQueryHandler<AuthorByGuidQuery, AuthorDto>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public AuthorByGuidHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<AuthorDto> HandleAsync(AuthorByGuidQuery query)
        {
            var author = await this._context.Authors.SingleOrDefaultAsync(p => p.AuthorGuid == query.AuthorGuid);

            return this._mapper.Map<AuthorReadModel, AuthorDto>(author);
        }
    }
}