using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace BookLovers.Auth.Application.Contracts.Tokens
{
    public interface ITokenDescriptor
    {
    }

    public interface ITokenDescriptor<T> : ITokenDescriptor
    {
        ITokenDescriptor<T> AddIssuer(string issuer);

        ITokenDescriptor<T> AddAudience(string audience);

        ITokenDescriptor<T> AddIssuedAndExpiresDate(
            DateTimeOffset issuedAt,
            DateTimeOffset expires);

        ITokenDescriptor<T> AddClaims(IEnumerable<Claim> claims);

        ITokenDescriptor<T> AddSigningCredentials(string secretKey);

        T GetTokenDescriptor();
    }
}