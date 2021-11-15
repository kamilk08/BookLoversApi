using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Infrastructure.Mementos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Store.Persistence;
using Ninject.Modules;

namespace BookLovers.Bookcases.Infrastructure.Root.Persistence
{
    public class PersistenceModule : NinjectModule
    {
        private readonly PersistenceSettings _settings;
        private readonly string _connectionString;
        private readonly string _storeConnectionString;

        public PersistenceModule(string connectionString, string storeConnectionString)
        {
            _settings = new PersistenceSettings(
                SnapshotSettings.Default(),
                PersistenceInitialSettings.SeedWithProcedures());

            _connectionString = connectionString;
            _storeConnectionString = storeConnectionString;
        }

        public PersistenceModule(
            PersistenceSettings settings,
            string connectionString,
            string storeConnectionString)
        {
            _settings = settings;
            _connectionString = connectionString;
            _storeConnectionString = storeConnectionString;
        }

        public override void Load()
        {
            Bind<BookcaseContext>().ToSelf()
                .WithConstructorArgument("connectionString", _connectionString);

            Bind<IUnitOfWork>().To<UnitOfWork>();

            Bind<IReadContextAccessor>().To<ReadContextAccessor>();

            Bind<IEventStore>().To<EventStore>();

            Bind<ISnapshotMaker>().To<SnapshotMaker>()
                .WithConstructorArgument("settings", _settings.SnapshotSettings);

            Bind<ISnapshotProvider>().To<SnapshotProvider>();

            Bind<IEventStream>().To<EventStream>();

            Bind<BookcaseStoreContext>().ToSelf()
                .WithConstructorArgument("connectionString", _storeConnectionString);

            Bind<IMementoFactory>().To<MementoFactory>();

            Bind<IMemento<Bookcase>>().To<BookcaseMemento>();

            Bind<IMemento<SettingsManager>>().To<SettingsManagerMemento>();
        }
    }
}