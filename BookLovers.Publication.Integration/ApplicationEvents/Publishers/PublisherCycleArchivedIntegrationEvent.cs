using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Publishers
{
    public class PublisherCycleArchivedIntegrationEvent : IIntegrationEvent
    {
        public Guid PublisherCycleGuid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private PublisherCycleArchivedIntegrationEvent()
        {
        }

        public PublisherCycleArchivedIntegrationEvent(Guid publisherCycleGuid, Guid publisherGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.PublisherCycleGuid = publisherCycleGuid;
            this.PublisherGuid = publisherGuid;
        }
    }
}