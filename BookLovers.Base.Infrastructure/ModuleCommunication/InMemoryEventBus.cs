using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Base.Infrastructure.ModuleCommunication
{
    public class InMemoryEventBus : IInMemoryEventBus
    {
        public void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler)
            where TEvent : IIntegrationEvent
        {
            EventBus.Instance.Subscribe(handler);
        }

        public void AddNotifier<TEvent>(IProducerNotification<TEvent> handler)
            where TEvent : IIntegrationEvent
        {
            EventBus.Instance.AddProducer(handler);
        }

        public async Task PublishWithMap<TEvent>(TEvent @event, Dictionary<int, bool> map)
            where TEvent : IIntegrationEvent
        {
            await EventBus.Instance.PublishAsync(@event, map);
        }

        public async Task Publish<TEvent>(TEvent integrationEvent)
            where TEvent : IIntegrationEvent
        {
            await EventBus.Instance.PublishAsync(integrationEvent);
        }

        public async Task PublishMultiple<TEvent>(IEnumerable<TEvent> events)
            where TEvent : IIntegrationEvent
        {
            foreach (var @event in events)
                await EventBus.Instance.PublishAsync(@event);
        }

        public IEnumerable<Subscription> GetSubscriptions()
        {
            return EventBus.Instance.Subscriptions.ToList();
        }

        public IEnumerable<Subscription> GetSubscriptions(string eventName)
        {
            return EventBus.Instance.Subscriptions.Where(p => p.EventName == eventName);
        }
    }
}