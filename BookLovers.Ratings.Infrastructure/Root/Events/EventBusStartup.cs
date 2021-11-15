using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Bookcases.Integration.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;
using BookLovers.Publication.Integration.ApplicationEvents.Series;
using Ninject;

namespace BookLovers.Ratings.Infrastructure.Root.Events
{
    internal static class EventBusStartup
    {
        internal static void Initialize()
        {
            SubscribeToIntegrationEvents();
        }

        private static void SubscribeToIntegrationEvents()
        {
            var eventBus = CompositionRoot.Kernel.Get<IInMemoryEventBus>();

            eventBus.SubscribeToIntegrationEvent<UserSignedUpIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<UserBlockedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<SuperAdminCreatedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<BookCreatedIntegrationEvent>();

            eventBus.SubscribeToIntegrationEvent<NewAuthorAddedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<AuthorHasNewBookIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<NewSeriesAddedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<SeriesHasNewBookIntegrationEvent>();

            eventBus.SubscribeToIntegrationEvent<PublisherCreatedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<PublisherHasNewBookIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<BookRemovedFromReadShelfIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<AuthorAddedToBookIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<AuthorRemovedFromBookIntegrationEvent>();

            eventBus.SubscribeToIntegrationEvent<AuthorLostBookIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<AuthorArchivedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<BookArchivedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<PublisherLostBookIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<PublisherArchivedIntegrationEvent>();

            eventBus.SubscribeToIntegrationEvent<SeriesArchivedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<SeriesLostBookIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<PublisherCycleCreatedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<PublisherCycleHasNewBookIntegrationEvent>();

            eventBus.SubscribeToIntegrationEvent<PublisherCycleArchivedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<BookRemovedFromPublisherCycleIntegrationEvent>();
        }
    }
}