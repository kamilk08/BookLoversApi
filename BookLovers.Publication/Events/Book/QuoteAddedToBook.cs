using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class QuoteAddedToBook : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid QuoteGuid { get; private set; }

        private QuoteAddedToBook()
        {
        }

        public QuoteAddedToBook(Guid aggregateGuid, Guid quoteGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.QuoteGuid = quoteGuid;
        }
    }
}