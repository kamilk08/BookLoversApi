using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Publishers
{
    public class PublisherBookRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private PublisherBookRemoved()
        {
        }

        public PublisherBookRemoved(Guid publisherGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = publisherGuid;
            this.BookGuid = bookGuid;
        }
    }
}