using System;
using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Domain.Tokens.Services;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Tokens;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace BookLovers.Middleware.OAuth
{
    public class CustomOAuthRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly IModule<AuthModule> _authModule;
        private readonly IAppManager _appManager;
        private readonly ITokenManager _tokenManager;

        public CustomOAuthRefreshTokenProvider(
            IModule<AuthModule> authModule,
            IAppManager appManager,
            ITokenManager tokenManager)
        {
            _authModule = authModule;
            _appManager = appManager;
            _tokenManager = tokenManager;
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var configValue = _appManager.GetConfigValue(JwtSettings.Issuer);

            var audience = context.OwinContext.Get<AudienceReadModel>("audience");

            var refreshTokenProperties = CreateRefreshTokenProperties(context, audience, configValue);

            var validationResult =
                await _authModule.SendCommandAsync(new CreateRefreshTokenCommand(refreshTokenProperties));

            var tokenAsync = await _tokenManager.GetTokenAsync(refreshTokenProperties.TokenGuid);

            context.Ticket.Properties.ExpiresUtc =
                new DateTimeOffset?(new DateTimeOffset(DateTime.UtcNow.AddMinutes(audience.RefreshTokenLifeTime)));

            context.SetToken(tokenAsync);
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var audienceReadModel = context.OwinContext.Get<AudienceReadModel>("audience");

            var tokenProperties = new RefreshTokenProperties()
            {
                AudienceGuid = audienceReadModel.AudienceGuid,
                Issuer = _appManager.GetConfigValue(JwtSettings.Issuer),
                SigningKey = _appManager.GetConfigValue(JwtSettings.JsonWebTokenKey),
                RefreshTokenLifeTime = audienceReadModel.RefreshTokenLifeTime
            };

            var refreshTokenResult = await _authModule
                .ExecuteQueryAsync<GetRefreshTokenBasedOnProtectedTokenQuery, RefreshTokenReadModel>(
                    new GetRefreshTokenBasedOnProtectedTokenQuery(context.Token, tokenProperties));

            if (refreshTokenResult.Value == null || refreshTokenResult.HasErrors)
            {
                context.Response.Headers.Add(
                    OAuthCustomErrors.AuthenticationFailure,
                    new string[] { OAuthCustomErrors.AuthenticationFailure });
                return;
            }

            var isTokenValidQuery =
                await _authModule.ExecuteQueryAsync<IsRefreshTokenValidQuery, bool>(
                    new IsRefreshTokenValidQuery(refreshTokenResult.Value, context.Token));

            if (!isTokenValidQuery.Value || isTokenValidQuery.HasErrors)
            {
                context.Response.Headers.Add(
                    OAuthCustomErrors.AuthenticationFailure,
                    new string[] { OAuthCustomErrors.AuthenticationFailure });

                return;
            }

            context.OwinContext.Set<string>("protectedToken", context.Token);
            context.OwinContext.Set<RefreshTokenReadModel>("refreshToken", refreshTokenResult.Value);
            context.DeserializeTicket(refreshTokenResult.Value.ProtectedTicket);
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(refreshTokenResult.Value.Expires);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
        }

        private AudienceReadModel GetAudience(IOwinContext context)
        {
            return context.Get<AudienceReadModel>("audience");
        }

        private RefreshTokenProperties CreateRefreshTokenProperties(
            AuthenticationTokenCreateContext context,
            AudienceReadModel audience,
            string issuer)
        {
            return new RefreshTokenProperties()
            {
                AudienceGuid = audience.AudienceGuid,
                Email = context.Ticket.Identity.FindFirst(p => p.Type == "email").Value,
                IssuedAt = context.Ticket.Properties.IssuedUtc,
                Issuer = issuer,
                RefreshTokenLifeTime = audience.RefreshTokenLifeTime,
                SerializedTicket = context.SerializeTicket(),
                SigningKey = _appManager.GetConfigValue(JwtSettings.JsonWebTokenKey)
            };
        }
    }
}