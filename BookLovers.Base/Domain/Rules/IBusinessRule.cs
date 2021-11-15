namespace BookLovers.Base.Domain.Rules
{
    public interface IBusinessRule
    {
        bool IsFulfilled();

        string BrokenRuleMessage { get; }
    }
}