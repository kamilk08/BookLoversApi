using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.PublisherCycles
{
    public class PublisherCycleRestored : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        private PublisherCycleRestored()
        {
        }

        public PublisherCycleRestored(Guid aggregateGuid, Guid publisherGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.PublisherGuid = publisherGuid;
        }
    }
}