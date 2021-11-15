using System;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using Serilog;

namespace BookLovers.Auth.Infrastructure.Root.Events
{
    internal class DomainEventDispatcherLoggingDecorator : IDomainEventDispatcher
    {
        private readonly IDomainEventDispatcher _decoratedDispatcher;
        private readonly ILogger _logger;

        public DomainEventDispatcherLoggingDecorator(
            IDomainEventDispatcher decoratedDispatcher,
            ILogger logger)
        {
            _decoratedDispatcher = decoratedDispatcher;
            _logger = logger;
        }

        public async Task Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            try
            {
                _logger.Information("Processing domain event of type " + @event.GetType().FullName);

                await _decoratedDispatcher.Dispatch(@event);

                _logger.Information("Event of type " + @event.GetType().FullName + " handled successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while processing " + @event.GetType().FullName);
                throw;
            }
        }
    }
}