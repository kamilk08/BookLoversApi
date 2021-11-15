using System;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using Serilog;

namespace BookLovers.Readers.Infrastructure.Root.Events
{
    internal class DomainEventDispatcherLoggingDecorator : IDomainEventDispatcher
    {
        private readonly IDomainEventDispatcher _decoratedDispatcher;
        private readonly ILogger _logger;

        public DomainEventDispatcherLoggingDecorator(
            IDomainEventDispatcher decoratedDispatcher,
            ILogger logger)
        {
            this._decoratedDispatcher = decoratedDispatcher;
            this._logger = logger;
        }

        public async Task Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            try
            {
                this._logger.Information("Module is handling domain event of type " + @event.GetType().FullName);

                await this._decoratedDispatcher.Dispatch(@event);

                this._logger.Information("Module handled successfully event of type " + @event.GetType().FullName);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, "Error while processing event of type " + @event.GetType().FullName + ".");

                throw;
            }
        }
    }
}