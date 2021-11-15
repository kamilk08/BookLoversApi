using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.Series;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Series
{
    internal class SeriesByIdHandler : IQueryHandler<SeriesByIdQuery, SeriesDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public SeriesByIdHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<SeriesDto> HandleAsync(SeriesByIdQuery query)
        {
            var series = await this._context.Series
                .Include(p => p.Books).AsNoTracking()
                .ActiveRecords().WithId(query.SeriesId)
                .SingleOrDefaultAsync();

            return this._mapper.Map<SeriesReadModel, SeriesDto>(series);
        }
    }
}