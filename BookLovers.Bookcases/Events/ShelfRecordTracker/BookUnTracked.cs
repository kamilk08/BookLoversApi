using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.ShelfRecordTracker
{
    public class BookUnTracked : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        public DateTime AddedAt { get; private set; }

        private BookUnTracked()
        {
        }

        internal BookUnTracked(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();

            AggregateGuid = aggregateGuid;
        }

        public BookUnTracked WithBookcase(Guid bookcaseGuid)
        {
            BookcaseGuid = bookcaseGuid;

            return this;
        }

        public BookUnTracked WithBook(Guid bookGuid)
        {
            BookGuid = bookGuid;

            return this;
        }

        public BookUnTracked WithShelf(Guid shelfGuid)
        {
            ShelfGuid = shelfGuid;

            return this;
        }

        public BookUnTracked WithTrackedAt(DateTime addedAt)
        {
            AddedAt = addedAt;

            return this;
        }
    }
}