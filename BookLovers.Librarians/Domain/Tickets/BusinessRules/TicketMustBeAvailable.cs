using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.Tickets.BusinessRules
{
    public class TicketMustBeAvailable : IBusinessRule
    {
        private readonly Ticket _ticket;

        public TicketMustBeAvailable(Ticket ticket)
        {
            this._ticket = ticket;
        }

        public bool IsFulfilled()
        {
            return this._ticket != null && this._ticket.Status == AggregateStatus.Active.Value;
        }

        public string BrokenRuleMessage => "Ticket is not available.";
    }
}