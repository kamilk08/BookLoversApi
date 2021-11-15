using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Queries;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers
{
    internal class GetLibrariansInboxMessagesHandler :
        IQueryHandler<GetLibrariansInboxMessagesQuery, List<InBoxMessage>>
    {
        private readonly LibrariansContext _context;

        public GetLibrariansInboxMessagesHandler(LibrariansContext context)
        {
            this._context = context;
        }

        public Task<List<InBoxMessage>> HandleAsync(
            GetLibrariansInboxMessagesQuery query)
        {
            return this._context.InBoxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}