using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Base.Domain.Rules
{
    public class AggregateMustBeArchived : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Cannot change state of an aggregate that was already archived.";

        private readonly int _status;

        public AggregateMustBeArchived(int status) => this._status = status;

        public bool IsFulfilled() => this._status == AggregateStatus.Archived.Value;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}