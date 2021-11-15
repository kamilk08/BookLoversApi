using BookLovers.Base.Infrastructure;
using Ninject.Modules;
using Serilog;

namespace BookLovers.Auth.Infrastructure.Root.Logging
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
            Bind<IModule<AuthModule>>()
                .To<ModuleValidationDecorator>()
                .WhenInjectedInto(typeof(ModuleLoggingDecorator));

            Bind<ILogger>()
                .ToConstant(_logger)
                .InSingletonScope();
        }
    }
}