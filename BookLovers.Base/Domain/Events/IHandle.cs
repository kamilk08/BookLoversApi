namespace BookLovers.Base.Domain.Events
{
    public interface IHandle<in TEvent>
        where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}