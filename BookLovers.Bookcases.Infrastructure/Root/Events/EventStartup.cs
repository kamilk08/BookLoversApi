using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Bookcases.Integration.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using Ninject;

namespace BookLovers.Bookcases.Infrastructure.Root.Events
{
    internal static class EventStartup
    {
        internal static void Initialize()
        {
            SubscribeToIntegrationEvents();
        }

        private static void SubscribeToIntegrationEvents()
        {
            var eventBus = CompositionRoot.Kernel.Get<IInMemoryEventBus>();

            eventBus.SubscribeToIntegrationEvent<UserBlockedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<BookCreatedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<UserSignedUpIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<SuperAdminCreatedIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<BookArchivedIntegrationEvent>();

            eventBus.NotifyProducer<BookAddedToBookcaseIntegrationEvent>();
            eventBus.NotifyProducer<BookRemovedFromBookcaseIntegrationEvent>();
            eventBus.NotifyProducer<BookRemovedFromReadShelfIntegrationEvent>();
            eventBus.NotifyProducer<ReaderRemovedShelfFromBookcaseIntegrationEvent>();
            eventBus.NotifyProducer<ShelfCreatedIntegrationEvent>();
            eventBus.NotifyProducer<ShelfRemovedIntegrationEvent>();
        }
    }
}