namespace BookLovers.Librarians.Domain.Librarians
{
    public interface IDecisionChecker
    {
        bool IsDecisionValid(int decisionId);
    }
}