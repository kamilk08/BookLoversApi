using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using Serilog;

namespace BookLovers.Publication.Infrastructure.Root.Events
{
    internal static class BusExtensions
    {
        internal static void SubscribeToIntegrationEvent<TIntegrationEvent>(
            this IInMemoryEventBus eventBus)
            where TIntegrationEvent : IIntegrationEvent
        {
            var loggingEventHandler =
                new GenericIntegrationEventLoggingEventHandler<TIntegrationEvent>(
                    new GenericIntegrationEventHandler<TIntegrationEvent>(), Log.Logger);

            eventBus.Subscribe(loggingEventHandler);
        }

        internal static void NotifyProducer<TIntegrationEvent>(this IInMemoryEventBus eventBus)
            where TIntegrationEvent : IIntegrationEvent
        {
            var producerLoggingNotifier =
                new GenericProducerLoggingNotifier<TIntegrationEvent>(
                    new GenericProducerNotifier<TIntegrationEvent>(),
                    Log.Logger);

            eventBus.AddNotifier(producerLoggingNotifier);
        }
    }
}