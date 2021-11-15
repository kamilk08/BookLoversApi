using System.Web.Http.Controllers;

namespace BookLovers.Responses
{
    public static class ResponsesExtensions
    {
        internal static void BadRequestResponse(this HttpActionContext ctx, string reason)
        {
            ctx.Response = new BadRequestResponse(ctx.Request, reason);
        }

        internal static void UnAuthorizedResponse(this HttpActionContext ctx, string reason = null)
        {
            ctx.Response = new UnauthorizedResponse(ctx.Request, reason);
        }

        internal static void ForbiddenResponse(this HttpActionContext ctx, string reason = null)
        {
            ctx.Response = new ForbiddenResponse(ctx.Request, reason);
        }

        internal static void NotFoundResponse(this HttpActionContext ctx)
        {
            ctx.Response = new NotFoundResponse(ctx.Request);
        }

        internal static void NotAllowed(this HttpActionContext ctx)
        {
            ctx.Response = new NotAllowedResponse(ctx.Request);
        }
    }
}