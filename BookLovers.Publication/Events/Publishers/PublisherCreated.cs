using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Publishers
{
    public class PublisherCreated : IEvent
    {
        public string Name { get; private set; }

        public int PublisherStatus { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private PublisherCreated()
        {
        }

        public PublisherCreated(Guid publisherGuid, string name)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = publisherGuid;
            this.Name = name;
            this.PublisherStatus = AggregateStatus.Active.Value;
        }
    }
}