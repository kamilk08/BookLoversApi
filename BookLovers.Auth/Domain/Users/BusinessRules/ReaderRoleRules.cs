using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal sealed class ReaderRoleRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public string BrokenRuleMessage => Message;

        public ReaderRoleRules(User user)
        {
            FollowingRules.Add(new AggregateMustBeActive(user.Status));
            FollowingRules.Add(new UserMustBeAReader(user));
            FollowingRules.Add(new UserMustHaveMoreThenZeroRoles(user));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();
    }
}