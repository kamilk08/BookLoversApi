using System;
using BookLovers.Base.Infrastructure.Persistence;

namespace BaseTests.EventStore
{
    internal class InMemorySnapshot : ISnapshot
    {
        public int Id { get; set; }

        public Guid AggregateGuid { get; set; }

        public int Version { get; set; }

        public string SnapshottedAggregate { get; set; }

        public InMemorySnapshot(Guid aggregateGuid, string snapshottedAggregate, int version)
        {
            AggregateGuid = aggregateGuid;
            SnapshottedAggregate = snapshottedAggregate;
            Version = version;
        }
    }
}