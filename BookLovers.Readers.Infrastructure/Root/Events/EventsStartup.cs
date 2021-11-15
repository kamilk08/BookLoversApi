using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Bookcases.Integration.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Readers.Integration.IntegrationEvents;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root.Events
{
    internal static class EventsStartup
    {
        internal static void Initialize() => EventsStartup.SubscribeToEvents();

        private static void SubscribeToEvents()
        {
            var eventBus = CompositionRoot.Kernel.Get<IInMemoryEventBus>();

            eventBus.SubscribeToApplicationEvent<BookAddedToBookcaseIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<UserSignedUpIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<UserChangedEmailIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<BookRemovedFromReadShelfIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<SuperAdminCreatedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<BookCreatedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<NewAuthorAddedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<UserBlockedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<BookAcceptedByLibrarian>();
            eventBus.SubscribeToApplicationEvent<BookDismissedByLibrarian>();
            eventBus.SubscribeToApplicationEvent<BookArchivedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<AuthorAcceptedByLibrarian>();
            eventBus.SubscribeToApplicationEvent<AuthorDismissedByLibrarian>();
            eventBus.SubscribeToApplicationEvent<AuthorArchivedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<AuthorQuoteAddedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<BookQuoteAddedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<ShelfCreatedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<ShelfRemovedIntegrationEvent>();
            eventBus.SubscribeToApplicationEvent<BookRemovedFromBookcaseIntegrationEvent>();

            eventBus.NotifyProducer<ReviewAddedByReaderIntegrationEvent>();
            eventBus.NotifyProducer<ReviewArchivedIntegrationEvent>();
            eventBus.NotifyProducer<ReviewReportedByReaderIntegrationEvent>();
        }
    }
}