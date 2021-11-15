using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Tokens.Rules
{
    internal class TokenSecurityMustBePresent : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Token secuity must be present";

        private readonly RefreshTokenSecurity _tokenSecurity;

        public TokenSecurityMustBePresent(RefreshTokenSecurity tokenSecurity)
        {
            _tokenSecurity = tokenSecurity;
        }

        public bool IsFulfilled() => _tokenSecurity != null;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}