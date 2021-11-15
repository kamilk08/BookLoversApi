using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Publishers
{
    public class PublisherBookAdded : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private PublisherBookAdded()
        {
        }

        public PublisherBookAdded(Guid publisherGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = publisherGuid;
            this.BookGuid = bookGuid;
        }
    }
}