using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BookLovers.Responses;
using BookLovers.Services;

namespace BookLovers.Filters
{
    public class HasPermissionAttribute : ActionFilterAttribute
    {
        private readonly string _permissionName;

        public HasPermissionAttribute(string permissionName)
        {
            _permissionName = permissionName;
        }

        public override async Task OnActionExecutingAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            var factory = actionContext.GetDependency<AuthorizationHandlerFactory>() as AuthorizationHandlerFactory;

            var handler = factory.GetAuthorizationHandler(_permissionName);

            if (!await handler.HasPermission(actionContext))
                actionContext.Response = new ForbiddenResponse(actionContext.Request, "Forbidden");

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}