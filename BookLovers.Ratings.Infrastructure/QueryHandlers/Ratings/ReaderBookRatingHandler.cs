using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Ratings;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Ratings
{
    internal class ReaderBookRatingHandler : IQueryHandler<ReaderBookRatingQuery, RatingDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public ReaderBookRatingHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<RatingDto> HandleAsync(ReaderBookRatingQuery query)
        {
            var rating = await this._context.Books.AsNoTracking()
                .Include(p => p.Ratings)
                .Where(p => p.BookId == query.BookId)
                .SelectMany(sm => sm.Ratings)
                .SingleOrDefaultAsync(p => p.ReaderId == query.ReaderId);

            return this._mapper.Map<RatingReadModel, RatingDto>(rating);
        }
    }
}