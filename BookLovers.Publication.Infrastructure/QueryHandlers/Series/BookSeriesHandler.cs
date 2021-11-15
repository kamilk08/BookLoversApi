using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using BookLovers.Publication.Infrastructure.Queries.Series;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Series
{
    internal class BookSeriesHandler : IQueryHandler<BookSeriesQuery, SeriesDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public BookSeriesHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<SeriesDto> HandleAsync(BookSeriesQuery query)
        {
            var series = await this._context.Series.AsNoTracking()
                .Include(p => p.Books)
                .GetSeriesWithBook(query.BookId);

            return this._mapper.Map<SeriesReadModel, SeriesDto>(series);
        }
    }
}