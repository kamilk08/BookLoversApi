using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.Series;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Series
{
    internal class MultipleSeriesHandler : IQueryHandler<MultipleSeriesQuery, List<SeriesDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public MultipleSeriesHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<List<SeriesDto>> HandleAsync(MultipleSeriesQuery query)
        {
            var series = await this._context.Series.AsNoTracking()
                .Include(p => p.Books).ActiveRecords()
                .Where(p => query.Ids.Contains(p.Id)).ToListAsync();

            return this._mapper.Map<List<SeriesReadModel>, List<SeriesDto>>(series);
        }
    }
}