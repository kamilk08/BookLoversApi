using System;
using BookLovers.Base.Domain.Events;

namespace BaseTests.Aggregates.DummyEvents
{
    internal class DoWorkEvent : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; set; }

        public int Counter { get; private set; }

        private DoWorkEvent()
        {
        }

        public DoWorkEvent(Guid aggregateGuid, int counter)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Counter = counter;
        }
    }
}