using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.ShelfRecordTracker
{
    public class ShelfRecordTrackerCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public int Status { get; private set; }

        private ShelfRecordTrackerCreated()
        {
        }

        public ShelfRecordTrackerCreated(Guid aggregateGuid, Guid bookcaseGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookcaseGuid = bookcaseGuid;
            Status = AggregateStatus.Active.Value;
        }
    }
}