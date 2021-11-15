using System;

namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class CreatedTicketReadModel
    {
        public int Id { get; set; }

        public Guid TicketGuid { get; set; }

        public bool IsSolved { get; set; }
    }
}