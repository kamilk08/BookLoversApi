using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Authors
{
    public class AuthorArchivedIntegrationEvent : IIntegrationEvent
    {
        public Guid AuthorGuid { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private AuthorArchivedIntegrationEvent()
        {
        }

        public AuthorArchivedIntegrationEvent(Guid authorGuid)
        {
            this.AuthorGuid = authorGuid;
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
        }
    }
}