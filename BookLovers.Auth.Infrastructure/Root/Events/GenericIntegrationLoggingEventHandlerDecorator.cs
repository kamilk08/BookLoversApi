using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using Serilog;

namespace BookLovers.Auth.Infrastructure.Root.Events
{
    internal class GenericIntegrationLoggingEventHandlerDecorator<T> :
        IIntegrationEventHandler<T>,
        IIntegrationEventHandler
        where T : IIntegrationEvent
    {
        private readonly IIntegrationEventHandler<T> _decoratedHandler;
        private readonly ILogger _logger;

        public GenericIntegrationLoggingEventHandlerDecorator(
            IIntegrationEventHandler<T> decoratedHandler,
            ILogger logger)
        {
            _decoratedHandler = decoratedHandler;
            _logger = logger;
        }

        public async Task HandleAsync(T @event)
        {
            try
            {
                _logger.Information("Processing integration event of type " + @event.GetType().FullName);

                await _decoratedHandler.HandleAsync(@event);

                _logger.Information("Event of type " + @event.GetType().FullName + " processed successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while processing " + @event.GetType().FullName);
            }
        }
    }
}