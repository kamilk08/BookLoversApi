using System;

namespace BookLovers.Librarians.Application.WriteModels
{
    public class ResolveTicketWriteModel
    {
        public Guid LibrarianGuid { get; set; }

        public Guid TicketGuid { get; set; }

        public int DecisionType { get; set; }

        public int TicketConcern { get; set; }

        public string DecisionJustification { get; set; }

        public DateTime Date { get; set; }
    }
}