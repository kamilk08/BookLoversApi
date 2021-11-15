using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace BookLovers.Auth.Application.Contracts.Tokens
{
    public class AccessTokenProperties
    {
        public string Issuer { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        public string AudienceId { get; set; }

        public string SigningKey { get; set; }

        public DateTimeOffset? IssuedAt { get; set; }

        public DateTimeOffset? ExpiresAt { get; set; }
    }
}