using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Librarians.Infrastructure.Persistence;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.Root.Events
{
    internal class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly LibrariansContext _context;

        public DomainEventDispatcher(LibrariansContext context)
        {
            _context = context;
        }

        public async Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var interfaceType = typeof(IDomainEventHandler<>)
                .MakeGenericType(@event.GetType());

            var implementations = CompositionRoot.Kernel.GetAll(interfaceType);

            await InvokeHandlersAsync(@event, implementations);
        }

        private async Task InvokeHandlersAsync<TEvent>(
            TEvent @event,
            IEnumerable<object> implementations)
            where TEvent : IEvent
        {
            foreach (var implementation in implementations)
            {
                if (implementation == null)
                    throw new ArgumentNullException(
                        $"Invalid domain event handler. Domain event handler cannot be null");

                await (Task) typeof(IDomainEventHandler<>)
                    .MakeGenericType(@event.GetType())
                    .GetMethod("HandleAsync")
                    .Invoke(implementation, new object[] {@event});
            }
        }
    }
}