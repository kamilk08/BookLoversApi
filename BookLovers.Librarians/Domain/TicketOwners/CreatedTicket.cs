using System;
using BookLovers.Base.Domain.Entity;

namespace BookLovers.Librarians.Domain.TicketOwners
{
    public class CreatedTicket : IEntityObject
    {
        public Guid TicketGuid { get; private set; }

        public bool IsSolved { get; private set; }

        private CreatedTicket()
        {
        }

        public CreatedTicket(Guid ticketGuid, bool isSolved)
        {
            TicketGuid = ticketGuid;
            IsSolved = isSolved;
        }

        public void MarkAsSolved() => IsSolved = true;
    }
}