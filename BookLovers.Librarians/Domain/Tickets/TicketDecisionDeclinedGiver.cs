using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.Tickets.Services;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class TicketDecisionDeclinedGiver : IDecisionGiver
    {
        public Decision Decision => Decision.Decline;

        public DecisionMade GiveDecision() => DecisionMade.Dismissed;
    }
}