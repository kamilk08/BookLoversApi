using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Audiences;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Audiences;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace BookLovers.Middleware.OAuth
{
    public class CustomOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IModule<AuthModule> _module;

        public CustomOAuthAuthorizationServerProvider(IModule<AuthModule> module)
        {
            _module = module;
        }

        public override async Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            var clientId = string.Empty;
            var clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
                context.TryGetFormCredentials(out clientId, out clientSecret);

            if (clientId.IsEmpty() || clientSecret.IsEmpty())
            {
                context.Response.Headers.Add(
                    OAuthCustomErrors.AuthenticationFailure,
                    new string[] { OAuthCustomErrors.AuthenticationFailure });

                return;
            }

            var command = new AuthenticateAudienceCommand(clientId, clientSecret);
            await _module.SendCommandAsync(command);

            if (!command.IsAuthenticated)
            {
                context.Response.Headers.Add(
                    OAuthCustomErrors.AuthenticationFailure,
                    new string[] { OAuthCustomErrors.AuthenticationFailure });

                return;
            }

            var queryResult =
                await _module.ExecuteQueryAsync<GetAudienceQuery, AudienceReadModel>(new GetAudienceQuery(clientId));

            if (queryResult.HasErrors || queryResult.Value == null)
            {
                context.Response.Headers.Add(
                    OAuthCustomErrors.AuthenticationFailure,
                    new string[] { OAuthCustomErrors.AuthenticationFailure });

                return;
            }

            context.OwinContext.Set<AudienceReadModel>("audience", queryResult.Value);
            context.OwinContext.Set<string>("refreshTokenLifeTime", queryResult.Value.RefreshTokenLifeTime.ToString());
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                AddAccessControlHeader(context.Response);

            var command = new AuthenticateUserCommand(context.UserName, context.Password);
            await _module.SendCommandAsync(command);

            if (!command.IsAuthenticated)
            {
                context.Response.Headers.Add(
                    OAuthCustomErrors.AuthenticationFailure,
                    new string[] { OAuthCustomErrors.AuthenticationFailure });

                return;
            }

            var queryResult =
                await _module.ExecuteQueryAsync<GetUserClaimsQuery, ClaimsIdentity>(
                    new GetUserClaimsQuery(context.UserName));

            if (queryResult.HasErrors || queryResult.Value == null)
            {
                context.Response.Headers.Add(
                    OAuthCustomErrors.AuthenticationFailure,
                    new string[] { OAuthCustomErrors.AuthenticationFailure });

                return;
            }

            var dict = new Dictionary<string, string> { { "clientId", context.ClientId } };

            var authenticationProperties = new AuthenticationProperties(dict);

            var ticket = new AuthenticationTicket(queryResult.Value, authenticationProperties);

            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            GetRefreshTokenFromOwinContext(context.OwinContext);

            GetProtectedTokenFromOwinContext(context.OwinContext);

            if (context.Ticket.Properties.Dictionary["clientId"] != context.ClientId)
            {
                context.SetError("clientId", "Invalid clientId.");
                context.Rejected();
            }

            var ticket = new AuthenticationTicket(
                new ClaimsIdentity(context.Ticket.Identity.Claims, "Bearer"),
                context.Ticket.Properties);

            context.Validated(ticket);

            return Task.CompletedTask;
        }

        private RefreshTokenReadModel GetRefreshTokenFromOwinContext(
            IOwinContext context)
        {
            return context.Get<RefreshTokenReadModel>("refreshToken");
        }

        private string GetProtectedTokenFromOwinContext(IOwinContext context)
        {
            return context.Get<string>("protectedToken");
        }

        private void AddAccessControlHeader(IOwinResponse owinResponse)
        {
            owinResponse.Headers.Add("Access-Control-Allow-Origin", new string[] { "*" });
        }
    }
}