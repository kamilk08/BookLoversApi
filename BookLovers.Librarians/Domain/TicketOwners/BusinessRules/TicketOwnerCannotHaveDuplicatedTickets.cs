using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Domain.TicketOwners.BusinessRules
{
    internal class TicketOwnerCannotHaveDuplicatedTickets : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Ticket cannot be duplicated.";

        private readonly TicketOwner _ticketOwner;
        private readonly Ticket _ticket;

        public TicketOwnerCannotHaveDuplicatedTickets(TicketOwner ticketOwner, Ticket ticket)
        {
            _ticketOwner = ticketOwner;
            _ticket = ticket;
        }

        public bool IsFulfilled()
        {
            return _ticketOwner.Tickets
                .All(a => a.TicketGuid != _ticket.Guid);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}