using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Root.Domain;
using BookLovers.Librarians.Infrastructure.Root.Events;
using BookLovers.Librarians.Infrastructure.Root.Inbox;
using BookLovers.Librarians.Infrastructure.Root.Infrastructure;
using BookLovers.Librarians.Infrastructure.Root.InternalProcessing;
using BookLovers.Librarians.Infrastructure.Root.Jobs;
using BookLovers.Librarians.Infrastructure.Root.Logging;
using BookLovers.Librarians.Infrastructure.Root.Outbox;
using BookLovers.Librarians.Infrastructure.Root.Persistence;
using BookLovers.Librarians.Infrastructure.Root.Validation;
using FluentScheduler;
using Ninject;
using Serilog;

namespace BookLovers.Librarians.Infrastructure.Root
{
    public static class LibrarianModuleStartup
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

            PersistenceModuleStartup.Initialize(settings ?? PersistenceSettings.Default());

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
            ILogger logger)
        {
            _kernel = new StandardKernel();

            _kernel.Load<EventModule>();
            _kernel.Load<InfrastructureModule>();
            _kernel.Load<InboxModule>();
            _kernel.Load<OutboxModule>();
            _kernel.Load<HandlersModule>();
            _kernel.Load<DomainModule>();
            _kernel.Load<InternalProcessingModule>();
            _kernel.Load<ValidationModule>();

            var persistenceModule = new PersistenceModule(manager.GetConfigValue(LibrariansContext.ConnectionStringKey));
            
            _kernel.Load(persistenceModule);

            var loggingModule = new LoggingModule(logger.ForContext("Module", "LIBRARIANS_MODULE"));
            _kernel.Load(loggingModule);

            logger.Information("Testing logging in librarians module...");
            
            ConfigureExternalServices(contextAccessor);

            CompositionRoot.SetKernel(_kernel);
        }

        private static void ConfigureExternalServices(IHttpContextAccessor contextAccessor)
        {
            _kernel.Bind<IHttpContextAccessor>().ToConstant(contextAccessor);
        }

        private static void InitializeFluentScheduler()
        {
            JobManager.Initialize((Registry) new LibrariansJobsRegistry());
        }
    }
}