using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.TicketOwners
{
    public class CreatedAuthorDismissed : IOwnerNotification, IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid LibrarianGuid { get; }

        public Guid ReaderGuid { get; }

        public Guid TicketGuid { get; }

        public Guid AuthorGuid { get; }

        public TicketConcern TicketConcern => TicketConcern.Author;

        public string Notification { get; }

        private CreatedAuthorDismissed()
        {
        }

        [JsonConstructor]
        protected CreatedAuthorDismissed(
            Guid guid,
            Guid aggregateGuid,
            Guid librarianGuid,
            Guid readerGuid,
            Guid ticketGuid,
            Guid authorGuid,
            string notification)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.LibrarianGuid = librarianGuid;
            this.ReaderGuid = readerGuid;
            this.TicketGuid = ticketGuid;
            this.AuthorGuid = authorGuid;
            this.Notification = notification;
        }

        private CreatedAuthorDismissed(
            Guid aggregateGuid,
            Guid readerGuid,
            Guid ticketGuid,
            string notification,
            Guid authorGuid,
            Guid librarianGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.TicketGuid = ticketGuid;
            this.Notification = notification;
            this.AuthorGuid = authorGuid;
            this.LibrarianGuid = librarianGuid;
        }

        public static CreatedAuthorDismissed Initialize()
        {
            return new CreatedAuthorDismissed();
        }

        public CreatedAuthorDismissed WithAggregate(Guid aggregateGuid)
        {
            return new CreatedAuthorDismissed(aggregateGuid, this.ReaderGuid, this.TicketGuid, this.Notification,
                this.AuthorGuid, this.LibrarianGuid);
        }

        public CreatedAuthorDismissed WithTicketOwner(Guid readerGuid)
        {
            return new CreatedAuthorDismissed(this.AggregateGuid, readerGuid, this.TicketGuid, this.Notification,
                this.AuthorGuid, this.LibrarianGuid);
        }

        public CreatedAuthorDismissed WithTicket(
            Guid ticketGuid,
            string notification)
        {
            return new CreatedAuthorDismissed(this.AggregateGuid, this.ReaderGuid, ticketGuid, notification,
                this.AuthorGuid, this.LibrarianGuid);
        }

        public CreatedAuthorDismissed WithAuthor(Guid authorGuid)
        {
            return new CreatedAuthorDismissed(this.AggregateGuid, this.ReaderGuid, this.TicketGuid, this.Notification,
                authorGuid, this.LibrarianGuid);
        }

        public CreatedAuthorDismissed AcceptedBy(Guid librarianGuid)
        {
            return new CreatedAuthorDismissed(this.AggregateGuid, this.ReaderGuid, this.TicketGuid, this.Notification,
                this.AuthorGuid, librarianGuid);
        }
    }
}