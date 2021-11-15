using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.QueryHandlers
{
    internal class GetInboxMessagesHandler :
        IQueryHandler<GetReadersInboxMessagesQuery, List<InBoxMessage>>
    {
        private readonly ReadersContext _context;

        public GetInboxMessagesHandler(ReadersContext context)
        {
            this._context = context;
        }

        public Task<List<InBoxMessage>> HandleAsync(
            GetReadersInboxMessagesQuery query)
        {
            return this._context.InboxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}