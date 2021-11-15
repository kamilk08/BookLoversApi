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
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.PublisherCycles
{
    internal class PaginatedCyclesHandler :
        IQueryHandler<PaginatedCyclesQuery, PaginatedResult<PublisherCycleDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public PaginatedCyclesHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<PublisherCycleDto>> HandleAsync(
            PaginatedCyclesQuery query)
        {
            var baseQuery = this._context.PublisherCycles
                .Include(p => p.Publisher)
                .Include(p => p.CycleBooks).AsNoTracking()
                .ActiveRecords().Where(p => query.CyclesIds.Contains(p.Id));

            var totalCountQuery = baseQuery.DeferredCount();
            var resultsQuery = baseQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count);

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<PublisherCycleReadModel>, List<PublisherCycleDto>>(results);

            var paginatedResult = totalCount != 0
                ? new PaginatedResult<PublisherCycleDto>(mappedResults, query.Page, query.Count, totalCount)
                : new PaginatedResult<PublisherCycleDto>(query.Page);

            return paginatedResult;
        }
    }
}