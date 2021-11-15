using System;
using BookLovers.Base.Domain.Events;

namespace BaseTests.Aggregates.DummyEvents
{
    internal class DummyEventThatChangesStateToArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; set; }

        public byte AggregateStatus { get; set; }

        private DummyEventThatChangesStateToArchived()
        {
        }

        public DummyEventThatChangesStateToArchived(Guid aggregateId, byte aggregateStatus)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateId;
            AggregateStatus = aggregateStatus;
        }
    }
}