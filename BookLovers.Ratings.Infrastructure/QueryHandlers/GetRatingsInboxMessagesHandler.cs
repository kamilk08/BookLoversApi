using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Queries;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers
{
    internal class GetRatingsInboxMessagesHandler :
        IQueryHandler<GetRatingsInboxMessagesQuery, List<InBoxMessage>>
    {
        private readonly RatingsContext _context;

        public GetRatingsInboxMessagesHandler(RatingsContext context)
        {
            this._context = context;
        }

        public Task<List<InBoxMessage>> HandleAsync(
            GetRatingsInboxMessagesQuery query)
        {
            return this._context.InboxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}