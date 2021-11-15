using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Base.Infrastructure.ModuleCommunication
{
    public interface IProducerNotification
    {
    }

    public interface IProducerNotification<TIntegrationEvent> : IProducerNotification
        where TIntegrationEvent : IIntegrationEvent
    {
        void NotifyProducer(TIntegrationEvent @event, Dictionary<int, bool> map);
    }
}