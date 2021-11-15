using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.Librarians
{
    public class LibrarianResolvedTicket : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid TicketGuid { get; }

        public string Justification { get; }

        public int TicketConcern { get; }

        public int Decision { get; }

        private LibrarianResolvedTicket()
        {
        }

        [JsonConstructor]
        protected LibrarianResolvedTicket(
            Guid guid,
            Guid aggregateGuid,
            Guid ticketGuid,
            string justification,
            int ticketConcern,
            int decision)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.TicketGuid = ticketGuid;
            this.Justification = justification;
            this.TicketConcern = ticketConcern;
            this.Decision = decision;
        }

        private LibrarianResolvedTicket(
            Guid aggregateGuid,
            Guid ticketGuid,
            int ticketConcern,
            int decision,
            string justification)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.TicketGuid = ticketGuid;
            this.TicketConcern = ticketConcern;
            this.Decision = decision;
            this.Justification = justification;
        }

        public static LibrarianResolvedTicket Initialize()
        {
            return new LibrarianResolvedTicket();
        }

        public LibrarianResolvedTicket WithAggregate(Guid aggregateGuid)
        {
            return new LibrarianResolvedTicket(
                aggregateGuid,
                this.TicketGuid, this.TicketConcern,
                this.Decision, this.Justification);
        }

        public LibrarianResolvedTicket WithTicket(
            Guid ticketGuid,
            int ticketConcern)
        {
            return new LibrarianResolvedTicket(
                this.AggregateGuid,
                ticketGuid, ticketConcern, this.Decision,
                this.Justification);
        }

        public LibrarianResolvedTicket WithDecision(
            int decision,
            string justification)
        {
            return new LibrarianResolvedTicket(
                this.AggregateGuid,
                this.TicketGuid, this.TicketConcern,
                decision, justification);
        }
    }
}