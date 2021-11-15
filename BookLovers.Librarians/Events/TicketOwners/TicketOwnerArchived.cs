using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.TicketOwners
{
    public class TicketOwnerArchived : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public IEnumerable<Guid> UnSolvedTickets { get; }

        private TicketOwnerArchived()
        {
        }

        [JsonConstructor]
        protected TicketOwnerArchived(Guid guid, Guid aggregateGuid, IEnumerable<Guid> tickets)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.UnSolvedTickets = tickets;
        }

        public TicketOwnerArchived(
            Guid aggregateGuid,
            IEnumerable<Guid> tickets)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.UnSolvedTickets = tickets;
        }
    }
}