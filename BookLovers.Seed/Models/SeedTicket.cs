using System;

namespace BookLovers.Seed.Models
{
    public class SeedTicket
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public int TicketConcern { get; set; }

        public string TicketData { get; set; }
    }
}