using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Bookcases.Integration.IntegrationEvents
{
    public class ShelfCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; }

        public DateTime OccuredOn { get; }

        public Guid ReaderGuid { get; private set; }

        private ShelfCreatedIntegrationEvent()
        {
        }

        public ShelfCreatedIntegrationEvent(Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.ReaderGuid = readerGuid;
        }
    }
}