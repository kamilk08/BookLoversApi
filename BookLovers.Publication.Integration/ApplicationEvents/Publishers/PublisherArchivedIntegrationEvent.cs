using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Publishers
{
    public class PublisherArchivedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid PublisherGuid { get; private set; }

        private PublisherArchivedIntegrationEvent()
        {
        }

        public PublisherArchivedIntegrationEvent(Guid publisherGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.PublisherGuid = publisherGuid;
        }
    }
}