using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Tokens.Rules
{
    internal class TokenNeedsToBelongToUser : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Token does not belong to selected user.";

        private readonly RefreshToken _refreshToken;
        private readonly Guid _userGuid;

        public TokenNeedsToBelongToUser(RefreshToken refreshToken, Guid userGuid)
        {
            _userGuid = userGuid;
            _refreshToken = refreshToken;
        }

        public bool IsFulfilled() => _refreshToken.TokenIdentification.UserGuid
                                     == _userGuid;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}