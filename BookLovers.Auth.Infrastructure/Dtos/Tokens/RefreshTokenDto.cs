using System;

namespace BookLovers.Auth.Infrastructure.Dtos.Tokens
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }

        public Guid TokenGuid { get; set; }
    }
}