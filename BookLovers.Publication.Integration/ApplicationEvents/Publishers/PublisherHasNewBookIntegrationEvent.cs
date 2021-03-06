using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Publishers
{
    public class PublisherHasNewBookIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private PublisherHasNewBookIntegrationEvent()
        {
        }

        public PublisherHasNewBookIntegrationEvent(Guid publisherGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.PublisherGuid = publisherGuid;
            this.BookGuid = bookGuid;
        }
    }
}