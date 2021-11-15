using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Infrastructure.Persistence;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root.Events
{
    internal class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ReadersContext _context;

        public DomainEventDispatcher(ReadersContext context) => this._context = context;

        public async Task Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            var type = typeof(IDomainEventHandler<>);
            var typeArray = new Type[] { @event.GetType() };

            var implementations = CompositionRoot.Kernel.GetAll(type.MakeGenericType(typeArray));

            await DomainEventDispatcher.InvokeHandlersAsync(@event, implementations);
        }

        private static async Task InvokeHandlersAsync<TEvent>(
            TEvent @event,
            IEnumerable<object> implementations)
            where TEvent : IEvent
        {
            foreach (var implementation in implementations)
            {
                await (Task) typeof(IDomainEventHandler<>)
                    .MakeGenericType(@event.GetType())
                    .GetMethod("HandleAsync")
                    .Invoke(implementation, new object[] { @event });
            }
        }
    }
}