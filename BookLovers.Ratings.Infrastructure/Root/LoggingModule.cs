using BookLovers.Base.Infrastructure;
using Ninject.Modules;
using Serilog;

namespace BookLovers.Ratings.Infrastructure.Root
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
            Bind<IModule<RatingsModule>>()
                .To<ModuleValidationDecorator>()
                .WhenInjectedInto(typeof(ModuleLoggingDecorator));

            Bind<ILogger>().ToConstant(_logger).InSingletonScope();
        }
    }
}