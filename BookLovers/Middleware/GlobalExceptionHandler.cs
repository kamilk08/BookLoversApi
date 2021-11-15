using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using BookLovers.Base.Domain.Rules;
using BookLovers.Results;
using Serilog;

namespace BookLovers.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;

        public GlobalExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(
            ExceptionHandlerContext context,
            CancellationToken cancellationToken)
        {
            _logger.Error(
                context.Exception,
                $"Global exception handler. Exception:{context.Exception.Message}. InnerException:{context.Exception.InnerException}. StackTrace:{context.Exception.StackTrace}");

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