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
    internal class GetOutboxMessagesHandler :
        IQueryHandler<GetReadersOutboxMessagesQuery, List<OutboxMessage>>
    {
        private readonly ReadersContext _readContext;

        public GetOutboxMessagesHandler(ReadersContext readContext)
        {
            this._readContext = readContext;
        }

        public Task<List<OutboxMessage>> HandleAsync(
            GetReadersOutboxMessagesQuery inReadersOutboxMessagesQuery)
        {
            return this._readContext.OutboxMessages
                .AsNoTracking()
                .Where(p => p.ProcessedAt == null).ToListAsync();
        }
    }
}