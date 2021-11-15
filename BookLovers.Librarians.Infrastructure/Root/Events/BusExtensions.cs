using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using Serilog;

namespace BookLovers.Librarians.Infrastructure.Root.Events
{
    internal static class BusExtensions
    {
        internal static void SubscribeToIntegrationEvent<T>(this IInMemoryEventBus eventBus) where T : IIntegrationEvent
        {
            var loggingEventHandler =
                new GenericIntegrationLoggingEventHandler<T>(new GenericIntegrationEventHandler<T>(), Log.Logger);
            
            eventBus.Subscribe<T>(loggingEventHandler);
        }

        internal static void NotifyProducer<TIntegrationEvent>(this IInMemoryEventBus eventBus)
            where TIntegrationEvent : IIntegrationEvent
        {
            var producerLoggingNotifier =
                new GenericProducerLoggingNotifier<TIntegrationEvent>(new GenericProducerNotifier<TIntegrationEvent>(),
                    Log.Logger);
            
            eventBus.AddNotifier<TIntegrationEvent>(producerLoggingNotifier);
        }
    }
}