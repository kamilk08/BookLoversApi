using System;
using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.TicketOwners.BusinessRules
{
    internal class OwnerMustHaveSelectedTicket : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Owner must have selected ticket";

        private readonly TicketOwner _ticketOwner;
        private readonly Guid _ticketGuid;

        public OwnerMustHaveSelectedTicket(TicketOwner ticketOwner, Guid ticketGuid)
        {
            _ticketOwner = ticketOwner;
            _ticketGuid = ticketGuid;
        }

        public bool IsFulfilled() => _ticketOwner.Tickets.Any(a => a.TicketGuid == _ticketGuid);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}