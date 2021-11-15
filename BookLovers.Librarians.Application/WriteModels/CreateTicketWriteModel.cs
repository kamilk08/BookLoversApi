using System;

namespace BookLovers.Librarians.Application.WriteModels
{
    public class CreateTicketWriteModel
    {
        public int TicketId { get; set; }

        public Guid TicketObjectGuid { get; set; }

        public Guid TicketGuid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public int TicketConcern { get; set; }

        public string TicketData { get; set; }
    }
}