using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.TicketOwners.BusinessRules
{
    internal class TicketMustBeIssuedByTicketOwner : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid owner of a ticket.";

        private readonly TicketOwner _ticketOwner;
        private readonly Guid _issuedByGuid;

        public TicketMustBeIssuedByTicketOwner(TicketOwner ticketOwner, Guid issuedByGuid)
        {
            _ticketOwner = ticketOwner;
            _issuedByGuid = issuedByGuid;
        }

        public bool IsFulfilled() => _ticketOwner.ReaderGuid == _issuedByGuid;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}