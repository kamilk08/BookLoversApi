using System.Collections.Generic;
using System.Data.Entity;
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
    internal class PaginatedBookQuotesHandler :
        IQueryHandler<PaginatedBookQuotesQuery, PaginatedResult<QuoteDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public PaginatedBookQuotesHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<QuoteDto>> HandleAsync(
            PaginatedBookQuotesQuery query)
        {
            var baseQuery = this._context.Quotes
                .Include(p => p.Author)
                .Include(p => p.Book)
                .Include(p => p.QuoteLikes)
                .AsNoTracking()
                .ActiveRecords().WithBook(query.BookId);

            var totalCountQuery = baseQuery.DeferredCount();
            var resultsQuery = baseQuery
                .OrderQuotesBy(query.Order, query.Descending)
                .Paginate(query.Page, query.Count)
                .Future();

            var totalQuotes = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<QuoteReadModel>, List<QuoteDto>>(results);

            var paginatedResult = new PaginatedResult<QuoteDto>(mappedResults, query.Page,
                query.Count, totalQuotes);

            return paginatedResult;
        }
    }
}