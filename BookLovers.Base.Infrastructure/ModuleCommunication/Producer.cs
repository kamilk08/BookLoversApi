namespace BookLovers.Base.Infrastructure.ModuleCommunication
{
    public class Producer
    {
        public IProducerNotification Notification { get; }

        public string EventName { get; }

        public Producer(IProducerNotification notification, string eventName)
        {
            Notification = notification;
            EventName = eventName;
        }
    }
}