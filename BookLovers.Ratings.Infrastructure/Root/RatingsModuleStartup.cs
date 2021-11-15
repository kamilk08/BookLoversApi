using System;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Root.Domain;
using BookLovers.Ratings.Infrastructure.Root.Events;
using BookLovers.Ratings.Infrastructure.Root.Inbox;
using BookLovers.Ratings.Infrastructure.Root.Infrastructure;
using BookLovers.Ratings.Infrastructure.Root.InternalProcessing;
using BookLovers.Ratings.Infrastructure.Root.Jobs;
using BookLovers.Ratings.Infrastructure.Root.Outbox;
using BookLovers.Ratings.Infrastructure.Root.Persistence;
using BookLovers.Ratings.Infrastructure.Root.Validation;
using FluentScheduler;
using Ninject;
using Ninject.Modules;
using Serilog;

namespace BookLovers.Ratings.Infrastructure.Root
{
    public static class RatingsModuleStartup
    {
        private static IKernel _kernel;

        public static void Initialize(
            IHttpContextAccessor httpAccessor,
            IAppManager manager,
            ILogger logger,
            PersistenceSettings settings)
        {
            ConfigureCompositionRoot(httpAccessor, manager, logger);
            EventBusStartup.Initialize();
            PersistenceStartup.Initialize(settings ?? PersistenceSettings.Default());
            ConfigureFluentScheduler();
        }

        public static void AddOrUpdateService<TService>(TService service, bool rebind)
        {
            if (rebind)
                _kernel.Rebind<TService>().ToConstant(service);
            else
                _kernel.Bind<TService>().ToConstant(service);
        }

        private static void ConfigureCompositionRoot(
            IHttpContextAccessor httpAccessor,
            IAppManager manager,
            ILogger logger)
        {
            _kernel = new StandardKernel(Array.Empty<INinjectModule>());

            var loggingModule = new LoggingModule(logger.ForContext("Module", "RATINGS_MODULE"));

            _kernel.Load<DomainModule>();
            _kernel.Load<EventModule>();
            _kernel.Load<HandlersModule>();
            _kernel.Load<InboxModule>();
            _kernel.Load<OutboxModule>();
            _kernel.Load<InfrastructureModule>();

            var connectionString = manager.GetConfigValue(RatingsContext.ConnectionStringKey);

            var persistenceModule = new PersistenceModule(connectionString);

            _kernel.Load(persistenceModule);
            _kernel.Load<InternalProcessingModule>();
            _kernel.Load<ValidationModule>();
            _kernel.Load(loggingModule);

            ConfigureExternalServices(httpAccessor);

            CompositionRoot.SetKernel(_kernel);
        }

        private static void ConfigureExternalServices(IHttpContextAccessor httpAccessor)
        {
            _kernel.Bind<IHttpContextAccessor>().ToConstant(httpAccessor);
        }

        private static void ConfigureFluentScheduler()
        {
            JobManager.Initialize((Registry) new RatingsJobsRegistry());
        }
    }
}