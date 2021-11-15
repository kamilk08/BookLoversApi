using System;

namespace BookLovers.Librarians.Domain.Tickets.Services.Factories
{
    public class TicketDetailsData
    {
        public DateTime CreatedAt { get; }

        public int TicketConcern { get; }

        public string Description { get; }

        public TicketDetailsData(DateTime createdAt, int ticketConcern, string description)
        {
            this.CreatedAt = createdAt;
            this.TicketConcern = ticketConcern;
            this.Description = description;
        }
    }
}