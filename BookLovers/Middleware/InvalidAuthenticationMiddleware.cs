using System.Net;
using System.Threading.Tasks;
using BookLovers.Middleware.OAuth;
using Microsoft.Owin;

namespace BookLovers.Middleware
{
    public class InvalidAuthenticationMiddleware : OwinMiddleware
    {
        public InvalidAuthenticationMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (IsBadRequest(context) && HasAuthenticationFailureHeader(context))
            {
                context.Response.Headers.Remove(OAuthCustomErrors.AuthenticationFailure);
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            }
        }

        private bool IsBadRequest(IOwinContext context)
        {
            return context.Response.StatusCode == (int) HttpStatusCode.BadRequest;
        }

        private bool HasAuthenticationFailureHeader(IOwinContext context)
        {
            return context.Response.Headers.ContainsKey(OAuthCustomErrors.AuthenticationFailure);
        }
    }
}