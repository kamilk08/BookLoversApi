using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Bookcases
{
    public class BookRemovedFromShelf : IEvent
    {
        public Guid BookGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ShelfRecordTrackerGuid { get; private set; }

        public DateTime RemovedAt { get; private set; }

        private BookRemovedFromShelf()
        {
        }

        private BookRemovedFromShelf(
            Guid bookcaseGuid,
            Guid shelfGuid,
            Guid bookGuid,
            Guid trackerGuid,
            DateTime removedAt)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = bookcaseGuid;
            ShelfGuid = shelfGuid;
            BookGuid = bookGuid;
            ShelfRecordTrackerGuid = trackerGuid;
            RemovedAt = removedAt;
        }

        public static BookRemovedFromShelf Initialize()
        {
            return new BookRemovedFromShelf();
        }

        public BookRemovedFromShelf WithBookcase(Guid bookcaseGuid)
        {
            return new BookRemovedFromShelf(bookcaseGuid, ShelfGuid, BookGuid, ShelfRecordTrackerGuid, RemovedAt);
        }

        public BookRemovedFromShelf WithShelf(Guid shelfGuid)
        {
            return new BookRemovedFromShelf(AggregateGuid, shelfGuid,
                BookGuid, ShelfRecordTrackerGuid, RemovedAt);
        }

        public BookRemovedFromShelf WithBook(Guid bookGuid)
        {
            return new BookRemovedFromShelf(AggregateGuid, ShelfGuid,
                bookGuid, ShelfRecordTrackerGuid, RemovedAt);
        }

        public BookRemovedFromShelf WithTracker(Guid shelfRecordTrackerGuid)
        {
            return new BookRemovedFromShelf(AggregateGuid, ShelfGuid, BookGuid, shelfRecordTrackerGuid, RemovedAt);
        }

        public BookRemovedFromShelf WithRemovedAt(DateTime removedAt)
        {
            return new BookRemovedFromShelf(AggregateGuid, ShelfGuid, BookGuid, ShelfRecordTrackerGuid, removedAt);
        }
    }
}