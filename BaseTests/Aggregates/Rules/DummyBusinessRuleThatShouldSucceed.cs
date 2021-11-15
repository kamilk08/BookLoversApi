using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BaseTests.Aggregates.Rules
{
    public class DummyBusinessRuleThatShouldSucceed : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public bool IsFulfilled()
        {
            return true;
        }

        public string BrokenRuleMessage { get; }
    }
}