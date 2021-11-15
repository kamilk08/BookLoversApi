using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Publishers
{
    internal class PaginatedPublishersHandler :
        IQueryHandler<PaginatedPublishersQuery, PaginatedResult<PublisherDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public PaginatedPublishersHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<PublisherDto>> HandleAsync(
            PaginatedPublishersQuery query)
        {
            var baseQuery = this._context.Publishers
                .AsNoTracking().Include(p => p.Books.Select(s => s.Authors))
                .Include(p => p.Cycles.Select(s => s.CycleBooks))
                .ActiveRecords().OrderBy(p => p.Id);

            var totalCountQuery = baseQuery.DeferredCount();
            var resultsQuery = baseQuery.Paginate(query.Page, query.Count)
                .Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<PublisherReadModel>, List<PublisherDto>>(results);

            var paginatedResult = results != null
                ? new PaginatedResult<PublisherDto>(
                    mappedResults, query.Page, query.Count,
                    totalCount)
                : new PaginatedResult<PublisherDto>(query.Page);

            return paginatedResult;
        }
    }
}