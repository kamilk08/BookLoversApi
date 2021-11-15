using System.Threading.Tasks;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.TicketReasons;

namespace BookLovers.Librarians.Domain.Librarians.DecisionNotifiers
{
    public interface IDecisionNotifier
    {
        ITicketSummary TicketSummary { get; }

        Task Notify(Ticket ticket, string justification);
    }
}