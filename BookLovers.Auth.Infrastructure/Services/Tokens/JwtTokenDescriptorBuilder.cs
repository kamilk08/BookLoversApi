using System;
using System.Collections.Generic;
using System.Security.Claims;
using BookLovers.Auth.Application.Contracts.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace BookLovers.Auth.Infrastructure.Services.Tokens
{
    public class JwtTokenDescriptorBuilder :
        ITokenDescriptor<SecurityTokenDescriptor>,
        ITokenDescriptor
    {
        private readonly SecurityTokenDescriptor _tokenDescriptor;

        public JwtTokenDescriptorBuilder() => _tokenDescriptor = new SecurityTokenDescriptor();

        public ITokenDescriptor<SecurityTokenDescriptor> AddIssuer(
            string issuer)
        {
            _tokenDescriptor.Issuer = issuer;

            return this;
        }

        public ITokenDescriptor<SecurityTokenDescriptor> AddAudience(
            string audience)
        {
            _tokenDescriptor.Audience = audience;

            return this;
        }

        public ITokenDescriptor<SecurityTokenDescriptor> AddIssuedAndExpiresDate(
            DateTimeOffset issuedAt,
            DateTimeOffset expires)
        {
            _tokenDescriptor.IssuedAt = issuedAt.UtcDateTime;
            _tokenDescriptor.Expires = expires.UtcDateTime;

            return this;
        }

        public ITokenDescriptor<SecurityTokenDescriptor> AddClaims(
            IEnumerable<Claim> claims)
        {
            _tokenDescriptor.Subject = new ClaimsIdentity(claims);

            return this;
        }

        public ITokenDescriptor<SecurityTokenDescriptor> AddSigningCredentials(
            string secretKey)
        {
            _tokenDescriptor.SigningCredentials = SigningCredentialsFactory.Create(secretKey);

            return this;
        }

        public SecurityTokenDescriptor GetTokenDescriptor() =>
            _tokenDescriptor;
    }
}