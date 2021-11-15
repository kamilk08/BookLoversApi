using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.Tickets.BusinessRules
{
    internal class TicketCannotBeSolvedTwiceOrMore : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Ticket cannot be solved by anyone.";

        private readonly Ticket _ticket;

        public TicketCannotBeSolvedTwiceOrMore(Ticket ticket)
        {
            this._ticket = ticket;
        }

        public bool IsFulfilled() => !this._ticket.IsSolved();

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}