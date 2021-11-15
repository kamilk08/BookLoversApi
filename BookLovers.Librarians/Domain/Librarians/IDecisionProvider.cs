namespace BookLovers.Librarians.Domain.Librarians
{
    public interface IDecisionProvider
    {
        Decision GetDecision(int decisionType);
    }
}