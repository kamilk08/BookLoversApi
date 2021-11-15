using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using Ninject;

namespace BookLovers.Bookcases.Infrastructure.Root.Events
{
    internal class ProjectionDispatcher : IProjectionDispatcher
    {
        public void Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            var implementation = CompositionRoot.Kernel
                .Get(typeof(IProjectionHandler<>).MakeGenericType(@event.GetType()));

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