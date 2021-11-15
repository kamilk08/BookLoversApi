using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Base.Infrastructure.ModuleCommunication
{
    internal class EventBus
    {
        public static readonly EventBus Instance = new EventBus();
        internal readonly IList<Subscription> Subscriptions = new List<Subscription>();
        internal readonly IList<Producer> Producers = new List<Producer>();

        public void AddProducer<TEvent>(IProducerNotification<TEvent> handler)
            where TEvent : IIntegrationEvent
        {
            Producers.Add(new Producer(handler, typeof(TEvent).FullName));
        }

        public void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler)
            where TEvent : IIntegrationEvent
        {
            Subscriptions.Add(new Subscription(handler, typeof(TEvent).FullName));
        }

        public async Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event)
            where TIntegrationEvent : IIntegrationEvent
        {
            var eventType = @event.GetType();

            var producerNotification = GetProducerNotification<TIntegrationEvent>(eventType.FullName);
            var subscriptions = Subscriptions.Where(p => p.EventName == eventType.FullName).ToList();

            var map = new Dictionary<int, bool>();

            for (var j = 0; j < subscriptions.Count(); ++j)
                map.Add(j, false);

            var i = 0;
            try
            {
                foreach (var subscription in subscriptions)
                {
                    if (subscription.HandlerType is IIntegrationEventHandler<TIntegrationEvent> handler)
                        await handler.HandleAsync(@event);

                    map[i] = true;
                    ++i;
                }
            }
            catch (Exception ex)
            {
                producerNotification.NotifyProducer(@event, map);

                return;
            }

            producerNotification.NotifyProducer(@event, map);
        }

        public async Task PublishAsync<TIntegrationEvent>(
            TIntegrationEvent @event,
            Dictionary<int, bool> map)
            where TIntegrationEvent : IIntegrationEvent
        {
            var eventType = @event.GetType();
            var subscriptions = Subscriptions.Where(p => p.EventName == eventType.FullName).ToList();

            for (var i = 0; i < map.Count; ++i)
            {
                try
                {
                    if (!map.ElementAt(i).Value)
                    {
                        var subscription = subscriptions.ElementAt(i);

                        await (Task) typeof(IIntegrationEventHandler<>)
                            .MakeGenericType(@event.GetType())
                            .GetMethod("HandleAsync")
                            .Invoke(subscription.HandlerType, new object[] { @event });

                        map[i] = true;
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private IProducerNotification<TIntegrationEvent> GetProducerNotification<TIntegrationEvent>(
            string eventName)
            where TIntegrationEvent : IIntegrationEvent
        {
            return Producers
                    .FirstOrDefault(p => p.EventName == eventName)?
                    .Notification as
                IProducerNotification<TIntegrationEvent>;
        }
    }
}