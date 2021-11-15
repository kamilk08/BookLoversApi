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
    internal class GetRatingsOutboxMessagesHandler :
        IQueryHandler<GetRatingsOutboxMessagesQuery, List<OutboxMessage>>
    {
        private readonly RatingsContext _context;

        public GetRatingsOutboxMessagesHandler(RatingsContext context)
        {
            this._context = context;
        }

        public Task<List<OutboxMessage>> HandleAsync(
            GetRatingsOutboxMessagesQuery inRatingsOutboxMessagesQuery)
        {
            return this._context.OutboxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}