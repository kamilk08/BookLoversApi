using BookLovers.Base.Domain.Events;

namespace BookLovers.Base.Infrastructure.Events.DomainEvents
{
    public interface IProjectionHandler
    {
    }

    public interface IProjectionHandler<in TEvent> : IProjectionHandler
        where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}