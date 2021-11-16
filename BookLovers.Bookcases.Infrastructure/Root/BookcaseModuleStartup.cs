using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Root.Domain;
using BookLovers.Bookcases.Infrastructure.Root.Events;
using BookLovers.Bookcases.Infrastructure.Root.Inbox;
using BookLovers.Bookcases.Infrastructure.Root.Infrastructure;
using BookLovers.Bookcases.Infrastructure.Root.InternalProcessing;
using BookLovers.Bookcases.Infrastructure.Root.Jobs;
using BookLovers.Bookcases.Infrastructure.Root.Logging;
using BookLovers.Bookcases.Infrastructure.Root.Outbox;
using BookLovers.Bookcases.Infrastructure.Root.Persistence;
using BookLovers.Bookcases.Infrastructure.Root.Validation;
using BookLovers.Bookcases.Store.Persistence;
using FluentScheduler;
using Ninject;
using Serilog;

namespace BookLovers.Bookcases.Infrastructure.Root
{
    public class BookcaseModuleStartup
    {
        private static IKernel _kernel;

        public static void Initialize(
            IHttpContextAccessor contextAccessor,
            IAppManager manager,
            ILogger logger,
            PersistenceSettings settings = null)
        {
            ConfigureCompositionRoot(contextAccessor, manager, logger, settings);

            EventStartup.Initialize();

            PersistenceStartup.Initialize(settings ?? PersistenceSettings.Default());

            InitializeFluentScheduler();
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
            _kernel.Load<EventsModule>();
            _kernel.Load<InboxModule>();
            _kernel.Load<OutboxModule>();
            _kernel.Load<ServicesModule>();
            _kernel.Load<InternalProcessingModule>();
            _kernel.Load<ValidationModule>();

            var connectionString = manager.GetConfigValue(BookcaseContext.ConnectionStringKey);
            var storeConnectionString = manager.GetConfigValue(BookcaseStoreContext.ConnectionStringKey);

            var persistenceModule = settings == null
                ? new PersistenceModule(connectionString, storeConnectionString)
                : new PersistenceModule(settings, connectionString, storeConnectionString);

            logger = logger.ForContext("Module", "BOOKCASE_MODULE");

            _kernel.Load(persistenceModule);
            _kernel.Load(new LoggingModule(logger));

            RegisterExternalServices(contextAccessor);

            CompositionRoot.SetKernel(_kernel);
        }

        private static void RegisterExternalServices(IHttpContextAccessor contextAccessor)
        {
            _kernel.Bind<IHttpContextAccessor>()
                .ToConstant(contextAccessor);
        }

        private static void InitializeFluentScheduler()
        {
            JobManager.Initialize((Registry) new BookcaseJobRegistry());
        }
    }
}