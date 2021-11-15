using System.Collections.Generic;
using BookLovers.Auth.Domain.PasswordResets;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.BusinessRules
{
    internal sealed class ResetPasswordRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public string BrokenRuleMessage => Message;

        public ResetPasswordRules(User user, PasswordResetToken token)
        {
            FollowingRules.Add(new AggregateMustBeActive(user.Status));
            FollowingRules.Add(new AggregateMustBeActive(token.Status));
            FollowingRules.Add(new PasswordResetTokenCannotBeExpired(token));
            FollowingRules.Add(new AccountCannotBeBlockedRule(user.Account));
            FollowingRules.Add(new AccountMustBeConfirmed(user.Account));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();
    }
}