using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.TicketOwners
{
    public class CreatedBookAccepted : IOwnerNotification, IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid ReaderGuid { get; }

        public Guid TicketGuid { get; }

        public Guid LibrarianGuid { get; }

        public Guid BookGuid { get; }

        public TicketConcern TicketConcern => TicketConcern.Book;

        public string Notification { get; }

        public string BookData { get; }

        private CreatedBookAccepted()
        {
        }

        [JsonConstructor]
        protected CreatedBookAccepted(
            Guid guid,
            Guid aggregateGuid,
            Guid readerGuid,
            Guid ticketGuid,
            Guid librarianGuid,
            Guid bookGuid,
            string notification,
            string bookData)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.TicketGuid = ticketGuid;
            this.LibrarianGuid = librarianGuid;
            this.BookGuid = bookGuid;
            this.Notification = notification;
            this.BookData = bookData;
        }

        private CreatedBookAccepted(
            Guid aggregateGuid,
            Guid readerGuid,
            Guid ticketGuid,
            Guid bookGuid,
            string bookData,
            string notification,
            Guid librarianGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.TicketGuid = ticketGuid;
            this.BookGuid = bookGuid;
            this.BookData = bookData;
            this.Notification = notification;
            this.LibrarianGuid = librarianGuid;
        }

        public static CreatedBookAccepted Initialize()
        {
            return new CreatedBookAccepted();
        }

        public CreatedBookAccepted WithAggregate(Guid aggregateGuid)
        {
            return new CreatedBookAccepted(aggregateGuid, this.ReaderGuid, this.TicketGuid, this.BookGuid,
                this.BookData,
                this.Notification, this.LibrarianGuid);
        }

        public CreatedBookAccepted WithTicketOwner(Guid readerGuid)
        {
            return new CreatedBookAccepted(this.AggregateGuid, readerGuid, this.TicketGuid, this.BookGuid,
                this.BookData,
                this.Notification, this.LibrarianGuid);
        }

        public CreatedBookAccepted WithTicket(Guid ticketGuid, string notification)
        {
            return new CreatedBookAccepted(this.AggregateGuid, this.ReaderGuid, ticketGuid, this.BookGuid,
                this.BookData,
                notification, this.LibrarianGuid);
        }

        public CreatedBookAccepted WithBook(Guid bookGuid, string bookData)
        {
            return new CreatedBookAccepted(this.AggregateGuid, this.ReaderGuid, this.TicketGuid, bookGuid, bookData,
                this.Notification, this.LibrarianGuid);
        }

        public CreatedBookAccepted AcceptedBy(Guid librarianGuid)
        {
            return new CreatedBookAccepted(this.AggregateGuid, this.ReaderGuid, this.TicketGuid, this.BookGuid,
                this.BookData,
                this.Notification, librarianGuid);
        }
    }
}