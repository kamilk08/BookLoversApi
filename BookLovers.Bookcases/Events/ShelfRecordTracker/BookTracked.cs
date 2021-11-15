using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.ShelfRecordTracker
{
    public class BookTracked : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        public DateTime AddedAt { get; private set; }

        private BookTracked()
        {
        }

        internal BookTracked(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
        }

        public BookTracked WithBookcase(Guid bookcaseGuid)
        {
            BookcaseGuid = bookcaseGuid;

            return this;
        }

        public BookTracked WithBook(Guid bookGuid)
        {
            BookGuid = bookGuid;

            return this;
        }

        public BookTracked WithShelf(Guid shelfGuid)
        {
            ShelfGuid = shelfGuid;

            return this;
        }

        public BookTracked WithTrackedAt(DateTime addedAt)
        {
            AddedAt = addedAt;

            return this;
        }
    }
}