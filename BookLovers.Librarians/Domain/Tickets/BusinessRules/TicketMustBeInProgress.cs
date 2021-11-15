using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.Tickets.BusinessRules
{
    internal class TicketMustBeInProgress : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Ticket state is in invalid state.";

        private readonly Ticket _ticket;

        public TicketMustBeInProgress(Ticket ticket)
        {
            this._ticket = ticket;
        }

        public bool IsFulfilled()
        {
            return this._ticket.TicketState == TicketState.InProgress;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}