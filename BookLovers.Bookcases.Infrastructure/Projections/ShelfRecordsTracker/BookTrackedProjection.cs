using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Events.ShelfRecordTracker;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.Projections.ShelfRecordsTracker
{
    internal class BookTrackedProjection : IProjectionHandler<BookTracked>, IProjectionHandler
    {
        private readonly BookcaseContext _context;

        public BookTrackedProjection(BookcaseContext context)
        {
            _context = context;
        }

        public void Handle(BookTracked @event)
        {
            var bookQuery = _context.Books.Where(p => p.BookGuid == @event.BookGuid).FutureValue();
            var shelfQuery = _context.Shelves.Include(p => p.Books).Where(p => p.Guid == @event.ShelfGuid)
                .FutureValue();
            var trackerQuery = _context.ShelfRecordTrackers.Include(p => p.ShelfRecords)
                .Where(p => p.ShelfRecordTrackerGuid == @event.AggregateGuid).FutureValue();

            var book = bookQuery.Value;
            var shelf = shelfQuery.Value;
            var tracker = trackerQuery.Value;
            var shelfRecord = _context.BookOnShelvesRecords.Include(p => p.Shelf.Bookcase)
                .OrderByDescending(p => p.AddedAt)
                .Where(p => p.Shelf.Bookcase.Guid == @event.AggregateGuid
                            && p.Shelf.ShelfCategory != ShelfCategory.Custom.Value)
                .FirstOrDefault(p => p.Book.Id == book.Id);

            var entity = new ShelfRecordReadModel
            {
                BookId = book.Id,
                ShelfId = shelf.Id,
                AddedAt = shelfRecord == null ? DateTime.UtcNow : shelfRecord.AddedAt
            };

            _context.BookOnShelvesRecords.Add(entity);
            tracker.ShelfRecords.Add(entity);

            _context.ShelfRecordTrackers.AddOrUpdate(p => p.Id, tracker);

            _context.SaveChanges();
        }
    }
}