using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Base.Infrastructure.ModuleCommunication
{
    public class Subscription
    {
        public IIntegrationEventHandler HandlerType { get; }

        public string EventName { get; }

        public Subscription(IIntegrationEventHandler handlerType, string eventName)
        {
            HandlerType = handlerType;
            EventName = eventName;
        }
    }
}