using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.PublisherCycles
{
    public class BookRemovedFromCycle : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private BookRemovedFromCycle()
        {
        }

        public BookRemovedFromCycle(Guid aggregateGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.BookGuid = bookGuid;
        }
    }
}