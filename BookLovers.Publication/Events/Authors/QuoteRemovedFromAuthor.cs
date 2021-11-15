using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class QuoteRemovedFromAuthor : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid QuoteGuid { get; private set; }

        private QuoteRemovedFromAuthor()
        {
        }

        public QuoteRemovedFromAuthor(Guid aggregateGuid, Guid quoteGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.QuoteGuid = quoteGuid;
        }
    }
}