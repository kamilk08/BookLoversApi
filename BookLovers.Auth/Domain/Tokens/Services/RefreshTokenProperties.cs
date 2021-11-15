using System;

namespace BookLovers.Auth.Domain.Tokens.Services
{
    public class RefreshTokenProperties
    {
        public string Issuer { get; set; }

        public Guid AudienceGuid { get; set; }

        public Guid TokenGuid { get; }

        public string Email { get; set; }

        public string SigningKey { get; set; }

        public DateTimeOffset? IssuedAt { get; set; }

        public DateTimeOffset? ExpiresAt => DateTimeOffset.UtcNow.AddMinutes(RefreshTokenLifeTime);

        public long RefreshTokenLifeTime { get; set; }

        public string SerializedTicket { get; set; }

        public RefreshTokenProperties() => TokenGuid = Guid.NewGuid();
    }
}