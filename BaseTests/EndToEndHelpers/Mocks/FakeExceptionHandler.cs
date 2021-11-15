// Decompiled with JetBrains decompiler
// Type: BaseTests.EndToEndHelpers.Mocks.FakeExceptionHandler
// Assembly: BaseTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D6D40273-49ED-405F-84F1-EB85C3A44EE0
// Assembly location: C:\Users\Kamil\source\repos\BookLovers\BaseTests\bin\Release\BaseTests.dll

using BookLovers.Base.Domain.Rules;
using BookLovers.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace BaseTests.EndToEndHelpers.Mocks
{
  public class FakeExceptionHandler : IExceptionHandler
  {
    private readonly FakeLogger _logger;

    public FakeExceptionHandler(FakeLogger logger) => this._logger = logger;

    public async Task HandleAsync(
      ExceptionHandlerContext context,
      CancellationToken cancellationToken)
    {
      this._logger.GetLogger().Error(context.Exception, string.Format("Global exception handler. Exception:{0}. InnerException:{1}. StackTrace:{2}", (object) context.Exception.Message, (object) context.Exception.InnerException, (object) context.Exception.StackTrace));
      await this.HandleExceptionAsync(context);
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
