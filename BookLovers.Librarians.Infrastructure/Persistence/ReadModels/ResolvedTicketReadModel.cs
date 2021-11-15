using System;

namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class ResolvedTicketReadModel
    {
        public int Id { get; set; }

        public Guid TicketGuid { get; set; }

        public string Justification { get; set; }

        public DateTime Date { get; set; }

        public string DecisionName { get; set; }

        public byte DecisionValue { get; set; }
    }
}