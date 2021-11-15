using BookLovers.Base.Infrastructure;
using Ninject.Modules;
using Serilog;

namespace BookLovers.Bookcases.Infrastructure.Root.Logging
{
    internal class LoggingModule : NinjectModule
    {
        private readonly ILogger _logger;

        public LoggingModule(ILogger logger)
        {
            _logger = logger;
        }

        public override void Load()
        {
            Bind<IModule<BookcaseModule>>()
                .To<ModuleValidationDecorator>()
                .WhenInjectedInto<ModuleLoggingDecorator>();

            Bind<ILogger>().ToConstant(_logger).InSingletonScope();
        }
    }
}