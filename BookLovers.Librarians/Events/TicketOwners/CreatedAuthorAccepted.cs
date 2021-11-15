using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.TicketOwners
{
    public class CreatedAuthorAccepted : IOwnerNotification, IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid LibrarianGuid { get; }

        public Guid AuthorGuid { get; }

        public Guid ReaderGuid { get; }

        public Guid TicketGuid { get; }

        public TicketConcern TicketConcern
        {
            get { return TicketConcern.Author; }
        }

        public string Justification { get; }

        public string AuthorData { get; }

        private CreatedAuthorAccepted()
        {
        }

        [JsonConstructor]
        protected CreatedAuthorAccepted(
            Guid guid,
            Guid aggregateGuid,
            Guid librarianGuid,
            Guid authorGuid,
            Guid readerGuid,
            Guid ticketGuid,
            string justification,
            string authorData)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.LibrarianGuid = librarianGuid;
            this.AuthorGuid = authorGuid;
            this.ReaderGuid = readerGuid;
            this.TicketGuid = ticketGuid;
            this.Justification = justification;
            this.AuthorData = authorData;
        }

        private CreatedAuthorAccepted(
            Guid aggregateGuid,
            Guid readerGuid,
            Guid ticketGuid,
            string justification,
            string authorData,
            Guid librarianGuid,
            Guid authorGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.TicketGuid = ticketGuid;
            this.Justification = justification;
            this.AuthorData = authorData;
            this.LibrarianGuid = librarianGuid;
            this.AuthorGuid = authorGuid;
        }

        public static CreatedAuthorAccepted Initialize()
        {
            return new CreatedAuthorAccepted();
        }

        public CreatedAuthorAccepted WithAggregate(Guid aggregateGuid)
        {
            return new CreatedAuthorAccepted(
                aggregateGuid,
                this.ReaderGuid, this.TicketGuid,
                this.Justification, this.AuthorData, this.LibrarianGuid,
                this.AuthorGuid);
        }

        public CreatedAuthorAccepted WithTicketOwner(Guid readerGuid)
        {
            return new CreatedAuthorAccepted(
                this.AggregateGuid,
                readerGuid, this.TicketGuid, this.Justification,
                this.AuthorData, this.LibrarianGuid, this.AuthorGuid);
        }

        public CreatedAuthorAccepted WithTicket(
            Guid ticketGuid,
            string justification)
        {
            return new CreatedAuthorAccepted(
                this.AggregateGuid, this.ReaderGuid, ticketGuid,
                justification, this.AuthorData, this.LibrarianGuid, this.AuthorGuid);
        }

        public CreatedAuthorAccepted WithAuthor(string authorData, Guid authorGuid)
        {
            return new CreatedAuthorAccepted(
                this.AggregateGuid, this.ReaderGuid, this.TicketGuid,
                this.Justification, authorData,
                this.LibrarianGuid,
                authorGuid);
        }

        public CreatedAuthorAccepted AcceptedBy(Guid librarianGuid)
        {
            return new CreatedAuthorAccepted(
                this.AggregateGuid,
                this.ReaderGuid, this.TicketGuid,
                this.Justification, this.AuthorData,
                librarianGuid, this.AuthorGuid);
        }
    }
}