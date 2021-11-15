using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.ShelfRecordTracker;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.ShelfRecordsTracker
{
    internal class BookReTrackedProjection : IProjectionHandler<BookReTracked>, IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public BookReTrackedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookReTracked @event)
        {
            var bookQuery = _context.Books.Where(p => p.BookGuid == @event.BookGuid).FutureValue();
            var shelfQuery = _context.Shelves.Include(p => p.Books).Where(p => p.Guid == @event.NewShelfGuid)
                .FutureValue();
            var shelfRecordQuery = _context.BookOnShelvesRecords.Include(p => p.Shelf).Include(p => p.Book)
                .Where(p => p.Book.BookGuid == @event.BookGuid && p.Shelf.Guid == @event.OldShelfGuid)
                .FutureValue();
            var tackerQuery = _context.ShelfRecordTrackers.Include(p => p.ShelfRecords.Select(s => s.Book))
                .Where(p => p.ShelfRecordTrackerGuid == @event.AggregateGuid)
                .FutureValue();

            var book = bookQuery.Value;
            var shelf = shelfQuery.Value;
            var shelfRecord = shelfRecordQuery.Value;
            var tracker = tackerQuery.Value;

            var newShelfRecord = new ShelfRecordReadModel
            {
                BookId = book.Id,
                ShelfId = shelf.Id,
                AddedAt = @event.TrackedAt
            };

            tracker.ShelfRecords.Remove(shelfRecord);
            tracker.ShelfRecords.Add(newShelfRecord);

            _context.ShelfRecordTrackers.AddOrUpdate(p => p.Id, tracker);
            _context.SaveChanges();
        }
    }
}