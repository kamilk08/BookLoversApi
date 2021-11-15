using BookLovers.Base.Domain.Events;

namespace BookLovers.Base.Infrastructure.Events.DomainEvents
{
    public interface IProjectionDispatcher
    {
        void Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}