using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Publishers
{
    public class PublisherArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int PublisherStatus { get; private set; }

        public IEnumerable<Guid> PublisherBooks { get; private set; }

        private PublisherArchived()
        {
        }

        public PublisherArchived(Guid publisherGuid, IEnumerable<Guid> publisherBooks)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = publisherGuid;
            this.PublisherStatus = AggregateStatus.Archived.Value;
            this.PublisherBooks = publisherBooks;
        }
    }
}