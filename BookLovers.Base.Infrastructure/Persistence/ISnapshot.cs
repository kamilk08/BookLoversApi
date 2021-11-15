using System;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public interface ISnapshot
    {
        int Id { get; set; }

        Guid AggregateGuid { get; set; }

        int Version { get; set; }

        string SnapshottedAggregate { get; set; }
    }
}