using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Books
{
    public class BookArchivedIntegrationEvent : IIntegrationEvent
    {
        public Guid BookGuid { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private BookArchivedIntegrationEvent()
        {
        }

        public BookArchivedIntegrationEvent(Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.BookGuid = bookGuid;
        }
    }
}