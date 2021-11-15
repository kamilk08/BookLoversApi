using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Root.Domain;
using BookLovers.Readers.Infrastructure.Root.Events;
using BookLovers.Readers.Infrastructure.Root.Inbox;
using BookLovers.Readers.Infrastructure.Root.Infrastructure;
using BookLovers.Readers.Infrastructure.Root.InternalProcessing;
using BookLovers.Readers.Infrastructure.Root.Jobs;
using BookLovers.Readers.Infrastructure.Root.Logging;
using BookLovers.Readers.Infrastructure.Root.Outbox;
using BookLovers.Readers.Infrastructure.Root.Persistence;
using BookLovers.Readers.Infrastructure.Root.Validation;
using BookLovers.Readers.Store.Persistence;
using FluentScheduler;
using Ninject;
using Serilog;

namespace BookLovers.Readers.Infrastructure.Root
{
    public class ReadersModuleStartup
    {
        private static IKernel _kernel;

        public static void Initialize(
            IHttpContextAccessor contextAccessor,
            IAppManager manager,
            ILogger logger,
            PersistenceSettings settings = null)
        {
            ConfigureCompositionRoot(contextAccessor, manager, logger);

            EventsStartup.Initialize();

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

            _kernel.Load<HandlersModule>();
            _kernel.Load<InfrastructureModule>();
            _kernel.Load<EventsModule>();
            _kernel.Load<InboxModule>();
            _kernel.Load<OutboxModule>();
            _kernel.Load<DomainModule>();
            _kernel.Load<ValidationModule>();

            var connectionString = manager.GetConfigValue(ReadersContext.ConnectionStringKey);
            var storeConnectionString = manager.GetConfigValue(ReadersStoreContext.ConnectionStringKey);

            var persistenceModule = settings == null
                ? new PersistenceModule(connectionString, storeConnectionString)
                : new PersistenceModule(settings, connectionString, storeConnectionString);

            _kernel.Load(persistenceModule);

            var loggingModule = new LoggingModule(logger.ForContext("Module", "READERS_MODULE"));
            _kernel.Load(loggingModule);

            _kernel.Load<InternalProcessingModule>();

            ConfigureExternalServices(contextAccessor);

            CompositionRoot.SetKernel(_kernel);
        }

        private static void ConfigureExternalServices(IHttpContextAccessor contextAccessor)
        {
            _kernel.Bind<IHttpContextAccessor>().ToConstant(contextAccessor);
        }

        private static void ConfigureFluentScheduler()
        {
            JobManager.Initialize(new ReadersJobsRegistry());
        }
    }
}