using BookLovers.Base.Infrastructure;
using Ninject.Modules;
using Serilog;

namespace BookLovers.Publication.Infrastructure.Root.Logging
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
            Bind<IModule<PublicationModule>>().To<ModuleValidationDecorator>()
                .WhenInjectedInto<ModuleLoggingDecorator>();

            Bind<ILogger>().ToConstant(_logger);
        }
    }
}