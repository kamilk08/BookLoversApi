using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.PublisherCycles
{
    public class PublisherCycleArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public int Status { get; private set; }

        private PublisherCycleArchived()
        {
        }

        public PublisherCycleArchived(Guid cycleGuid, Guid publisherGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = cycleGuid;
            this.PublisherGuid = publisherGuid;
            this.Status = AggregateStatus.Archived.Value;
        }
    }
}