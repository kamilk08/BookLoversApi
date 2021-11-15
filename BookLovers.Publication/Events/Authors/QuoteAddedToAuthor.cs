using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class QuoteAddedToAuthor : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid QuoteGuid { get; private set; }

        private QuoteAddedToAuthor()
        {
        }

        public QuoteAddedToAuthor(Guid aggregateGuid, Guid quoteGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.QuoteGuid = quoteGuid;
        }
    }
}