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
    internal class GetLibrariansOutboxMessagesHandler :
        IQueryHandler<GetLibrariansOutboxMessagesQuery, List<OutboxMessage>>
    {
        private readonly LibrariansContext _context;

        public GetLibrariansOutboxMessagesHandler(LibrariansContext context)
        {
            this._context = context;
        }

        public Task<List<OutboxMessage>> HandleAsync(
            GetLibrariansOutboxMessagesQuery inLibrarianModuleQuery)
        {
            return this._context.OutboxMessages.AsNoTracking()
                .Where(p => p.ProcessedAt == null)
                .ToListAsync();
        }
    }
}