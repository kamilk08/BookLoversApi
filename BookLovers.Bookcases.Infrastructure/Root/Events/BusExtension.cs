using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using Serilog;

namespace BookLovers.Bookcases.Infrastructure.Root.Events
{
    internal static class BusExtension
    {
        internal static void SubscribeToIntegrationEvent<TIntegrationEvent>(
            this IInMemoryEventBus eventBus)
            where TIntegrationEvent : IIntegrationEvent
        {
            var handler = new GenericIntegrationEventHandler<TIntegrationEvent>();

            var handlerDecorator =
                new GenericIntegrationLoggingEventHandlerDecorator<TIntegrationEvent>(handler, Log.Logger);

            eventBus.Subscribe(handlerDecorator);
        }

        internal static void NotifyProducer<TIntegrationEvent>(this IInMemoryEventBus eventBus)
            where TIntegrationEvent : IIntegrationEvent
        {
            var handler = new GenericProducerNotifier<TIntegrationEvent>();

            var producerLoggingNotifier = new GenericProducerLoggingNotifier<TIntegrationEvent>(handler, Log.Logger);

            eventBus.AddNotifier(producerLoggingNotifier);
        }
    }
}