using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.PublisherCycles;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.PublisherCycles
{
    internal class FindCycleHandler : IQueryHandler<FindCycleQuery, PaginatedResult<PublisherCycleDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public FindCycleHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<PublisherCycleDto>> HandleAsync(
            FindCycleQuery query)
        {
            var baseQuery = this._context.PublisherCycles.AsNoTracking()
                .ActiveRecords().Include(p => p.CycleBooks)
                .Include(p => p.Publisher)
                .FilterCyclesByTitle(query.Value);

            var totalCountQuery = baseQuery.DeferredCount();
            var resultsQuery = baseQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count);

            var totalCount = await totalCountQuery.ExecuteAsync();

            var results = await resultsQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<PublisherCycleReadModel>, List<PublisherCycleDto>>(results);

            var paginatedResult =
                new PaginatedResult<PublisherCycleDto>(mappedResults, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}