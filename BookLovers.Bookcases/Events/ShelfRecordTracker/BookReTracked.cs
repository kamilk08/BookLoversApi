using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.ShelfRecordTracker
{
    public class BookReTracked : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid OldShelfGuid { get; private set; }

        public Guid NewShelfGuid { get; private set; }

        public DateTime TrackedAt { get; private set; }

        private BookReTracked()
        {
        }

        internal BookReTracked(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
        }

        public BookReTracked WithBook(Guid bookGuid)
        {
            BookGuid = bookGuid;

            return this;
        }

        public BookReTracked WithOldShelf(Guid oldShelfGuid)
        {
            OldShelfGuid = oldShelfGuid;
            return this;
        }

        public BookReTracked WithNewShelf(Guid newShelfGuid)
        {
            NewShelfGuid = newShelfGuid;
            return this;
        }

        public BookReTracked WithTrackedAt(DateTime trackedAt)
        {
            TrackedAt = trackedAt;
            return this;
        }
    }
}