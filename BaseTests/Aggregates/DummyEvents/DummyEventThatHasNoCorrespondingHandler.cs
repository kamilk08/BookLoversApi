using System;
using BookLovers.Base.Domain.Events;

namespace BaseTests.Aggregates.DummyEvents
{
    internal class DummyEventThatHasNoCorrespondingHandler : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; set; }

        private DummyEventThatHasNoCorrespondingHandler()
        {
        }

        public DummyEventThatHasNoCorrespondingHandler(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
        }
    }
}