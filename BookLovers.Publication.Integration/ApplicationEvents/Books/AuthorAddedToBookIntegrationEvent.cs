using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Books
{
    public class AuthorAddedToBookIntegrationEvent : IIntegrationEvent
    {
        public Guid AuthorGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private AuthorAddedToBookIntegrationEvent()
        {
        }

        public AuthorAddedToBookIntegrationEvent(Guid authorGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.AuthorGuid = authorGuid;
            this.BookGuid = bookGuid;
        }
    }
}