using BookLovers.Base.Infrastructure;
using Ninject.Modules;
using Serilog;

namespace BookLovers.Librarians.Infrastructure.Root.Logging
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
            Bind<ILogger>().ToConstant(_logger);

            Bind<IModule<LibrarianModule>>().To<ModuleValidationDecorator>()
                .WhenInjectedInto(typeof(ModuleLoggingDecorator));
        }
    }
}