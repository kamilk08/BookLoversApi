using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using BookLovers.Base.Infrastructure.Validation;
using Newtonsoft.Json;

namespace BookLovers.Results
{
    public static class ResultsExtensions
    {
        public static IHttpActionResult BadRequest(
            this ApiController controller,
            IEnumerable<ValidationError> errors)
        {
            return new BadRequestResult(controller.Request, errors);
        }

        public static Task UnauthorizedResult(this ExceptionHandlerContext context)
        {
            context.Result = new UnauthorizedResult(context.Request, context.Exception.Message);

            return Task.CompletedTask;
        }

        public static Task ConflictResult(this ExceptionHandlerContext context)
        {
            context.Result = new ConflictResult(context.Request, context.Exception.Message);

            return Task.CompletedTask;
        }

        public static Task InternalServerErrorResult(this ExceptionHandlerContext context)
        {
            context.Result = new InternalServerErrorResult(context.Request, context.Exception.Message);

            return Task.CompletedTask;
        }

        public static IHttpActionResult NotFoundResult(this ApiController controller)
        {
            return new NotFoundResult(controller.Request);
        }

        private static string CreatePayload(HttpStatusCode statusCode, string message)
        {
            return JsonConvert.SerializeObject(new
            {
                code = statusCode,
                message = message
            });
        }
    }
}