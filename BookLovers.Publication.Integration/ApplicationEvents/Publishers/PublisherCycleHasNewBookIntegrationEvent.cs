using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Publishers
{
    public class PublisherCycleHasNewBookIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid PublisherCycleGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private PublisherCycleHasNewBookIntegrationEvent()
        {
        }

        public PublisherCycleHasNewBookIntegrationEvent(Guid publisherCycleGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.PublisherCycleGuid = publisherCycleGuid;
            this.BookGuid = bookGuid;
        }
    }
}