using System;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Bookcases.Store.Persistence
{
    public class Snapshot : ISnapshot
    {
        public int Id { get; set; }

        public Guid AggregateGuid { get; set; }

        public int Version { get; set; }

        public string SnapshottedAggregate { get; set; }

        private Snapshot()
        {
        }

        public Snapshot(Guid aggregateGuid, string snapshottedAggregate, int version)
        {
            AggregateGuid = aggregateGuid;
            SnapshottedAggregate = snapshottedAggregate;
            Version = version;
        }
    }
}