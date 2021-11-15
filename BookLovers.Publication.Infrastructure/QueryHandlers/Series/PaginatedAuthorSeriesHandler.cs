using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using BookLovers.Publication.Infrastructure.Queries.Series;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Series
{
    internal class PaginatedAuthorSeriesHandler :
        IQueryHandler<PaginatedAuthorSeriesQuery, PaginatedResult<SeriesDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public PaginatedAuthorSeriesHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<SeriesDto>> HandleAsync(
            PaginatedAuthorSeriesQuery query)
        {
            var baseQuery = this._context.Series
                .Include(p => p.Books.Select(s => s.Authors))
                .AsNoTracking().FilterSeriesWithAuthorBooks(query.AuthorId);

            var totalCountQuery = baseQuery.DeferredCount();
            var seriesQuery = baseQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await seriesQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<SeriesReadModel>, List<SeriesDto>>(results);

            var paginatedResult = new PaginatedResult<SeriesDto>(mappedResults, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}