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
    internal class GetAuthInboxMessagesHandler :
        IQueryHandler<GetAuthInboxMessagesQuery, List<InBoxMessage>>
    {
        private readonly AuthContext _context;

        public GetAuthInboxMessagesHandler(AuthContext context)
        {
            _context = context;
        }

        public Task<List<InBoxMessage>> HandleAsync(GetAuthInboxMessagesQuery query)
        {
            return _context.InboxMessages
                .AsNoTracking()
                .Where(p => p.ProcessedAt == null).ToListAsync();
        }
    }
}