using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Authors
{
    public class AuthorQuoteAddedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid QuoteGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private AuthorQuoteAddedIntegrationEvent()
        {
        }

        public AuthorQuoteAddedIntegrationEvent(Guid quoteGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.QuoteGuid = quoteGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}