using System.Collections.Generic;
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
    internal class MultipleReaderRatingsHandler :
        IQueryHandler<MultipleReaderRatingsQuery, List<RatingDto>>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public MultipleReaderRatingsHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<List<RatingDto>> HandleAsync(MultipleReaderRatingsQuery query)
        {
            var ratings = await this._context.Books.AsNoTracking()
                .Include(p => p.Ratings)
                .Where(p => query.BookIds.Contains(p.BookId))
                .Where(p => p.Ratings.Any(a => a.ReaderId == query.ReaderId))
                .SelectMany(sm => sm.Ratings)
                .ToListAsync();

            return this._mapper.Map<List<RatingReadModel>, List<RatingDto>>(ratings);
        }
    }
}