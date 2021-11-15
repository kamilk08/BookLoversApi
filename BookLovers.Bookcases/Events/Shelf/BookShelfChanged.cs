using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Shelf
{
    public class BookShelfChanged : IEvent
    {
        public Guid BookGuid { get; private set; }

        public Guid OldShelfGuid { get; private set; }

        public Guid NewShelfGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid TrackerGuid { get; private set; }

        public DateTime ChangedAt { get; private set; }

        private BookShelfChanged()
        {
        }

        private BookShelfChanged(
            Guid aggregateGuid,
            Guid bookGuid,
            Guid oldShelfGuid,
            Guid newShelfGuid,
            Guid trackerGuid,
            DateTime changedAt)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookGuid = bookGuid;
            OldShelfGuid = oldShelfGuid;
            NewShelfGuid = newShelfGuid;
            ChangedAt = changedAt;
            TrackerGuid = trackerGuid;
        }

        public static BookShelfChanged Initialize()
        {
            return new BookShelfChanged();
        }

        public BookShelfChanged WithBookcaseAndBook(Guid bookcaseGuid, Guid bookGuid)
        {
            return new BookShelfChanged(bookcaseGuid, bookGuid, OldShelfGuid, NewShelfGuid, TrackerGuid, ChangedAt);
        }

        public BookShelfChanged WithOldShelf(Guid oldShelfGuid)
        {
            return new BookShelfChanged(AggregateGuid, BookGuid,
                oldShelfGuid, NewShelfGuid, TrackerGuid, ChangedAt);
        }

        public BookShelfChanged WithNewShelf(Guid newShelfGuid)
        {
            return new BookShelfChanged(AggregateGuid, BookGuid,
                OldShelfGuid, newShelfGuid, TrackerGuid, ChangedAt);
        }

        public BookShelfChanged WithChangedAt(DateTime changedAt)
        {
            return new BookShelfChanged(AggregateGuid, BookGuid,
                OldShelfGuid, NewShelfGuid, TrackerGuid, changedAt);
        }

        public BookShelfChanged WithTracker(Guid trackerGuid)
        {
            return new BookShelfChanged(AggregateGuid, BookGuid,
                OldShelfGuid, NewShelfGuid, trackerGuid, ChangedAt);
        }
    }
}