using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BookLovers.Filters
{
    public class ValidateModelState : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            var modelState = actionContext.ModelState;

            if (!modelState.IsValid)
                actionContext.Response =
                    actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);

            return Task.FromResult(actionContext.Response);
        }
    }
}