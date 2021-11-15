using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Authors
{
    public class AuthorLostBookIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private AuthorLostBookIntegrationEvent()
        {
        }

        public AuthorLostBookIntegrationEvent(Guid authorGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.AuthorGuid = authorGuid;
            this.BookGuid = bookGuid;
        }
    }
}