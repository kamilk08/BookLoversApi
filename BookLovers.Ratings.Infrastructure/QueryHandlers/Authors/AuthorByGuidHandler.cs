using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Authors;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Authors
{
    internal class AuthorByGuidHandler : IQueryHandler<AuthorByGuidQuery, AuthorDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public AuthorByGuidHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<AuthorDto> HandleAsync(AuthorByGuidQuery query)
        {
            var author = await this._context.Authors.AsNoTracking()
                .Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.AuthorGuid == query.AuthorGuid);

            return this._mapper.Map<AuthorReadModel, AuthorDto>(author);
        }
    }
}