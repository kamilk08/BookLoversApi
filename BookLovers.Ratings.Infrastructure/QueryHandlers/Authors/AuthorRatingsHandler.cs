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
    internal class AuthorRatingsHandler : IQueryHandler<AuthorRatingsQuery, RatingsDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public AuthorRatingsHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<RatingsDto> HandleAsync(AuthorRatingsQuery query)
        {
            var author = await this._context.Authors
                .Include(p => p.Books.Select(s => s.Ratings))
                .AsNoTracking()
                .ActiveRecords()
                .SingleOrDefaultAsync(p => p.AuthorId == query.AuthorId);

            return this._mapper.Map<AuthorReadModel, RatingsDto>(author);
        }
    }
}