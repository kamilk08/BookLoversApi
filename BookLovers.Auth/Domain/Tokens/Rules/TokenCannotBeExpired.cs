using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Tokens.Rules
{
    internal class TokenCannotBeExpired : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Token cannot be expired";

        private readonly RefreshToken _refreshToken;

        public TokenCannotBeExpired(RefreshToken refreshToken)
        {
            _refreshToken = refreshToken;
        }

        public bool IsFulfilled()
        {
            return DateTime.UtcNow < _refreshToken.TokenDetails.ExpiresAt;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}