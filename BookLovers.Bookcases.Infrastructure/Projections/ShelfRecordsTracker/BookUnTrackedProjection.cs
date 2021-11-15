using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.ShelfRecordTracker;
using BookLovers.Bookcases.Infrastructure.Persistence;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.ShelfRecordsTracker
{
    internal class BookUnTrackedProjection : IProjectionHandler<BookUnTracked>, IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public BookUnTrackedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookUnTracked @event)
        {
            var bookQuery = _context.Books.Where(p => p.BookGuid == @event.BookGuid).FutureValue();
            var shelfQuery = _context.Shelves.Where(p => p.Guid == @event.ShelfGuid).FutureValue();

            var book = bookQuery.Value;
            var shelf = shelfQuery.Value;

            var shelfRecord = _context.BookOnShelvesRecords.OrderByDescending(p => p.AddedAt)
                .FirstOrDefault(p => p.Shelf.Id == shelf.Id && p.Book.Id == book.Id);

            _context.BookOnShelvesRecords.Remove(shelfRecord);
            _context.SaveChanges();
        }
    }
}