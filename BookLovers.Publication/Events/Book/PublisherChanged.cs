using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class PublisherChanged : IEvent
    {
        public Guid OldPublisherGuid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private PublisherChanged()
        {
        }

        public PublisherChanged(Guid bookGuid, Guid publisherGuid, Guid oldPublisherGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.PublisherGuid = publisherGuid;
            this.OldPublisherGuid = oldPublisherGuid;
        }
    }
}