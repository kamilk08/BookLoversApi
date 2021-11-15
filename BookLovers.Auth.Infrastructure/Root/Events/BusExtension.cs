using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using Serilog;

namespace BookLovers.Auth.Infrastructure.Root.Events
{
    internal static class BusExtension
    {
        internal static void SubscribeToIntegrationEvent<TIntegrationEvent>(
            this IInMemoryEventBus eventBus)
            where TIntegrationEvent : IIntegrationEvent
        {
            var handlerDecorator =
                new GenericIntegrationLoggingEventHandlerDecorator<TIntegrationEvent>(
                    new GenericIntegrationEventHandler<TIntegrationEvent>(),
                    Log.Logger);

            eventBus.Subscribe(handlerDecorator);
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