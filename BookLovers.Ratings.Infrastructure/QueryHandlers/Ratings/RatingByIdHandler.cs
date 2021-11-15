using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Ratings;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Ratings
{
    internal class RatingByIdHandler : IQueryHandler<RatingByIdQuery, RatingDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public RatingByIdHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<RatingDto> HandleAsync(RatingByIdQuery query)
        {
            var rating = await this._context.Ratings.AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == query.RatingId);

            return this._mapper.Map<RatingReadModel, RatingDto>(rating);
        }
    }
}