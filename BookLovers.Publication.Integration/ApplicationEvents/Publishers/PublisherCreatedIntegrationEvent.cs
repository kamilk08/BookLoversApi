using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Publishers
{
    public class PublisherCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid PublisherGuid { get; private set; }

        public int PublisherId { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private PublisherCreatedIntegrationEvent()
        {
        }

        public PublisherCreatedIntegrationEvent(Guid publisherGuid, int publisherId)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.PublisherGuid = publisherGuid;
            this.PublisherId = publisherId;
        }
    }
}