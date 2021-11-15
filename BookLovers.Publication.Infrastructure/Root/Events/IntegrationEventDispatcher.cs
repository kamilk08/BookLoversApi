using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Events
{
    internal class IntegrationEventDispatcher : IIntegrationEventDispatcher
    {
        public async Task DispatchAsync<TIntegrationEvent>(TIntegrationEvent @event)
            where TIntegrationEvent : IIntegrationEvent
        {
            var type = typeof(IIntegrationEventHandler<>).MakeGenericType(@event.GetType());

            var implementation = CompositionRoot.Kernel
                .Get(type);

            if (implementation == null)
                throw new InvalidOperationException("There is no handler linked with given event");

            await (Task) typeof(IIntegrationEventHandler<>)
                .MakeGenericType(@event.GetType())
                .GetMethod("HandleAsync")
                .Invoke(implementation, new object[] { @event });
        }
    }
}