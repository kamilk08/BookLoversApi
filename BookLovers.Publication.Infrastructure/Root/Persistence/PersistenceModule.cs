using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Infrastructure.Mementos;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Store.Persistence;
using Ninject.Modules;

namespace BookLovers.Publication.Infrastructure.Root.Persistence
{
    internal class PersistenceModule : NinjectModule
    {
        private readonly PersistenceSettings _settings;
        private readonly string _connectionString;
        private readonly string _storeConnectionString;

        public PersistenceModule(string connectionString, string storeConnectionString)
        {
            _settings =
                new PersistenceSettings(SnapshotSettings.Default(), PersistenceInitialSettings.DropDatabases());
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
            Bind<PublicationsContext>().ToSelf()
                .WithConstructorArgument("connectionString", _connectionString);

            Bind<IUnitOfWork>().To<UnitOfWork>();

            Bind<IEventStore>().To<EventStore>();

            Bind<IEventStream>().To<EventStream>();

            Bind<ISnapshotProvider>().To<SnapshotProvider>();

            Bind<ISnapshotMaker>().To<SnapshotMaker>()
                .WithConstructorArgument("settings", _settings.SnapshotSettings);

            Bind<PublicationsStoreContext>().ToSelf()
                .WithConstructorArgument("connectionString", _storeConnectionString);

            Bind<IMementoFactory>().To<MementoFactory>();

            Bind<IMemento<Author>>().To<AuthorMemento>();

            Bind<IMemento<Book>>().To<BookMemento>();

            Bind<IMemento<PublisherCycle>>().To<PublisherCycleMemento>();

            Bind<IMemento<Publisher>>().To<PublisherMemento>();

            Bind<IMemento<Quote>>().To<QuoteMemento>();

            Bind<IMemento<Series>>().To<SeriesMemento>();
        }
    }
}