using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.TicketOwners
{
    public class CreatedBookDismissed : IOwnerNotification, IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid ReaderGuid { get; }

        public Guid TicketGuid { get; }

        public Guid BookGuid { get; }

        public Guid LibrarianGuid { get; }

        public TicketConcern TicketConcern => TicketConcern.Book;

        public string Notification { get; }

        private CreatedBookDismissed()
        {
        }

        [JsonConstructor]
        protected CreatedBookDismissed(
            Guid guid,
            Guid aggregateGuid,
            Guid readerGuid,
            Guid ticketGuid,
            Guid bookGuid,
            Guid librarianGuid,
            string notification)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.TicketGuid = ticketGuid;
            this.BookGuid = bookGuid;
            this.LibrarianGuid = librarianGuid;
            this.Notification = notification;
        }

        private CreatedBookDismissed(
            Guid aggregateGuid,
            Guid readerGuid,
            Guid ticketGuid,
            Guid bookGuid,
            string notification,
            Guid librarianGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.TicketGuid = ticketGuid;
            this.BookGuid = bookGuid;
            this.Notification = notification;
            this.LibrarianGuid = librarianGuid;
        }

        public static CreatedBookDismissed Initialize()
        {
            return new CreatedBookDismissed();
        }

        public CreatedBookDismissed WithAggregate(Guid aggregateGuid)
        {
            return new CreatedBookDismissed(aggregateGuid, this.ReaderGuid, this.TicketGuid, this.BookGuid,
                this.Notification,
                this.LibrarianGuid);
        }

        public CreatedBookDismissed WithTicketOwner(Guid readerGuid)
        {
            return new CreatedBookDismissed(this.AggregateGuid, readerGuid, this.TicketGuid, this.BookGuid,
                this.Notification,
                this.LibrarianGuid);
        }

        public CreatedBookDismissed WithTicket(Guid ticketGuid, string notification)
        {
            return new CreatedBookDismissed(this.AggregateGuid, this.ReaderGuid, ticketGuid, this.BookGuid,
                notification,
                this.LibrarianGuid);
        }

        public CreatedBookDismissed WithBook(Guid bookGuid)
        {
            return new CreatedBookDismissed(this.AggregateGuid, this.ReaderGuid, this.TicketGuid, bookGuid,
                this.Notification,
                this.LibrarianGuid);
        }

        public CreatedBookDismissed AcceptedBy(Guid librarianGuid)
        {
            return new CreatedBookDismissed(this.AggregateGuid, this.ReaderGuid, this.TicketGuid, this.BookGuid,
                this.Notification, librarianGuid);
        }
    }
}