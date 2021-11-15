using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Librarians.Integration.IntegrationEvents
{
    public class LibrarianDegradedToReader : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid UserGuid { get; private set; }

        private LibrarianDegradedToReader()
        {
        }

        public LibrarianDegradedToReader(Guid userGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.UserGuid = userGuid;
        }
    }
}