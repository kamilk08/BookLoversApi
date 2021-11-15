using System.Collections.Generic;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.BusinessRules;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Tokens.Rules
{
    internal sealed class RevokeTokenRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public RevokeTokenRules(RefreshToken refreshToken, User user)
        {
            FollowingRules.Add(new UserMustBeAvailable(user));
            FollowingRules.Add(new AggregateMustBeActive(refreshToken.Status));
            FollowingRules.Add(new TokenNeedsToBelongToUser(refreshToken, user.Guid));
            FollowingRules.Add(new TokenCannotBeRevoked(refreshToken));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}