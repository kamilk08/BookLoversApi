using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Events.InfrastructureEvents
{
    public interface IInfrastructureEventHandler<TInfrastructureEvent>
        where TInfrastructureEvent : IInfrastructureEvent
    {
        Task HandleAsync(TInfrastructureEvent @event);
    }
}