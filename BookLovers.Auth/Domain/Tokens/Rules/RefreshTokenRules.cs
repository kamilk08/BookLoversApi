using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Tokens.Rules
{
    internal sealed class RefreshTokenRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RefreshTokenRules(
            RefreshToken refreshToken,
            Guid userGuid,
            RefreshTokenSecurity tokenSecurity)
        {
            FollowingRules.Add(new AggregateMustBeActive(refreshToken.Status));
            FollowingRules.Add(new TokenNeedsToBelongToUser(refreshToken, userGuid));
            FollowingRules.Add(new TokenSecurityMustBePresent(tokenSecurity));
            FollowingRules.Add(new TokenCannotBeRevoked(refreshToken));
            FollowingRules.Add(new TokenCannotBeExpired(refreshToken));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}