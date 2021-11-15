using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Events.IntegrationEvents
{
    public interface IIntegrationEventDispatcher
    {
        Task DispatchAsync<TIntegrationEvent>(TIntegrationEvent @event)
            where TIntegrationEvent : IIntegrationEvent;
    }
}