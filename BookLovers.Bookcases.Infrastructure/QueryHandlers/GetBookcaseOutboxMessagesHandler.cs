using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers
{
    internal class GetBookcaseOutboxMessagesHandler :
        IQueryHandler<GetBookcaseOutboxMessagesQuery, List<OutboxMessage>>
    {
        private readonly BookcaseContext _context;

        public GetBookcaseOutboxMessagesHandler(BookcaseContext context)
        {
            _context = context;
        }

        public Task<List<OutboxMessage>> HandleAsync(
            GetBookcaseOutboxMessagesQuery inBookcaseOutboxMessagesQuery)
        {
            return _context.OutboxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}