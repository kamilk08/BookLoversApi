using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Queries.FilteringExtensions;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Bookcases
{
    internal class BookcasesWithBookHandler : IQueryHandler<BookcasesWithBookQuery, List<int>>
    {
        private readonly BookcaseContext _context;

        public BookcasesWithBookHandler(BookcaseContext context)
        {
            _context = context;
        }

        public Task<List<int>> HandleAsync(BookcasesWithBookQuery query)
        {
            return _context.Bookcases
                .Include(p => p.Shelves.Select(s => s.Books))
                .AsNoTracking()
                .ActiveRecords()
                .GetBookcasesWithBook(query.BookId).Select(s => s.Id)
                .ToListAsync();
        }
    }
}