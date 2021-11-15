using System;

namespace BookLovers.Base.Domain.Rules
{
    public class AggregateMustExist : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Aggregate does not exist.";

        private readonly Guid _aggregateGuid;

        public AggregateMustExist(Guid aggregateGuid) => this._aggregateGuid = aggregateGuid;

        public bool IsFulfilled() => this._aggregateGuid != Guid.Empty;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}