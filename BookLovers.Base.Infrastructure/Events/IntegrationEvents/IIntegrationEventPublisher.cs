namespace BookLovers.Base.Infrastructure.Events.IntegrationEvents
{
    public interface IIntegrationEventPublisher
    {
        void Publish<TIntegrationEvent>(TIntegrationEvent @event)
            where TIntegrationEvent : IIntegrationEvent;
    }
}