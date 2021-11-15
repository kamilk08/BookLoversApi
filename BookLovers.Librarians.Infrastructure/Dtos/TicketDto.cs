using System;

namespace BookLovers.Librarians.Infrastructure.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime TicketDate { get; set; }

        public int TicketConcern { get; set; }

        public int TicketDecision { get; set; }

        public int TicketState { get; set; }

        public Guid TicketObjectGuid { get; set; }

        public string TicketData { get; set; }

        public Guid SolvedByGuid { get; set; }

        public Guid TicketOwnerGuid { get; set; }

        public int TicketOwnerId { get; set; }
    }
}