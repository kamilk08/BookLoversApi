using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal sealed class AccountConfirmationRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public AccountConfirmationRules(Account account)
        {
            FollowingRules.Add(new AccountCannotBeBlockedRule(account));
            FollowingRules.Add(new AccountCannotBeConfirmed(account));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}