using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Root.Domain;
using BookLovers.Auth.Infrastructure.Root.Events;
using BookLovers.Auth.Infrastructure.Root.Inbox;
using BookLovers.Auth.Infrastructure.Root.Infrastructure;
using BookLovers.Auth.Infrastructure.Root.InternalProcessing;
using BookLovers.Auth.Infrastructure.Root.Jobs;
using BookLovers.Auth.Infrastructure.Root.Logging;
using BookLovers.Auth.Infrastructure.Root.Outbox;
using BookLovers.Auth.Infrastructure.Root.Persistence;
using BookLovers.Auth.Infrastructure.Root.Validation;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using FluentScheduler;
using Ninject;
using Serilog;

namespace BookLovers.Auth.Infrastructure.Root
{
    public class AuthModuleStartup
    {
        private static IKernel _kernel;

        public static void Initialize(
            IHttpContextAccessor contextAccessor,
            IAppManager appManager,
            ILogger logger,
            PersistenceSettings settings)
        {
            ConfigureCompositionRoot(
                contextAccessor,
                appManager,
                logger,
                appManager.GetConfigValue(AuthContext.ConnectionStringKey));

            EventsStartup.Initialize();

            PersistenceStartup.Initialize(settings);

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
            IAppManager appManager,
            ILogger logger,
            string connectionString)
        {
            _kernel = new StandardKernel();

            _kernel.Load<DomainModule>();
            _kernel.Load<EventsModule>();
            _kernel.Load<HandlersModule>();
            _kernel.Load<OutboxModule>();
            _kernel.Load<InboxModule>();
            _kernel.Load<ServicesModule>();
            _kernel.Load<InternalProcessingModule>();
            _kernel.Load<CacheModule>();
            _kernel.Load<ValidationModule>();

            var loggingModule = new LoggingModule(logger.ForContext("Module", "AUTH_MODULE"));
            _kernel.Load(loggingModule);

            var persistenceModule = new PersistenceModule(connectionString);
            _kernel.Load(persistenceModule);

            RegisterExternalServices(contextAccessor, appManager);

            CompositionRoot.SetKernel(_kernel);
        }

        private static void RegisterExternalServices(
            IHttpContextAccessor contextAccessor,
            IAppManager appManager)
        {
            _kernel.Bind<IHttpContextAccessor>().ToConstant(contextAccessor);

            _kernel.Bind<IAppManager>().ToConstant(appManager);
        }

        private static void InitializeFluentScheduler()
        {
            JobManager.Initialize((Registry)new AuthJobRegistry());
        }
    }
}