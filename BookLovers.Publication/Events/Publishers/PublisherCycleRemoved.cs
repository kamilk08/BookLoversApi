using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Publishers
{
    public class PublisherCycleRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid CycleGuid { get; private set; }

        private PublisherCycleRemoved()
        {
        }

        public PublisherCycleRemoved(Guid publisherGuid, Guid cycleGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = publisherGuid;
            this.CycleGuid = cycleGuid;
        }
    }
}