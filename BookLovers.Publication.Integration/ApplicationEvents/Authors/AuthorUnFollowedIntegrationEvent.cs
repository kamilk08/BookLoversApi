using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Authors
{
    public class AuthorUnFollowedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private AuthorUnFollowedIntegrationEvent()
        {
        }

        public AuthorUnFollowedIntegrationEvent(Guid authorGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.AuthorGuid = authorGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}