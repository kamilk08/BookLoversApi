using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Readers.Domain.NotificationWalls;
using BookLovers.Readers.Domain.ProfileManagers;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Infrastructure.Mementos;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Store.Persistence;
using Ninject.Modules;

namespace BookLovers.Readers.Infrastructure.Root.Persistence
{
    internal class PersistenceModule : NinjectModule
    {
        private readonly PersistenceSettings _settings;
        private readonly string _connectionString;
        private readonly string _storeConnectionString;

        public PersistenceModule(string connectionString, string storeConnectionString)
        {
            this._settings =
                new PersistenceSettings(SnapshotSettings.Default(), PersistenceInitialSettings.DropDatabases());
            this._connectionString = connectionString;
            this._storeConnectionString = storeConnectionString;
        }

        public PersistenceModule(
            PersistenceSettings settings,
            string connectionString,
            string storeConnectionString)
        {
            this._settings = settings;
            this._connectionString = connectionString;
            this._storeConnectionString = storeConnectionString;
        }

        public override void Load()
        {
            this.Bind<ReadersContext>().ToSelf()
                .WithConstructorArgument("connectionString", this._connectionString);

            this.Bind<IUnitOfWork>().To<UnitOfWork>();

            this.Bind<IEventStore>().To<EventStore>();

            this.Bind<IEventStream>().To<EventStream>();

            this.Bind<ISnapshotProvider>().To<SnapshotProvider>();

            this.Bind<ISnapshotMaker>().To<SnapshotMaker>()
                .WithConstructorArgument("settings", this._settings.SnapshotSettings);

            this.Bind<IMementoFactory>().To<MementoFactory>();

            this.Bind<IMemento<NotificationWall>>().To<NotificationWallMemento>();

            this.Bind<IMemento<Profile>>().To<SocialProfileMemento>();

            this.Bind<IMemento<ProfilePrivacyManager>>().To<PrivacyManagerMemento>();

            this.Bind<IMemento<Reader>>().To<ReaderMemento>();

            this.Bind<IMemento<Review>>().To<ReviewMemento>();

            this.Bind<IMemento<StatisticsGatherer>>().To<StatisticsGathererMemento>();

            this.Bind<ReadersStoreContext>().ToSelf()
                .WithConstructorArgument("connectionString", this._storeConnectionString);
        }
    }
}