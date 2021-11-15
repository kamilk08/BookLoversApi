using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Librarians.Integration.IntegrationEvents
{
    public class AuthorAcceptedByLibrarian : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid AcceptedByGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public string Notification { get; private set; }

        public string AuthorData { get; private set; }

        private AuthorAcceptedByLibrarian()
        {
        }

        private AuthorAcceptedByLibrarian(
            Guid readerGuid,
            Guid acceptedByGuid,
            Guid authorGuid,
            string notification,
            string data)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.ReaderGuid = readerGuid;
            this.AcceptedByGuid = acceptedByGuid;
            this.AuthorGuid = authorGuid;
            this.Notification = notification;
            this.AuthorData = data;
        }

        public static AuthorAcceptedByLibrarian Initialize()
        {
            return new AuthorAcceptedByLibrarian();
        }

        public AuthorAcceptedByLibrarian WithReader(Guid readerGuid)
        {
            return new AuthorAcceptedByLibrarian(
                readerGuid,
                this.AcceptedByGuid,
                this.AuthorGuid,
                this.Notification,
                this.AuthorData);
        }

        public AuthorAcceptedByLibrarian AcceptedBy(Guid acceptedByGuid)
        {
            return new AuthorAcceptedByLibrarian(
                this.ReaderGuid,
                acceptedByGuid,
                this.AuthorGuid,
                this.Notification,
                this.AuthorData);
        }

        public AuthorAcceptedByLibrarian WithAuthor(Guid authorGuid, string authorData)
        {
            return new AuthorAcceptedByLibrarian(this.ReaderGuid, this.AcceptedByGuid, authorGuid, this.Notification,
                authorData);
        }

        public AuthorAcceptedByLibrarian WithNotification(string notification)
        {
            return new AuthorAcceptedByLibrarian(this.ReaderGuid, this.AcceptedByGuid, this.AuthorGuid, notification,
                this.AuthorData);
        }
    }
}