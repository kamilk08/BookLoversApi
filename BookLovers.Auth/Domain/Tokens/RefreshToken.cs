using System;
using BookLovers.Auth.Domain.Tokens.Rules;
using BookLovers.Auth.Domain.Users;
using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Auth.Domain.Tokens
{
    public class RefreshToken : AggregateRoot
    {
        public RefreshTokenIdentification TokenIdentification { get; private set; }

        public RefreshTokenDetails TokenDetails { get; private set; }

        public RefreshTokenSecurity TokenSecurity { get; private set; }

        public bool HasBeenRevoked => TokenDetails.RevokedAt.HasValue;

        public bool HasExpired => DateTime.UtcNow > TokenDetails.ExpiresAt;

        private RefreshToken()
        {
        }

        private RefreshToken(
            Guid guid,
            RefreshTokenIdentification identification,
            RefreshTokenSecurity security,
            RefreshTokenDetails details)
        {
            Guid = guid;
            TokenIdentification = identification;
            TokenDetails = details;
            TokenSecurity = security;
            Status = AggregateStatus.Active.Value;
        }

        public static RefreshToken Create(
            Guid guid,
            RefreshTokenIdentification identification,
            RefreshTokenSecurity tokenSecurity,
            RefreshTokenDetails details)
        {
            return new RefreshToken(guid, identification, tokenSecurity, details);
        }

        public void Revoke(User user)
        {
            CheckBusinessRules(new RevokeTokenRules(this, user));

            TokenDetails = TokenDetails.SetRevokedDate();
        }

        public void Refresh(User user, RefreshTokenSecurity tokenSecurity)
        {
            CheckBusinessRules(new RefreshTokenRules(this, user.Guid, tokenSecurity));

            TokenSecurity = tokenSecurity;
        }

        internal bool CanBeRefreshed() => DateTime.UtcNow < TokenDetails.ExpiresAt;
    }
}