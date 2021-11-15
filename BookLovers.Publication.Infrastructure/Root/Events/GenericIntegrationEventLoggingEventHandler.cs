using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using Serilog;

namespace BookLovers.Publication.Infrastructure.Root.Events
{
    internal class GenericIntegrationEventLoggingEventHandler<T> :
        IIntegrationEventHandler<T>,
        IIntegrationEventHandler
        where T : IIntegrationEvent
    {
        private readonly IIntegrationEventHandler<T> _decoratedHandler;
        private readonly ILogger _logger;

        public GenericIntegrationEventLoggingEventHandler(
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
                _logger.Information("Module processing integration event of type " + @event.GetType().FullName);

                await _decoratedHandler.HandleAsync(@event);

                _logger.Information("Module processed successfully integration event of type " +
                                    @event.GetType().FullName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while processing integration event of typ " + @event.GetType().FullName);
                throw;
            }
        }
    }
}