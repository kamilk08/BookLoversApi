using System;
using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Application.Contracts.Tokens;
using BookLovers.Auth.Infrastructure.Dtos.Tokens;
using BookLovers.Auth.Infrastructure.Queries.Tokens;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using Microsoft.Owin.Security;

namespace BookLovers.Middleware.OAuth
{
    public class CustomOAuthTokenFormatProvider : ISecureDataFormat<AuthenticationTicket>
    {
        private const string ClientId = "clientId";
        private const string SigningKey = "signingKey";
        private readonly IAppManager _appManager;
        private readonly ITokenManager _tokenManager;
        private readonly IModule<AuthModule> _authModule;

        public CustomOAuthTokenFormatProvider(
            IAppManager appManager,
            ITokenManager tokenManager,
            IModule<AuthModule> authModule)
        {
            _appManager = appManager;
            _tokenManager = tokenManager;
            _authModule = authModule;
        }

        public string Protect(AuthenticationTicket data)
        {
            var clientId = GetClientId(data);

            var signingKey = _appManager.GetConfigValue(JwtSettings.JsonWebTokenKey);

            if (clientId.IsEmpty() || signingKey.IsEmpty())
                throw new UnauthorizedAccessException();

            var issuer = _appManager.GetConfigValue(JwtSettings.Issuer);
            var tokenProperties = GetAccessTokenProperties(data, issuer);

            var command = new CreateAccessTokenCommand(tokenProperties);

            Task.Run(async () => await _authModule.SendCommandAsync(command)).Wait();

            return _tokenManager.GetToken(command.TokenGuid);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            if (protectedText.IsEmpty())
                throw new UnauthorizedAccessException("Invalid token");

            var issuer = _appManager.GetConfigValue(JwtSettings.Issuer);
            var query = new GetClaimsIdentityFromProtectedTokenQuery(protectedText, issuer);

            QueryResult<GetClaimsIdentityFromProtectedTokenQuery, ClaimsIdentityDto>
                queryResult = null;

            Task.Run(async () =>
                queryResult = await _authModule
                    .ExecuteQueryAsync<GetClaimsIdentityFromProtectedTokenQuery, ClaimsIdentityDto>(query)).Wait();

            var properties = new AuthenticationProperties();

            if (queryResult != null)
            {
                properties.IssuedUtc = queryResult.Value.IssuedAtUtc;
                properties.ExpiresUtc = queryResult.Value.ExpiresAtUtc;
            }

            return new AuthenticationTicket(queryResult.Value.ClaimsIdentity, properties);
        }

        private string GetClientId(AuthenticationTicket ticket)
        {
            return ticket.Properties.Dictionary["clientId"];
        }

        private AccessTokenProperties GetAccessTokenProperties(
            AuthenticationTicket ticket,
            string issuer)
        {
            return new AccessTokenProperties()
            {
                SigningKey = _appManager.GetConfigValue(JwtSettings.JsonWebTokenKey),
                AudienceId = GetClientId(ticket),
                Claims = ticket.Identity.Claims,
                IssuedAt = ticket.Properties.IssuedUtc,
                ExpiresAt = ticket.Properties.ExpiresUtc,
                Issuer = issuer
            };
        }
    }
}