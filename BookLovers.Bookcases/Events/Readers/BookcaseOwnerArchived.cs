using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Readers
{
    public class BookcaseOwnerArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public int Status { get; private set; }

        private BookcaseOwnerArchived()
        {
        }

        public BookcaseOwnerArchived(Guid aggregateGuid, Guid bookcaseGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookcaseGuid = bookcaseGuid;
            Status = AggregateStatus.Archived.Value;
        }
    }
}