using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Base.Domain.Rules
{
    public class AggregateMustBeActive : IBusinessRule
    {
        private const string BrokenRuleErrorMessage =
            "Operation that was invoked on aggregate cannot be performed. Aggregate is not active.";

        private readonly int _status;

        public AggregateMustBeActive(int status) => this._status = status;

        public bool IsFulfilled() => this._status == AggregateStatus.Active.Value;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}