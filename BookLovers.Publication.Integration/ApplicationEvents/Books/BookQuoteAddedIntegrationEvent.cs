using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Books
{
    public class BookQuoteAddedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid QuoteGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private BookQuoteAddedIntegrationEvent()
        {
        }

        public BookQuoteAddedIntegrationEvent(Guid quoteGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.QuoteGuid = quoteGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}