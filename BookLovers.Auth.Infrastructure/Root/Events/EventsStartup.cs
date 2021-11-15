using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Librarians.Integration.IntegrationEvents;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root.Events
{
    internal static class EventsStartup
    {
        internal static void Initialize()
        {
            var eventBus = CompositionRoot.Kernel.Get<IInMemoryEventBus>();

            eventBus.SubscribeToIntegrationEvent<ReaderPromotedToLibrarian>();
            eventBus.SubscribeToIntegrationEvent<LibrarianDegradedToReader>();

            eventBus.NotifyProducer<SuperAdminCreatedIntegrationEvent>();
            eventBus.NotifyProducer<UserBlockedIntegrationEvent>();
            eventBus.NotifyProducer<UserChangedEmailIntegrationEvent>();
            eventBus.NotifyProducer<UserSignedUpIntegrationEvent>();
        }
    }
}