using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Base.Infrastructure.Events.DomainEvents
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;

        Task PublishAsync<TEvent>(IEnumerable<TEvent> events)
            where TEvent : IEvent;
    }
}