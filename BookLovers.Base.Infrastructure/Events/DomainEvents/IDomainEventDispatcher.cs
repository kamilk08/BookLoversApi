using System.Threading.Tasks;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Base.Infrastructure.Events.DomainEvents
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}