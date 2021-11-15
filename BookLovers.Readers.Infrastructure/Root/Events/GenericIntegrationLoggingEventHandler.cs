using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using Serilog;

namespace BookLovers.Readers.Infrastructure.Root.Events
{
    internal class GenericIntegrationLoggingEventHandler<T> :
        IIntegrationEventHandler<T>,
        IIntegrationEventHandler
        where T : IIntegrationEvent
    {
        private readonly IIntegrationEventHandler<T> _decoratedHandler;
        private readonly ILogger _logger;

        public GenericIntegrationLoggingEventHandler(
            IIntegrationEventHandler<T> decoratedHandler,
            ILogger logger)
        {
            this._decoratedHandler = decoratedHandler;
            this._logger = logger;
        }

        public async Task HandleAsync(T @event)
        {
            try
            {
                this._logger.Information("Module is processing integration event of type " + @event.GetType().FullName);

                await this._decoratedHandler.HandleAsync(@event);

                this._logger.Information("Module processed successfully integration event of type " +
                                         @event.GetType().FullName);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, "Error while processing integration event of type " + @event.GetType().FullName);
                throw;
            }
        }
    }
}