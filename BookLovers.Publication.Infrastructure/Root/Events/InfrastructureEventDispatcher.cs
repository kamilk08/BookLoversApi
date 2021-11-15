using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Events
{
    internal class InfrastructureEventDispatcher : IInfrastructureEventDispatcher
    {
        public async Task DispatchAsync<TInfrastructureEvent>(TInfrastructureEvent @event)
            where TInfrastructureEvent : IInfrastructureEvent
        {
            var type = typeof(IInfrastructureEventHandler<>)
                .MakeGenericType(@event.GetType());

            var implementation = CompositionRoot.Kernel.Get(type);

            if (implementation == null)
                throw new InvalidOperationException(
                    "Cannot dispatch infrastructure event. Event does not have associated handler.");

            await (Task) typeof(IInfrastructureEventHandler<>)
                .MakeGenericType(@event.GetType())
                .GetMethod("HandleAsync").Invoke(implementation, new object[] { @event });
        }
    }
}