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
    internal class SeriesByGuidHandler : IQueryHandler<SeriesByGuidQuery, SeriesDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public SeriesByGuidHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<SeriesDto> HandleAsync(SeriesByGuidQuery query)
        {
            var series = await this._context.Series.AsNoTracking()
                .Include(p => p.Books).FirstOrDefaultAsync(p => p.SeriesGuid == query.SeriesGuid);

            return this._mapper.Map<SeriesReadModel, SeriesDto>(series);
        }
    }
}