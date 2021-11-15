using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Books
{
    internal class IsBookInBookcaseQueryHandler : IQueryHandler<IsBookInBookcaseQuery, bool>
    {
        private readonly BookcaseContext _context;

        public IsBookInBookcaseQueryHandler(BookcaseContext context)
        {
            _context = context;
        }

        public Task<bool> HandleAsync(IsBookInBookcaseQuery query)
        {
            return _context.Bookcases
                .Include(p => p.Shelves.Select(s => s.Books))
                .Where(p => p.ReaderGuid == query.ReaderGuid)
                .SelectMany(sm => sm.Shelves
                    .Where(a => a.ShelfCategory != ShelfCategory.Custom.Value))
                .SelectMany(sm => sm.Books).AnyAsync(a => a.BookGuid == query.BookGuid);
        }
    }
}