using BookLovers.Base.Domain.Rules;
using BookLovers.Librarians.Domain.Tickets.Services;

namespace BookLovers.Librarians.Domain.Tickets.BusinessRules
{
    internal class TicketConcernMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Ticket concern is not valid.";

        private readonly ITicketConcernChecker _checker;
        private readonly Ticket _ticket;

        public TicketConcernMustBeValid(ITicketConcernChecker checker, Ticket ticket)
        {
            this._checker = checker;
            this._ticket = ticket;
        }

        public bool IsFulfilled()
        {
            return !(this._ticket.TicketContent.TicketConcern == null) &&
                   this._checker.IsConcernValid(this._ticket.TicketContent.TicketConcern.Value);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}