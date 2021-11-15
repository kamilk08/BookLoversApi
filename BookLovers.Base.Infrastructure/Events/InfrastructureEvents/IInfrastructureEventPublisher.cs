namespace BookLovers.Base.Infrastructure.Events.InfrastructureEvents
{
    public interface IInfrastructureEventPublisher
    {
        void Publish<TInfrastructureEvent>(TInfrastructureEvent @event)
            where TInfrastructureEvent : IInfrastructureEvent;
    }
}