using System.Collections.Generic;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal sealed class ChangeEmailRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangeEmailRules(User user, IEmailUniquenessChecker uniquenessChecker, string email)
        {
            FollowingRules.Add(new AggregateMustBeActive(user.Status));
            FollowingRules.Add(new AccountCannotBeBlockedRule(user.Account));
            FollowingRules.Add(new EmailMustBeUnique(uniquenessChecker, email));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}