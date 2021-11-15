using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Infrastructure;
using BookLovers.Responses;

namespace BookLovers.Filters
{
    public class CheckCredentials : ActionFilterAttribute
    {
        private const string RequestParameterName = "writeModel";

        public override async Task OnActionExecutingAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            var module =
                actionContext.Request.GetDependencyScope().GetService(typeof(IModule<AuthModule>)) as
                    IModule<AuthModule>;

            var actionArgument = actionContext.ActionArguments[RequestParameterName];

            if (actionArgument == null)
            {
                actionContext.BadRequestResponse("Bad request");
                return;
            }

            dynamic dto = actionContext.ActionArguments[RequestParameterName];

            var emailClaim = FindEmailInClaims(actionContext);

            if (emailClaim == null)
            {
                actionContext.ForbiddenResponse();
                return;
            }

            if (emailClaim.Value != dto.Email)
            {
                actionContext.ForbiddenResponse();
                return;
            }

            var command = new CheckCredentialsCommand(dto.Email, dto.Password);

            await module.SendCommandAsync(command);

            if (!command.IsAuthenticated)
                actionContext.UnAuthorizedResponse("Invalid credentials.");

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        private Claim FindEmailInClaims(HttpActionContext actionContext)
        {
            var claims = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            var email = claims?.FindFirst(p => p.Type == ClaimTypes.Email);
            return email;
        }
    }
}