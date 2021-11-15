using BookLovers.Base.Infrastructure;
using Ninject.Modules;
using Serilog;

namespace BookLovers.Readers.Infrastructure.Root.Logging
{
    internal class LoggingModule : NinjectModule
    {
        private readonly ILogger _logger;

        public LoggingModule(ILogger logger)
        {
            this._logger = logger;
        }

        public override void Load()
        {
            this.Bind<IModule<ReadersModule>>()
                .To<ModuleValidationDecorator>()
                .WhenInjectedInto<ModuleLoggingDecorator>();

            this.Bind<ILogger>().ToConstant(this._logger).InSingletonScope();
        }
    }
}