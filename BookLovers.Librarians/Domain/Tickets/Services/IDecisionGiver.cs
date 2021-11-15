using BookLovers.Librarians.Domain.Librarians;

namespace BookLovers.Librarians.Domain.Tickets.Services
{
    public interface IDecisionGiver
    {
        Decision Decision { get; }

        DecisionMade GiveDecision();
    }
}