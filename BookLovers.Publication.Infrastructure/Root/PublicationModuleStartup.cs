using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Root.Domain;
using BookLovers.Publication.Infrastructure.Root.Events;
using BookLovers.Publication.Infrastructure.Root.Inbox;
using BookLovers.Publication.Infrastructure.Root.Infrastructure;
using BookLovers.Publication.Infrastructure.Root.InternalProcessing;
using BookLovers.Publication.Infrastructure.Root.Jobs;
using BookLovers.Publication.Infrastructure.Root.Logging;
using BookLovers.Publication.Infrastructure.Root.Outbox;
using BookLovers.Publication.Infrastructure.Root.Persistence;
using BookLovers.Publication.Infrastructure.Root.Validation;
using BookLovers.Publication.Store.Persistence;
using FluentScheduler;
using Ninject;
using Serilog;

namespace BookLovers.Publication.Infrastructure.Root
{
    public static class PublicationModuleStartup
    {
        private static IKernel _kernel;

        public static void Initialize(
            IHttpContextAccessor contextAccessor,
            IAppManager manager,
            ILogger logger,
            PersistenceSettings settings = null)
        {
            ConfigureCompositionRoot(contextAccessor, manager, logger, settings);

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
            IHttpContextAccessor contextAccessor,
            IAppManager manager,
            ILogger logger,
            PersistenceSettings settings = null)
        {
            _kernel = new StandardKernel();

            _kernel.Load<DomainModule>();
            _kernel.Load<HandlersModule>();
            _kernel.Load<InfrastructureModule>();
            _kernel.Load<EventBusModule>();
            _kernel.Load<InboxModule>();
            _kernel.Load<OutboxModule>();
            _kernel.Load<CacheModule>();
            _kernel.Load<ValidationModule>();

            var connectionString = manager.GetConfigValue(PublicationsContext.ConnectionStringKey);
            var storeConnectionString = manager.GetConfigValue(PublicationsStoreContext.ConnectionStringKey);

            var persistenceModule = settings == null
                ? new PersistenceModule(connectionString, storeConnectionString)
                : new PersistenceModule(settings, connectionString, storeConnectionString);

            _kernel.Load(persistenceModule);

            var loggingModule = new LoggingModule(logger.ForContext("Module", "BOOKS_MODULE"));

            logger.Information("Testing logging in publications module");

            _kernel.Load(loggingModule);

            _kernel.Load<InternalProcessingModule>();

            ConfigureExternalServices(contextAccessor);

            CompositionRoot.SetKernel(_kernel);
        }

        private static void ConfigureExternalServices(IHttpContextAccessor contextAccessor)
        {
            _kernel.Bind<IHttpContextAccessor>()
                .ToConstant(contextAccessor);
        }

        private static void ConfigureFluentScheduler()
        {
            JobManager.Initialize(new PublicationJobsRegistry());
        }
    }
}