using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Quotes;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Quotes
{
    internal class IsQuoteAddedByHandler : IQueryHandler<IsQuoteAddedByQuery, bool>
    {
        private readonly PublicationsContext _context;

        public IsQuoteAddedByHandler(PublicationsContext context)
        {
            this._context = context;
        }

        public Task<bool> HandleAsync(IsQuoteAddedByQuery query)
        {
            return this._context.Quotes.AsNoTracking()
                .AnyAsync(a => a.ReaderGuid == query.ReaderGuid && a.Guid == query.QuoteGuid);
        }
    }
}