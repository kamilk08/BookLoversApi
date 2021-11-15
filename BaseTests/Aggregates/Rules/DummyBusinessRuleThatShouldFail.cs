using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BaseTests.Aggregates.Rules
{
    public class DummyBusinessRuleThatShouldFail : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public bool IsFulfilled()
        {
            return false;
        }

        public string BrokenRuleMessage => "Broken rule message";
    }
}