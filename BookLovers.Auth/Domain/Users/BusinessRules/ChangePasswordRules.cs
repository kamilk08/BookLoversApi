using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal sealed class ChangePasswordRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangePasswordRules(User user)
        {
            FollowingRules.Add(new AggregateMustBeActive(user.Status));
            FollowingRules.Add(new AccountCannotBeBlockedRule(user.Account));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}