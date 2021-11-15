using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;

namespace BookLovers.Middleware.OAuth
{
    public static class OAuthBearerAuthenticationMiddleware
    {
        public static IAppBuilder UseOAuthBearerAuthenticationMiddleware(
            this IAppBuilder appBuilder,
            IKernel kernel)
        {
            var appManager = kernel.Get<IAppManager>();
            var authModule = kernel.Get<IModule<AuthModule>>();
            var tokenManager = kernel.Get<ITokenManager>();

            var options = new OAuthBearerAuthenticationOptions
            {
                AuthenticationType = "Bearer",
                AccessTokenFormat = new CustomOAuthTokenFormatProvider(appManager, tokenManager, authModule)
            };

            return appBuilder.UseOAuthBearerAuthentication(options);
        }
    }
}