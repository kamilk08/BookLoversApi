using System;
using BookLovers.Base.Domain.Entity;

namespace BookLovers.Librarians.Domain.Librarians
{
    public class ResolvedTicket : IEntityObject
    {
        public int Id { get; private set; }

        public Guid TicketGuid { get; private set; }

        public Decision Decision { get; private set; }

        public DecisionJustification Justification { get; private set; }

        private ResolvedTicket()
        {
        }

        public ResolvedTicket(Guid ticketGuid, Decision decision, DecisionJustification justification)
        {
            this.TicketGuid = ticketGuid;
            this.Decision = decision;
            this.Justification = justification;
        }

        public bool IsApproved()
        {
            return this.Decision == Decision.Approve;
        }

        public bool HasJustification()
        {
            return this.Justification.Content != string.Empty;
        }
    }
}