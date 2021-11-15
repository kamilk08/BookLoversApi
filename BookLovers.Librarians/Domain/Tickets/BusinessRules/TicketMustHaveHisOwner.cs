using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.Tickets.BusinessRules
{
    internal class TicketMustHaveHisOwner : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Ticket must have his owner.";

        private readonly Ticket _ticket;

        public TicketMustHaveHisOwner(Ticket ticket)
        {
            this._ticket = ticket;
        }

        public bool IsFulfilled()
        {
            return this._ticket.IssuedBy.HasOwner();
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}