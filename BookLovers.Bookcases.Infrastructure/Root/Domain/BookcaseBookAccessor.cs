using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks;
using BookLovers.Bookcases.Application.Contracts;
using BookLovers.Bookcases.Infrastructure.Persistence;

namespace BookLovers.Bookcases.Infrastructure.Root.Domain
{
    internal class BookcaseBookAccessor : IBookcaseBookAccessor
    {
        private readonly BookcaseContext _context;

        public BookcaseBookAccessor(BookcaseContext context)
        {
            _context = context;
        }

        public Task<Guid> GetBookcaseBookAggregateGuid(Guid bookGuid)
        {
            return _context.Books.AsNoTracking()
                .Where(p => p.BookGuid == bookGuid)
                .Select(s => s.AggregateGuid)
                .SingleOrDefaultAsync();
        }
    }
}