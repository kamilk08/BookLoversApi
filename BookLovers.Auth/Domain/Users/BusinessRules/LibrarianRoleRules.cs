using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal sealed class LibrarianRoleRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public string BrokenRuleMessage => Message;

        public LibrarianRoleRules(User user)
        {
            FollowingRules.Add(new AggregateMustBeActive(user.Status));
            FollowingRules.Add(new AccountCannotBeBlockedRule(user.Account));
            FollowingRules.Add(new UserMustBeAReader(user));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();
    }
}