using System.Threading.Tasks;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Base.Infrastructure.Events.DomainEvents
{
    public interface IDomainEventHandler
    {
    }

    public interface IDomainEventHandler<in TEvent> : IDomainEventHandler
        where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}