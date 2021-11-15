using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Publication.Integration.ApplicationEvents.Series;
using BookLovers.Readers.Integration.IntegrationEvents;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Events
{
    internal static class EventBusStartup
    {
        internal static void Initialize() => SubscribeToIntegrationEvents();

        private static void SubscribeToIntegrationEvents()
        {
            var eventBus = CompositionRoot.Kernel.Get<IInMemoryEventBus>();

            eventBus.SubscribeToIntegrationEvent<UserSignedUpIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<UserBlockedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<SuperAdminCreatedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<BookAcceptedByLibrarian>();
            eventBus.SubscribeToIntegrationEvent<AuthorAcceptedByLibrarian>();
            eventBus.SubscribeToIntegrationEvent<ReviewAddedByReaderIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<ReviewArchivedIntegrationEvent>();

            eventBus.NotifyProducer<AuthorArchivedIntegrationEvent>();
            eventBus.NotifyProducer<AuthorFollowedIntegrationEvent>();
            eventBus.NotifyProducer<AuthorHasNewBookIntegrationEvent>();
            eventBus.NotifyProducer<AuthorLostBookIntegrationEvent>();
            eventBus.NotifyProducer<AuthorQuoteAddedIntegrationEvent>();
            eventBus.NotifyProducer<AuthorUnFollowedIntegrationEvent>();
            eventBus.NotifyProducer<NewAuthorAddedIntegrationEvent>();
            eventBus.NotifyProducer<AuthorAddedToBookIntegrationEvent>();
            eventBus.NotifyProducer<AuthorRemovedFromBookIntegrationEvent>();
            eventBus.NotifyProducer<BookArchivedIntegrationEvent>();
            eventBus.NotifyProducer<BookCreatedIntegrationEvent>();
            eventBus.NotifyProducer<BookQuoteAddedIntegrationEvent>();
            eventBus.NotifyProducer<BookRemovedFromPublisherCycleIntegrationEvent>();
            eventBus.NotifyProducer<PublisherArchivedIntegrationEvent>();
            eventBus.NotifyProducer<PublisherCreatedIntegrationEvent>();
            eventBus.NotifyProducer<PublisherCycleArchivedIntegrationEvent>();
            eventBus.NotifyProducer<PublisherCycleCreatedIntegrationEvent>();
            eventBus.NotifyProducer<PublisherCycleHasNewBookIntegrationEvent>();
            eventBus.NotifyProducer<PublisherHasNewBookIntegrationEvent>();
            eventBus.NotifyProducer<PublisherLostBookIntegrationEvent>();
            eventBus.NotifyProducer<NewSeriesAddedIntegrationEvent>();
            eventBus.NotifyProducer<SeriesArchivedIntegrationEvent>();
            eventBus.NotifyProducer<SeriesHasNewBookIntegrationEvent>();
            eventBus.NotifyProducer<SeriesLostBookIntegrationEvent>();
        }
    }
}