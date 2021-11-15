using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using BookLovers.Base.Domain.Rules;
using BookLovers.Results;

namespace BaseTests.EndToEndHelpers
{
    public class FakeExceptionHandler : IExceptionHandler
    {
        private readonly FakeLogger _logger;

        public FakeExceptionHandler(FakeLogger logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            _logger.GetLogger()
                .Error(context.Exception, $"Global exception handler. Exception:{context.Exception.Message}. InnerException:{context.Exception.InnerException}. StackTrace:{context.Exception.StackTrace}");

            await HandleExceptionAsync(context);
        }

        private Task HandleExceptionAsync(ExceptionHandlerContext ctx)
        {
            switch (ctx.Exception)
            {
                case BusinessRuleNotMetException _:
                    ctx.ConflictResult();
                    break;
                case UnauthorizedAccessException _:
                    ctx.UnauthorizedResult();
                    break;
                default:
                    return ctx.InternalServerErrorResult();
            }

            return Task.CompletedTask;
        }
    }
}