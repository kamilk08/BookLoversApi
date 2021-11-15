using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers
{
    internal class GetAuthOutboxMessagesHandler :
        IQueryHandler<GetAuthOutboxMessagesQuery, List<OutboxMessage>>
    {
        private readonly AuthContext _context;

        public GetAuthOutboxMessagesHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<List<OutboxMessage>> HandleAsync(
            GetAuthOutboxMessagesQuery query)
        {
            return _context.OutboxMessages
                .AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}