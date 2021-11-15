using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Root.Domain
{
    internal class BookReaderAccessor : IBookReaderAccessor
    {
        private readonly PublicationsContext _context;

        public BookReaderAccessor(PublicationsContext context)
        {
            _context = context;
        }

        public Task<Guid> GetAggregateGuidAsync(Guid bookReaderGuid)
        {
            return _context.Readers.AsNoTracking()
                .Where(p => p.ReaderGuid == bookReaderGuid)
                .Select(s => s.Guid)
                .SingleOrDefaultAsync();
        }
    }
}