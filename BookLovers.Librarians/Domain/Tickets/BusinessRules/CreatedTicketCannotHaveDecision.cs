using BookLovers.Base.Domain.Rules;
using BookLovers.Librarians.Domain.Librarians;

namespace BookLovers.Librarians.Domain.Tickets.BusinessRules
{
    internal class CreatedTicketCannotHaveDecision : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Created ticket should have decision of type uknown.";

        private readonly Ticket _ticket;

        public CreatedTicketCannotHaveDecision(Ticket ticket)
        {
            this._ticket = ticket;
        }

        public bool IsFulfilled()
        {
            return this._ticket.Decision == Decision.Unknown;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}