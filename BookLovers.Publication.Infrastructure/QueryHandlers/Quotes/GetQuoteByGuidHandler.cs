using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;
using BookLovers.Publication.Infrastructure.Queries.Quotes;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Quotes
{
    internal class GetQuoteByGuidHandler : IQueryHandler<QuoteByGuidQuery, QuoteDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public GetQuoteByGuidHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<QuoteDto> HandleAsync(QuoteByGuidQuery query)
        {
            var quote = await this._context.Quotes
                .Include(p => p.Author)
                .Include(p => p.Book)
                .Include(p => p.QuoteLikes)
                .AsNoTracking()
                .ActiveRecords().SingleOrDefaultAsync(p => p.Guid == query.QuoteGuid);

            return this._mapper.Map<QuoteReadModel, QuoteDto>(quote);
        }
    }
}