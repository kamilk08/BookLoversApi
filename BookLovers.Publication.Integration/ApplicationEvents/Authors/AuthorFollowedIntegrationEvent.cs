using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Authors
{
    public class AuthorFollowedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private AuthorFollowedIntegrationEvent()
        {
        }

        public AuthorFollowedIntegrationEvent(Guid authorGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.AuthorGuid = authorGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}