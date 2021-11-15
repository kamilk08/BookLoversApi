using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Series;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Series
{
    internal class SeriesRatingsHandler : IQueryHandler<SeriesRatingsQuery, RatingsDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public SeriesRatingsHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<RatingsDto> HandleAsync(SeriesRatingsQuery query)
        {
            var series = await this._context.Series.AsNoTracking()
                .SingleOrDefaultAsync(p => p.SeriesId == query.SeriesId);

            return this._mapper.Map<SeriesReadModel, RatingsDto>(series);
        }
    }
}