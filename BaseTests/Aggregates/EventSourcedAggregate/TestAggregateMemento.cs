using System;
using BookLovers.Base.Domain;

namespace BaseTests.Aggregates.EventSourcedAggregate
{
    public class TestAggregateMemento :
        ITestAggregateMemento,
        IMemento<TestEventSourcedAggregateRoot>,
        IMemento
    {
        public int Version { get; private set; }

        public int LastCommittedVersion { get; private set; }

        public int AggregateStatus { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int Counter { get; private set; }

        public IMemento<TestEventSourcedAggregateRoot> TakeSnapshot(
            TestEventSourcedAggregateRoot aggregate)
        {
            Version = aggregate.Version;
            LastCommittedVersion = aggregate.LastCommittedVersion;
            AggregateStatus = aggregate.AggregateStatus.Value;
            AggregateGuid = aggregate.Guid;
            Counter = aggregate.Counter;

            return this;
        }
    }
}