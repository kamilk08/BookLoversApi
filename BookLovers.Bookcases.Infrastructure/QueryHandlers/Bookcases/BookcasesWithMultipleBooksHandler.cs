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
    internal class BookcasesWithMultipleBooksHandler :
        IQueryHandler<BookcasesWithMultipleBooksQuery, List<int>>
    {
        private readonly BookcaseContext _context;

        public BookcasesWithMultipleBooksHandler(BookcaseContext context)
        {
            _context = context;
        }

        public Task<List<int>> HandleAsync(BookcasesWithMultipleBooksQuery query)
        {
            return _context.Bookcases
                .Include(p => p.Shelves.Select(s => s.Books))
                .AsNoTracking()
                .ActiveRecords()
                .GetBookcasesWithMultipleBooks(query.BookIds)
                .Select(s => s.Id)
                .ToListAsync();
        }
    }
}