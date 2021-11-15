using System.Collections.Generic;

namespace BookLovers.Base.Domain.Rules
{
    public abstract class BaseBusinessRule
    {
        protected virtual string Message { get; set; }

        protected abstract List<IBusinessRule> FollowingRules { get; }

        protected virtual bool AreFollowingRulesBroken()
        {
            foreach (var followingRule in this.FollowingRules)
            {
                if (!followingRule.IsFulfilled())
                {
                    this.Message = followingRule.BrokenRuleMessage;
                    return true;
                }
            }

            return false;
        }
    }
}