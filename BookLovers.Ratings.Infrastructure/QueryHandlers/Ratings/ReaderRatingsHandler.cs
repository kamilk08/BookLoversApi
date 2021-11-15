using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Ratings;
using Z.EntityFramework.Plus;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Ratings
{
    internal class ReaderRatingsHandler :
        IQueryHandler<ReaderRatingsByIdQuery, PaginatedResult<RatingDto>>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public ReaderRatingsHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<RatingDto>> HandleAsync(
            ReaderRatingsByIdQuery query)
        {
            var baseQuery = this._context.Ratings.AsNoTracking()
                .Where(p => p.ReaderId == query.ReaderId);

            var totalCountQuery = baseQuery.DeferredCount();
            var paginatedRatingsQuery = baseQuery.OrderByDescending(p => p.Id)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var ratings = await paginatedRatingsQuery.ToListAsync();

            var mappedRatings = this._mapper.Map<List<RatingReadModel>, List<RatingDto>>(ratings);

            var paginatedResult = new PaginatedResult<RatingDto>(mappedRatings, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}