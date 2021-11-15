using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using BookLovers.Publication.Infrastructure.Queries.Authors;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Authors
{
    internal class AuthorByNameHandler : IQueryHandler<AuthorByNameQuery, AuthorDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public AuthorByNameHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<AuthorDto> HandleAsync(AuthorByNameQuery query)
        {
            var author = await _context.Authors
                .Include(p => p.SubCategories)
                .Include(p => p.AuthorFollowers.Select(s => s.FollowedBy))
                .Include(p => p.AuthorBooks)
                .Include(p => p.Quotes)
                .Include(p => p.AddedBy)
                .ActiveRecords()
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.FullName.Trim().ToUpper() == query.Name.Trim().ToUpper());

            var mappedResult = _mapper.Map<AuthorReadModel, AuthorDto>(author);

            return mappedResult;
        }
    }
}