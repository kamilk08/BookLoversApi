using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.ShelfRecordTracker
{
    public class ShelfRecordTrackerArchived : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int Status { get; private set; }

        private ShelfRecordTrackerArchived()
        {
        }

        public ShelfRecordTrackerArchived(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Status = AggregateStatus.Archived.Value;
        }
    }
}