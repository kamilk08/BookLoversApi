using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.Tickets
{
    public class TicketArchived : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        private TicketArchived()
        {
        }

        [JsonConstructor]
        public TicketArchived(Guid guid, Guid aggregateGuid)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
        }

        public TicketArchived(Guid aggregateGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
        }
    }
}