using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.PublisherCycles
{
    public class PublisherCycleCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public string CycleName { get; private set; }

        public int CycleStatus { get; private set; }

        private PublisherCycleCreated()
        {
        }

        public PublisherCycleCreated(Guid cycleGuid, Guid publisherGuid, string cycleName)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = cycleGuid;
            this.PublisherGuid = publisherGuid;
            this.CycleName = cycleName;
            this.CycleStatus = AggregateStatus.Active.Value;
        }
    }
}