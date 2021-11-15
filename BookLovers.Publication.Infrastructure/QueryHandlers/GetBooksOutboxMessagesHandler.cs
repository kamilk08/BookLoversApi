using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.QueryHandlers
{
    internal class GetBooksOutboxMessagesHandler :
        IQueryHandler<GetBooksOutboxMessagesQuery, List<OutboxMessage>>
    {
        private readonly PublicationsContext _context;

        public GetBooksOutboxMessagesHandler(PublicationsContext context)
        {
            this._context = context;
        }

        public Task<List<OutboxMessage>> HandleAsync(
            GetBooksOutboxMessagesQuery inPublicationModuleQuery)
        {
            return this._context.OutboxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}