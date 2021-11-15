using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Tokens
{
    public class RefreshTokenDetails : ValueObject<RefreshTokenDetails>
    {
        public DateTime IssuedAt { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        public DateTime? RevokedAt { get; private set; }

        public string ProtectedTicket { get; private set; }

        private RefreshTokenDetails()
        {
        }

        public RefreshTokenDetails(DateTime issuedAt, DateTime expiresAt, string protectedTicket)
        {
            IssuedAt = issuedAt;
            ExpiresAt = expiresAt;
            ProtectedTicket = protectedTicket;
        }

        public RefreshTokenDetails(
            DateTime issuedAt,
            DateTime expiresAt,
            string protectedTicket,
            DateTime revokedAt)
            : this(issuedAt, expiresAt, protectedTicket)
        {
            RevokedAt = DateTime.UtcNow;
        }

        public RefreshTokenDetails SetRevokedDate() =>
            new RefreshTokenDetails(IssuedAt, ExpiresAt, ProtectedTicket, DateTime.UtcNow);

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.IssuedAt.GetHashCode();
            hash = (hash * 23) + this.ExpiresAt.GetHashCode();
            hash = (hash * 23) + this.ProtectedTicket.GetHashCode();
            hash = (hash * 23) + this.RevokedAt.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(RefreshTokenDetails obj)
        {
            return this.IssuedAt == obj.IssuedAt
                   && this.ExpiresAt == obj.ExpiresAt
                   && this.ProtectedTicket == obj.ProtectedTicket
                   && this.RevokedAt == obj.RevokedAt;
        }
    }
}