using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Events.InfrastructureEvents
{
    public interface IInfrastructureEventDispatcher
    {
        Task DispatchAsync<TInfrastructureEvent>(TInfrastructureEvent @event)
            where TInfrastructureEvent : IInfrastructureEvent;
    }
}