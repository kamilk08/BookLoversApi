using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.Tickets.Services;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class TicketDecisionAcceptedGiver : IDecisionGiver
    {
        public Decision Decision => Decision.Approve;

        public DecisionMade GiveDecision() => DecisionMade.Accepted;
    }
}