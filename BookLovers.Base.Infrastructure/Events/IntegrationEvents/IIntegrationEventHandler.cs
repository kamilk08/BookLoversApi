using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Events.IntegrationEvents
{
    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<TEvent> : IIntegrationEventHandler
        where TEvent : IIntegrationEvent
    {
        Task HandleAsync(TEvent @event);
    }
}