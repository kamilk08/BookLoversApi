using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Quotes
{
    public class QuoteArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ObjectGuid { get; private set; }

        public Guid AddedBy { get; private set; }

        public int QuoteStatus { get; private set; }

        private QuoteArchived()
        {
        }

        public QuoteArchived(Guid aggregateGuid, Guid objectGuid, Guid addedBy)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ObjectGuid = objectGuid;
            this.AddedBy = addedBy;
            this.QuoteStatus = AggregateStatus.Archived.Value;
        }
    }
}