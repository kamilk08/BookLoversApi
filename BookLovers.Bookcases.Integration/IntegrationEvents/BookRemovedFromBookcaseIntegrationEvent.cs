using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Bookcases.Integration.IntegrationEvents
{
    public class BookRemovedFromBookcaseIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private BookRemovedFromBookcaseIntegrationEvent()
        {
        }

        public BookRemovedFromBookcaseIntegrationEvent(Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.ReaderGuid = readerGuid;
        }
    }
}