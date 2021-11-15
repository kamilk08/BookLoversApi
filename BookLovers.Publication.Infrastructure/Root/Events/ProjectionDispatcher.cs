using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Events
{
    internal class ProjectionDispatcher : IProjectionDispatcher
    {
        public void Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            var type = typeof(IProjectionHandler<>).MakeGenericType(@event.GetType());

            var implementation = CompositionRoot.Kernel.Get(type);

            if (implementation == null)
                throw new InvalidOperationException(
                    "There is no projection of aggregate event. Cannot make projection of data.");

            typeof(IProjectionHandler<>)
                .MakeGenericType(@event.GetType())
                .GetMethod("Handle")
                .Invoke(implementation, new object[] { @event });
        }
    }
}