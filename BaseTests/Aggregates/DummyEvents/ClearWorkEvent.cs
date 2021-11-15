using System;
using BookLovers.Base.Domain.Events;

namespace BaseTests.Aggregates.DummyEvents
{
    internal class ClearWorkEvent : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; set; }

        public int Counter { get; }

        private ClearWorkEvent()
        {
        }

        public ClearWorkEvent(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Counter = 0;
        }
    }
}