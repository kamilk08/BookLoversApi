using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Base.Infrastructure.ModuleCommunication
{
    public interface IInMemoryEventBus
    {
        void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler)
            where TEvent : IIntegrationEvent;

        void AddNotifier<TEvent>(IProducerNotification<TEvent> handler)
            where TEvent : IIntegrationEvent;

        Task Publish<TEvent>(TEvent @event)
            where TEvent : IIntegrationEvent;

        Task PublishWithMap<TEvent>(TEvent @event, Dictionary<int, bool> map)
            where TEvent : IIntegrationEvent;

        Task PublishMultiple<TEvent>(IEnumerable<TEvent> events)
            where TEvent : IIntegrationEvent;

        IEnumerable<Subscription> GetSubscriptions();

        IEnumerable<Subscription> GetSubscriptions(string eventName);
    }
}