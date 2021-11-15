using System;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;

namespace BookLovers.Middleware.OAuth
{
    public static class OAuthMiddleware
    {
        public static IAppBuilder UseOAuthServerAuthorizationMiddleware(
            this IAppBuilder appBuilder,
            IKernel kernel)
        {
            var appManager = kernel.Get<IAppManager>();
            var authModule = kernel.Get<IModule<AuthModule>>();
            var tokenManager = kernel.Get<TokenManager>();

            return appBuilder.UseOAuthAuthorizationServer(
                DefaultOptions(appManager, authModule, tokenManager));
        }

        public static IAppBuilder UseOAuthServerAuthorizationMiddleware(
            this IAppBuilder appBuilder,
            OAuthAuthorizationServerOptions options)
        {
            return appBuilder.UseOAuthAuthorizationServer(options);
        }

        private static OAuthAuthorizationServerOptions DefaultOptions(
            IAppManager appManager,
            IModule<AuthModule> authModule,
            TokenManager tokenManager)
        {
            return new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/auth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(15),
                Provider = new CustomOAuthAuthorizationServerProvider(authModule),
                AccessTokenFormat = new CustomOAuthTokenFormatProvider(appManager, tokenManager, authModule),
                RefreshTokenProvider = new CustomOAuthRefreshTokenProvider(authModule, appManager, tokenManager)
            };
        }
    }
}