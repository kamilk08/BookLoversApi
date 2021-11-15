using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.Tickets
{
    public class TicketCreated : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid TicketOwnerGuid { get; }

        public string Title { get; }

        public DateTime Date { get; }

        public int TicketState { get; }

        public string TicketData { get; }

        private TicketCreated()
        {
        }

        [JsonConstructor]
        protected TicketCreated(
            Guid guid,
            Guid aggregateGuid,
            Guid ticketOwnerGuid,
            string title,
            DateTime date,
            byte ticketState,
            string ticketData)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.TicketOwnerGuid = ticketOwnerGuid;
            this.Title = title;
            this.Date = date;
            this.TicketState = ticketState;
            this.TicketData = ticketData;
        }

        private TicketCreated(
            Guid ticketGuid,
            Guid ticketOwnerGuid,
            string title,
            string ticketData,
            DateTime date)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = ticketGuid;
            this.Title = title;
            this.Date = date;
            this.TicketOwnerGuid = ticketOwnerGuid;
            this.TicketData = ticketData;
            this.TicketState = AggregateStatus.Active.Value;
        }

        public static TicketCreated Initialize()
        {
            return new TicketCreated();
        }

        public TicketCreated WithAggregate(Guid aggregateGuid)
        {
            return new TicketCreated(
                aggregateGuid,
                this.TicketOwnerGuid,
                this.Title, this.TicketData,
                this.Date);
        }

        public TicketCreated WithTicketOwner(Guid ticketOwnerGuid)
        {
            return new TicketCreated(
                this.AggregateGuid,
                ticketOwnerGuid, this.Title,
                this.TicketData, this.Date);
        }

        public TicketCreated WithTicket(string title, string ticketData)
        {
            return new TicketCreated(
                this.AggregateGuid,
                this.TicketOwnerGuid, title,
                ticketData, this.Date);
        }

        public TicketCreated WithDate(DateTime date)
        {
            return new TicketCreated(this.AggregateGuid, this.TicketOwnerGuid,
                this.Title, this.TicketData, date);
        }
    }
}