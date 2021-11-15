using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Series;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Series
{
    internal class MultipleSeriesStatisticsHandler :
        IQueryHandler<MultipleSeriesStatisticsQuery, List<RatingsDto>>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public MultipleSeriesStatisticsHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<List<RatingsDto>> HandleAsync(
            MultipleSeriesStatisticsQuery query)
        {
            var series = await this._context.Series
                .Include(p => p.Books.Select(s => s.Ratings))
                .AsNoTracking()
                .Where(p => query.SeriesIds.Contains(p.SeriesId))
                .ToListAsync();

            return this._mapper.Map<List<SeriesReadModel>, List<RatingsDto>>(series);
        }
    }
}