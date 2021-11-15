using System;
using System.Collections.Generic;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.BusinessRules;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.PasswordResets
{
    internal sealed class GeneratePasswordResetTokenRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public GeneratePasswordResetTokenRules(User user, TimeSpan resetTime)
        {
            FollowingRules.Add(new AggregateMustExist(user.Guid));
            FollowingRules.Add(new AggregateMustBeActive(user.Status));
            FollowingRules.Add(new AccountCannotBeBlockedRule(user.Account));
            FollowingRules.Add(new AccountMustBeConfirmed(user.Account));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}