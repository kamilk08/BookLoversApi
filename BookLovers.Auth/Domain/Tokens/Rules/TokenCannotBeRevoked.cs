using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Tokens.Rules
{
    internal class TokenCannotBeRevoked : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Cannot revoke token that is already marked as revoked.";

        private readonly RefreshToken _refreshToken;

        public TokenCannotBeRevoked(RefreshToken refreshToken)
        {
            _refreshToken = refreshToken;
        }

        public bool IsFulfilled() => !_refreshToken.HasBeenRevoked;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}