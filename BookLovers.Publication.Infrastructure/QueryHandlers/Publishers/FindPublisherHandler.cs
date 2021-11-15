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
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Publishers
{
    internal class FindPublisherHandler :
        IQueryHandler<FindPublisherQuery, PaginatedResult<PublisherDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public FindPublisherHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<PublisherDto>> HandleAsync(
            FindPublisherQuery query)
        {
            var baseQuery = this._context.Publishers.Include(p => p.Books)
                .Include(p => p.Cycles).AsNoTracking()
                .ActiveRecords().FilterPublishersByTitle(query.Value);

            var totalCountQuery = baseQuery.DeferredCount();

            var resultsQuery = baseQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<PublisherReadModel>, List<PublisherDto>>(results);

            var paginatedResult = new PaginatedResult<PublisherDto>(mappedResults, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}