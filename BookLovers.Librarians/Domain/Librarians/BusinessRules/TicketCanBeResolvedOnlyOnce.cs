using BookLovers.Base.Domain.Rules;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Domain.Librarians.BusinessRules
{
    internal class TicketCanBeResolvedOnlyOnce : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Ticket can be resolved only once.";

        private readonly Librarian _librarian;
        private readonly Ticket _ticket;

        public TicketCanBeResolvedOnlyOnce(Librarian librarian, Ticket ticket)
        {
            this._librarian = librarian;
            this._ticket = ticket;
        }

        public bool IsFulfilled()
        {
            return this._ticket != null &&
                   !this._librarian.HasResolvedTicket(this._ticket.Guid) &&
                   !this._ticket.IsSolved();
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}