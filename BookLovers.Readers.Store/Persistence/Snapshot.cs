using System;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Readers.Store.Persistence
{
    public class Snapshot : ISnapshot
    {
        public int Id { get; set; }

        public Guid AggregateGuid { get; set; }

        public int Version { get; set; }

        public string SnapshottedAggregate { get; set; }

        public Snapshot()
        {
        }

        public Snapshot(Guid aggregateGuid, int version, string snapshottedAggregate)
        {
            AggregateGuid = aggregateGuid;
            Version = version;
            SnapshottedAggregate = snapshottedAggregate;
        }
    }
}