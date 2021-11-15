using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;
using BookLovers.Publication.Infrastructure.Queries.Quotes;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Quotes
{
    internal class PaginatedUserQuotesHandler :
        IQueryHandler<PaginatedUserQuotesQuery, PaginatedResult<QuoteDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public PaginatedUserQuotesHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<QuoteDto>> HandleAsync(
            PaginatedUserQuotesQuery query)
        {
            var baseQuery = this._context.Quotes.Include(p => p.Author)
                .Include(p => p.Author)
                .Include(p => p.QuoteLikes)
                .AsNoTracking()
                .ActiveRecords()
                .Where(p => p.ReaderId == query.ReaderId);

            var totalCountQuery = baseQuery.DeferredCount();

            var quotesQuery = baseQuery
                .OrderQuotesBy(query.Order, query.Descending)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await quotesQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<QuoteReadModel>, List<QuoteDto>>(results);

            var paginatedResult = new PaginatedResult<QuoteDto>(mappedResults, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}