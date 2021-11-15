using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Librarians.Integration.IntegrationEvents
{
    public class ReaderPromotedToLibrarian : IIntegrationEvent
    {
        public Guid ReaderGuid { get; private set; }

        public Guid LibrarianGuid { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private ReaderPromotedToLibrarian()
        {
        }

        public ReaderPromotedToLibrarian(Guid readerGuid, Guid librarianGuid)
        {
            this.ReaderGuid = readerGuid;
            this.LibrarianGuid = librarianGuid;
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
        }
    }
}