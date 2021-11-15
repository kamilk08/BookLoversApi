using System;
using System.Security.Claims;

namespace BookLovers.Auth.Infrastructure.Dtos.Tokens
{
    public class ClaimsIdentityDto
    {
        public ClaimsIdentity ClaimsIdentity { get; set; }

        public DateTime IssuedAtUtc { get; set; }

        public DateTime ExpiresAtUtc { get; set; }
    }
}