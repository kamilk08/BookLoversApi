using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Readers.Integration.IntegrationEvents;
using Ninject;

namespace BookLovers.Librarians.Infrastructure.Root.Events
{
    internal static class EventsStartup
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
            eventBus.SubscribeToIntegrationEvent<ReviewReportedByReaderIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<ReviewAddedByReaderIntegrationEvent>();
            eventBus.SubscribeToIntegrationEvent<ReviewArchivedIntegrationEvent>();

            eventBus.NotifyProducer<AuthorAcceptedByLibrarian>();
            eventBus.NotifyProducer<AuthorDismissedByLibrarian>();
            eventBus.NotifyProducer<BookAcceptedByLibrarian>();
            eventBus.NotifyProducer<BookDismissedByLibrarian>();
            eventBus.NotifyProducer<LibrarianDegradedToReader>();
            eventBus.NotifyProducer<ReaderPromotedToLibrarian>();
        }
    }
}