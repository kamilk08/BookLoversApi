using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Librarians.Domain.Tickets;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.Tickets
{
    public class TicketSolved : IEvent
    {
        public int State { get; }

        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid LibrarianGuid { get; }

        public string Notification { get; }

        private TicketSolved()
        {
        }

        [JsonConstructor]
        protected TicketSolved(
            int state,
            Guid guid,
            Guid aggregateGuid,
            Guid librarianGuid,
            string notification)
        {
            this.State = state;
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.LibrarianGuid = librarianGuid;
            this.Notification = notification;
        }

        public TicketSolved(Guid ticketGuid, Guid librarianGuid,
            string notification)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = ticketGuid;
            this.LibrarianGuid = librarianGuid;
            this.Notification = notification;
            this.State = TicketState.Solved.Value;
        }
    }
}