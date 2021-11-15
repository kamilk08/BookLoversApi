using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Publishers
{
    public class PublisherCycleCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid PublisherCycleGuid { get; private set; }

        public int PublisherCycleId { get; private set; }

        public Guid PublisherGuid { get; private set; }

        private PublisherCycleCreatedIntegrationEvent()
        {
        }

        public PublisherCycleCreatedIntegrationEvent(
            Guid publisherCycleGuid,
            int publisherCycleId,
            Guid publisherGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.PublisherCycleGuid = publisherCycleGuid;
            this.PublisherCycleId = publisherCycleId;
            this.PublisherGuid = publisherGuid;
        }
    }
}