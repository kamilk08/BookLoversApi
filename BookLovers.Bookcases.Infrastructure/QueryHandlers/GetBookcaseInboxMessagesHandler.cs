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
    internal class GetBookcaseInboxMessagesHandler :
        IQueryHandler<GetBookcaseInboxMessagesQuery, List<InBoxMessage>>
    {
        private readonly BookcaseContext _context;

        public GetBookcaseInboxMessagesHandler(BookcaseContext context)
        {
            _context = context;
        }

        public Task<List<InBoxMessage>> HandleAsync(
            GetBookcaseInboxMessagesQuery query)
        {
            return _context.InBoxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}