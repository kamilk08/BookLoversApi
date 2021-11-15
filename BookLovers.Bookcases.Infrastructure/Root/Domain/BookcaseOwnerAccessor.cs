using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Bookcases.Application.CommandHandlers.BookcaseOwners;
using BookLovers.Bookcases.Infrastructure.Persistence;

namespace BookLovers.Bookcases.Infrastructure.Root.Domain
{
    internal class BookcaseOwnerAccessor : IBookcaseOwnerAccessor
    {
        private readonly BookcaseContext _context;

        public BookcaseOwnerAccessor(BookcaseContext context)
        {
            _context = context;
        }

        public Task<Guid> GetOwnerAggregateGuidAsync(Guid bookcaseOwnerGuid)
        {
            return _context.Readers.AsNoTracking()
                .Where(p => p.ReaderGuid == bookcaseOwnerGuid)
                .Select(s => s.Guid)
                .SingleAsync();
        }
    }
}