using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Librarians.Integration.IntegrationEvents
{
    public class AuthorDismissedByLibrarian : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid DismissedByGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public string Justification { get; private set; }

        private AuthorDismissedByLibrarian()
        {
        }

        public AuthorDismissedByLibrarian(
            Guid readerGuid,
            Guid dismissedByGuid,
            Guid authorGuid,
            string justification)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.ReaderGuid = readerGuid;
            this.DismissedByGuid = dismissedByGuid;
            this.AuthorGuid = authorGuid;
            this.Justification = justification;
        }

        public static AuthorDismissedByLibrarian Initialize()
        {
            return new AuthorDismissedByLibrarian();
        }

        public AuthorDismissedByLibrarian WithReader(Guid readerGuid)
        {
            return new AuthorDismissedByLibrarian(readerGuid, this.DismissedByGuid, this.AuthorGuid,
                this.Justification);
        }

        public AuthorDismissedByLibrarian DismissedBy(Guid librarianGuid)
        {
            return new AuthorDismissedByLibrarian(this.ReaderGuid, librarianGuid, this.AuthorGuid, this.Justification);
        }

        public AuthorDismissedByLibrarian WithAuthor(Guid authorGuid)
        {
            return new AuthorDismissedByLibrarian(this.ReaderGuid, this.DismissedByGuid, authorGuid,
                this.Justification);
        }

        public AuthorDismissedByLibrarian WithJustification(
            string justification)
        {
            return new AuthorDismissedByLibrarian(this.ReaderGuid, this.DismissedByGuid, this.AuthorGuid,
                this.Justification);
        }
    }
}