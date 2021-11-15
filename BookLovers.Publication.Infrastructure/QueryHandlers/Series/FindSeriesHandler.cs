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
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Series
{
    internal class FindSeriesHandler : IQueryHandler<FindSeriesQuery, PaginatedResult<SeriesDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public FindSeriesHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<SeriesDto>> HandleAsync(
            FindSeriesQuery query)
        {
            var baseQuery = _context.Series.AsNoTracking()
                .Include(p => p.Books)
                .ActiveRecords()
                .Where(p => p.Name.ToUpper().StartsWith(query.Value.ToUpper()));

            var totalCountQuery = baseQuery.DeferredCount();
            var resultsQuery = baseQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count);

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var mappedResults = _mapper.Map<List<SeriesReadModel>, List<SeriesDto>>(results);

            return new PaginatedResult<SeriesDto>(mappedResults, query.Page, query.Count, totalCount);
        }
    }
}