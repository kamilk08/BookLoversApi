using System;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Publication.Store.Persistence
{
    public class Snapshot : ISnapshot
    {
        public int Id { get; set; }

        public Guid AggregateGuid { get; set; }

        public int Version { get; set; }

        public string SnapshottedAggregate { get; set; }

        protected Snapshot()
        {
        }

        public Snapshot(Guid aggregateGuid, string snapshottedAggregate, int version)
        {
            this.AggregateGuid = aggregateGuid;
            this.Version = version;
            this.SnapshottedAggregate = snapshottedAggregate;
        }
    }
}