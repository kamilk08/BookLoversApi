using BookLovers.Base.Domain.Rules;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Domain.Librarians.BusinessRules
{
    internal class TicketDecisionMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid ticket decision";

        private readonly IDecisionChecker _checker;
        private readonly Ticket _ticket;

        public TicketDecisionMustBeValid(Ticket ticket, IDecisionChecker checker)
        {
            this._checker = checker;
            this._ticket = ticket;
        }

        public bool IsFulfilled()
        {
            return this._checker.IsDecisionValid(this._ticket.Decision.Value);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}