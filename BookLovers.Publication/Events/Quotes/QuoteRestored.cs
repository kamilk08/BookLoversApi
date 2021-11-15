using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Quotes
{
    public class QuoteRestored : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; }

        public Guid QuotedGuid { get; private set; }

        public Guid AddedBy { get; private set; }

        public int QuoteStatus { get; private set; }

        private QuoteRestored()
        {
        }

        public QuoteRestored(Guid aggregateGuid, Guid quotedGuid, Guid addedBy)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.QuotedGuid = quotedGuid;
            this.AddedBy = addedBy;
            this.QuoteStatus = AggregateStatus.Active.Value;
        }
    }
}